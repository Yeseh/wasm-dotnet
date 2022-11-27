using System.Threading.Tasks;
using Orleans;

namespace WasiOrleans.Grains;

public interface IWorkerGrain : IGrainWithIntegerKey 
{
    ValueTask CallAsync();
}