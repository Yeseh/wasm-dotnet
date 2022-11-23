using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasiOrleans.Grains;

public interface IWasiModuleResolver
{
    Task<List<string>> ListModules();

    Task<Wasmtime.Module> GetModule(string moduleName);

    ValueTask Refresh();

    ValueTask Reload(string moduleName);
    
    ValueTask<Wasmtime.Engine> GetEngine();
}