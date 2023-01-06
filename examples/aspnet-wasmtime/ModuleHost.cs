using Wasmtime;

public class ModuleHost : IModuleHost
{
    private readonly Engine _engine;

    private Linker? _linker;

    private Module? _module;

    private WasiConfiguration? _wasiConfig;

    public ModuleHost(Engine engine)
    {
        _engine = engine; 
    }

    public WasmContext GetContext()
    {
        var uninitialized = _linker == null 
            || _module == null 
            || _wasiConfig == null;

        if (uninitialized) { throw new Exception("Modulehost not initialized"); }

        var store = new Store(_engine);
        store.SetWasiConfiguration(_wasiConfig!);

        var instance = _linker!.Instantiate(store, _module!);

        return new(store, instance);
    }

    // Read and initialize modules from FS / Pull from OCI / Downlad from blobstore etc.
    public Task Init()
    {
        _wasiConfig = new WasiConfiguration()
            .WithInheritedStandardOutput()
            .WithInheritedStandardError();

        _module = Module.FromFile(_engine, "C:\\Users\\JesseWellenberg\\repo\\wasm-dotnet\\modules\\compiled\\rust.wasm");

        _linker = new Linker(_engine);
        _linker.DefineWasi();
        _linker.DefineFunction("dotnet", "dotnet_hello", () => Console.WriteLine("Hello Host!"));
        _linker.DefineFunction("dotnet", "dotnet_hello_params", (int test, float test2) => Console.WriteLine("Hello Host!"));
        _linker.DefineFunction("dotnet", "mem_read", (int a, int b) => 1);

        return Task.CompletedTask;
    }
}