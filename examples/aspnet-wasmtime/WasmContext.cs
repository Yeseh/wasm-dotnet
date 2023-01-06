using Wasmtime;

public class WasmContext
{
    public readonly Store Store;

    public readonly Instance Instance;

    public WasmContext(Store store, Instance instance)
    {
        Store = store;
        Instance = instance;
    }
}