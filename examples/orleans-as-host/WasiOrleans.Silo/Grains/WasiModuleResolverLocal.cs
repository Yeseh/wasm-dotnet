using WasiOrleans.Grains;
using Orleans.Concurrency;
using Orleans.Runtime;
using Wasmtime;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

[Reentrant]
public class WasiModuleResolverLocal : GrainService, IWasiModuleResolver, IDisposable
{
    // TODO: For now, use single WASI context
    public Wasmtime.Engine Engine { get; private set; } = null!;

    private Dictionary<String, Wasmtime.Module> _modules = new();
            
    private const string ModuleBasePath = "C:\\Users\\JesseWellenberg\\repo\\wasm-dotnet\\modules\\compiled";

    public ValueTask<Engine> GetEngine() => ValueTask.FromResult(Engine);

    public override async Task Init(IServiceProvider serviceProvider)
    {
        Engine = new();
        _modules = new();

       var modules = await ListModules(); 

        foreach (var path in modules)
        {
            var modulePath = Path.Join(ModuleBasePath, path);
            var module = Wasmtime.Module.FromFile(Engine, path);
            _modules.Add(path, module);
        }

        await base.Init(serviceProvider);
    }

    public Task<Wasmtime.Module> GetModule(string modulename)
    {
        var bfound = _modules.TryGetValue(modulename, out var module);
        if (!bfound || module == null) { throw new ArgumentException("Module not found"); }

        return Task.FromResult(module);
    }

    new public void Dispose()
    {
        if (Engine is not null) { Engine.Dispose(); } 

        base.Dispose();
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