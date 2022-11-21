using Wasmtime;

// EXAMPLE: Import host functions into the wasi module 
//          Set environment variables in the WASI module

using var engine = new Engine();
using var store = new Store(engine);
using var linker = new Linker(engine);

var wasiConfig = new WasiConfiguration()
    .WithInheritedStandardOutput()
    .WithInheritedStandardError();

store.SetWasiConfiguration(wasiConfig);
// Inject expected WASI imports such as fd_read
linker.DefineWasi();

// Load a module, this operation does expensive AOT compilation of the module
var modulePathBase = "../../../../../modules/compiled";
var modulePath = Path.Join(modulePathBase, "rust.wasm");
using var module = Module.FromFile(engine, modulePath);

// Retrieve the exported function 'hello' from the rust module
var instance = linker.Instantiate(store, module);
var hello = instance.GetFunction("hello");

if (hello == null) { throw new Exception("Function not found"); }

hello.Invoke();

Console.ReadLine();

public partial class Program { }