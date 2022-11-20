using Wasmtime;

// EXAMPLE: Initialize Wasmtime, and execute a WASI function 
//          Set environment variables in the WASI module

// Initialize Wasmtime context
using var engine = new Engine();
using var store = new Store(engine);
using var linker = new Linker(engine);

var wasiConfig = new WasiConfiguration()
    // Write WASI stdout to host stdout
    .WithInheritedStandardOutput()
    // Write WASI stderr to host stderr
    .WithInheritedStandardError()
    // Only give access to DOTNET_VERSION env var 
    .WithEnvironmentVariables(new (string, string)[]{
        ("DOTNET_VERSION", "7")
    });

store.SetWasiConfiguration(wasiConfig);
// Inject expected WASI imports such as fd_read
linker.DefineWasi();

// Load a module, this operation does expensive AOT compilation of the module
var modulePathBase = "../../../../../modules/compiled";
var modulePath = Path.Join(modulePathBase, "rust.wasm");
using var module = Module.FromFile(engine, modulePath);

// Add host functions to the WASI module
linker.DefineFunction("host", "dotnet_hello", () => Console.WriteLine("Hello dotnet, from Rust"));
// With a parameter
linker.DefineFunction("host", "dotnet_hello_param", (string s) => Console.WriteLine($"Hello dotnet, from {s}"));
// With a C# struct
linker.DefineFunction("host", "dotnet_hello_param", (string s) => Console.WriteLine($"Hello dotnet, from {s}"));



// Retrieve the exported function 'hello' from the rust module
var instance = linker.Instantiate(store, module);

var hello = instance.GetFunction("hello");

if (hello == null) { throw new Exception("Function not found"); }

hello.Invoke();

Console.ReadLine();

public partial class Program { }