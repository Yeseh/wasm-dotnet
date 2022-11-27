using WasiOrleans.Grains;

namespace WasiOrleans.Silo;

using System.Threading;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Orleans;
using System;

[StatelessWorker]
public class WasiWorkerGrain : Grain, IWorkerGrain, IDisposable
{
    protected readonly IModuleResolverService _moduleResolver;

    private Wasmtime.Store? _store = null!;

    private Wasmtime.Linker? _linker = null!;

    private Wasmtime.Instance? _instance = null!;

    private Wasmtime.Function? _entryPoint = null!;

    public WasiWorkerGrain(IModuleResolverService moduleResolver)
    {
        _moduleResolver = moduleResolver;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        //TODO: Multiple module support?
        var module = await _moduleResolver.GetModule("rust.wasm");
        var engine = await _moduleResolver.GetEngine();

        var wasiConfig = new Wasmtime.WasiConfiguration()
            .WithInheritedStandardOutput()
            .WithInheritedStandardError();

        _linker = new(engine);
        _store = new(engine);

        _linker.DefineWasi();
        _store.SetWasiConfiguration(wasiConfig);

        //TODO: Add linker extension method to attach host calls 
        _instance = _linker.Instantiate(_store, module);
        _entryPoint = _instance.GetFunction("_start");
        if (_entryPoint == null) { throw new ApplicationException("WASI _start function is not exported"); }

        await base.OnActivateAsync(cancellationToken);
    }

    public ValueTask CallAsync()
    {
        if (_entryPoint == null) { throw new ArgumentNullException(nameof(_entryPoint)); }
        _entryPoint.Invoke();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        if (_linker != null) { _linker.Dispose(); }
        if (_store != null) { _store.Dispose(); }
        GC.SuppressFinalize(this);
    }
}