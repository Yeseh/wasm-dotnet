using Wasmtime;

var modulePathBase = "modules/compiled";

if (args.Length < 2) { throw new ArgumentException("Invalid args"); }

var moduleName = args[0];
var functionName = args[1];
var modulePath = Path.Join(modulePathBase, $"{moduleName}.wasm");
var environmentVars = new (string, string)[] 
{
    ("DOTNET_VERSION", "7")
};

using var engine = new Engine();

var wasiConfig= new WasiConfiguration()
    .WithInheritedStandardInput()
    .WithInheritedStandardOutput()
    .WithInheritedStandardError()
    .WithEnvironmentVariables(environmentVars);

using var store = new Store(engine);
store.SetWasiConfiguration(wasiConfig);

using var linker = new Linker(engine);
linker.DefineWasi();

using var module = Module.FromFile(engine, modulePath);

var instance = linker.Instantiate(store, module);
var hello = instance.GetFunction(functionName);

if (hello == null) { throw new Exception("Function not found"); }

hello.Invoke();