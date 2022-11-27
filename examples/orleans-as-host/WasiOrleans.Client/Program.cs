using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WasiOrleans.Grains;

using var host = new HostBuilder()
    .UseOrleansClient(cb => {
        cb.UseLocalhostClustering();
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();

await host.StartAsync();

var worker = client.GetGrain<ISimpleGrain>(0);

Console.ReadLine();
//await worker.CallAsync();
//Console.ReadLine();
