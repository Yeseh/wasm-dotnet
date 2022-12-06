using Wasmtime;

using var engine = new Engine(); 
using var store = new Store(engine);
using var linker = new Linker(engine);
using var module = Module.FromFile(engine, "./guest/bin/Debug/net7.0/function-imports-exports-guest.wasm");

store.SetWasiConfiguration(new WasiConfiguration()
    .WithInheritedStandardInput()
    .WithInheritedStandardOutput()
    .WithInheritedStandardError()
);

linker.DefineWasi();

linker.DefineFunction(
    "host", 
    "host_hello", 
    () => Console.WriteLine("Hello from host!"));


var instance = linker.Instantiate(store, module);

var start = instance.GetAction("_start");
var guestHello = instance.GetAction("guest_hello");

start!.Invoke();
guestHello!.Invoke();