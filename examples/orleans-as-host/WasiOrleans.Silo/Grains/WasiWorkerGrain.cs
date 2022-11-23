
using System.Threading;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Orleans;
using WasiOrleans.Grains;
using System;

[StatelessWorker]
public abstract class WasiGrain : Grain, IWasiWorkerGrain, IDisposable
{
    protected readonly IWasiModuleResolver _moduleResolver;

    private Wasmtime.Store? _store = null!; 

    private Wasmtime.Linker? _linker = null!; 
    
    private Wasmtime.Instance? _instance = null!;

    private Wasmtime.Function? _func = null!; 
    
    protected WasiGrain(IWasiModuleResolver moduleResolver) : base()
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

        _func = _instance.GetFunction("hello");

        await base.OnActivateAsync(cancellationToken);
    }

    public ValueTask CallAsync()
    {
        if (_func == null) { throw new ApplicationException("WASI Function is not initialized"); }

        _func.Invoke();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        if (_linker != null) { _linker.Dispose(); }
        if (_store != null) { _store.Dispose(); }
    }
}