using System.Threading.Tasks;
using Orleans;

namespace WasiOrleans.Grains;

public interface IWasiWorkerGrain : IGrainWithIntegerKey
{
    ValueTask CallAsync();
}