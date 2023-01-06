public interface IModuleHost 
{
    Task Init();

    WasmContext GetContext();
}
