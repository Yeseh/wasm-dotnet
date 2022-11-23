using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using WasiOrleans;

await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, silo) =>
    {
        var instanceId = ctx.Configuration.GetValue<int>("InstanceId");
        var port = 11_111;

        silo.UseLocalhostClustering();

        silo.UseDashboard();

        // Enable distributed logging;
        silo.AddActivityPropagation();
    })
    // .ConfigureWebHostDefaults(webBuilder => {
    //     webBuilder.UseStartup<Startup>();

    // })
    .RunConsoleAsync();