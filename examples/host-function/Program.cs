using System.Runtime.InteropServices;
using Wasmtime;

// EXAMPLE: Initialize Wasmtime, and execute a WASI function 
//          Set environment variables in the WASI module

// Initialize Wasmtime context

[StructLayout(LayoutKind.Sequential)]
struct Input {
    public int i;
    public float f;

    public Input(int i, float f)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Console.WriteLine("Actually big endian lmao");
            var ibytes = BitConverter.GetBytes(i);
            var fbytes = BitConverter.GetBytes(f);
            Array.Reverse(ibytes, 0, ibytes.Length);
            Array.Reverse(fbytes, 0, fbytes.Length);
            i = BitConverter.ToInt32(ibytes);
            f = BitConverter.ToSingle(fbytes);
        }
    }
}

public class Program
{
    public static int Main(string[] args)
    {
        using var engine = new Engine();
        using var store = new Store(engine);
        using var linker = new Linker(engine);

        var wasiConfig = new WasiConfiguration()
            .WithInheritedStandardOutput()
            .WithInheritedStandardError();

        store.SetWasiConfiguration(wasiConfig);
        linker.DefineWasi();

        var modulePathBase = "../../modules/compiled";
        var modulePath = Path.Join(modulePathBase, "rust.wasm");
        using var module = Module.FromFile(engine, modulePath);

        /* 
            Add some imports to the module from C# callbacks
            These can be imported in the module using FFI syntax
            A Rust example would look like:

            #[link(wasm_import_module = "dotnet")]
            extern "C" {
                fn dotnet_hello();
                fn dotnet_hello_params(i: i32, f: f32)
            }
        */
        linker.DefineFunction(
            "dotnet", "dotnet_hello", 
            () => Console.WriteLine("Hello dotnet, from WASI"));

        // With a parameter
        linker.DefineFunction(
            "dotnet", "dotnet_hello_params", 
            (int i, float f) => Console.WriteLine($"Got int: {i} float: {f} from WASI!"));

        linker.DefineFunction(
            "dotnet", "mem_read",
            (Caller caller, int ptr, int len) => {
                var mem = caller.GetMemory("memory");
                // Exception is not a thing known in WASI
                // What happens?
                if (mem == null) { throw new Exception("No exported memory found"); }

                unsafe {
                    var data = mem.GetSpan(ptr, len).
                    return &data;
                }

                return &span.ToArray();
            }
        );

        // Retrieve the exported function 'hello' from the rust module
        var instance = linker.Instantiate(store, module);
        // var fn = instance.GetFunction("host_functions");
        // if (fn == null) { throw new Exception("Function not found"); }

        // fn.Invoke();

        // Example 3: Working with Memory
        // WASM Memory is divided into pages of 65KiB each;
        // https://adlrocha.substack.com/p/adlrocha-playing-with-wasmtime-and ??
        var mem = instance.GetMemory("memory");
        if (mem == null) { throw new Exception("No exported memory found"); }

        Console.WriteLine($"Initial memory size: {mem.GetSize()}");
        Console.WriteLine($"Initial ptr: {mem.GetPointer()}");
        Console.WriteLine($"Initial length: {mem.GetLength()}");

        var input = new Input(10, 6.5f);
        Console.WriteLine($"Input len: {Marshal.SizeOf<Input>()}");
        
        mem.Write<Input>(0, input);

        var read_struct = instance.GetFunction<float>("read_struct_raw");
        if (read_struct == null) { throw new Exception("Read struct not found"); }
        var result = read_struct.Invoke();

        Console.WriteLine("Result: ", result);

        Console.ReadLine();
        return 0;
    }
}