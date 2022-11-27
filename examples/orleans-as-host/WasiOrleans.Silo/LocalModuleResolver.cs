using Orleans.Concurrency;
using Wasmtime;
using WasiOrleans.Grains;

[Reentrant]
public class LocalModuleResolver : IModuleResolverService, IDisposable
{
    // TODO: For now, use single WASI context
    public Engine Engine { get; private set; } = null!;

    private Dictionary<string, Module> _modules = new();
            
    private const string ModuleBasePath = "C:\\Users\\JesseWellenberg\\repo\\wasm-dotnet\\modules\\compiled";

    public ValueTask<Engine> GetEngine() => ValueTask.FromResult(Engine);
    
    public LocalModuleResolver()
    {
        Engine = new();
        _modules = new();
    }

    public async Task Init()
    {
       var modules = await ListModules(); 

        foreach (var path in modules)
        {
            var modulePath = Path.Join(ModuleBasePath, path);
            var module = Module.FromFile(Engine, modulePath);
            _modules.Add(path, module);
        }
    }

    public Task<Wasmtime.Module> GetModule(string modulename)
    {
        var bfound = _modules.TryGetValue(modulename, out var module);
        if (!bfound || module == null) { throw new ArgumentException("Module not found"); }

        return Task.FromResult(module);
    }

    public void Dispose()
    {
        if (Engine is not null) { Engine.Dispose(); }
        GC.SuppressFinalize(this);
    }

    public Task<List<string>> ListModules()
    {
        var modules = new List<string>() { "rust.wasm" };
        return Task.FromResult(modules);
    }

    public ValueTask Refresh()
    {
        throw new NotImplementedException();
    }

    public ValueTask Reload(string moduleName)
    {
        throw new NotImplementedException();
    }
}