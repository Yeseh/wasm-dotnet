using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using WasiOrleans.Grains;

using var host = new HostBuilder()
    .UseOrleansClient(cb => {
        cb.UseLocalhostClustering(gatewayPort: 5000);
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();

await host.StartAsync();

var worker = client.GetGrain<IWasiWorkerGrain>(0);

Console.ReadLine();
await worker.CallAsync();
Console.ReadLine();
