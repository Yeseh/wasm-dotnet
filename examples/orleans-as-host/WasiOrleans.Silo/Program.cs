using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WasiOrleans.Grains;
using Microsoft.Extensions.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, silo) =>
    {
        var instanceId = ctx.Configuration.GetValue<int>("InstanceId");

        silo.UseLocalhostClustering();

        //silo.UseDashboard();

        // Enable distributed logging;
        silo.AddActivityPropagation();

        // TODO: As GrainService?
        silo.ConfigureServices(s =>
        {
            s.AddSingleton<IModuleResolverService, LocalModuleResolver>();
        });
    }).Build();

var moduleResolver = host.Services.GetRequiredService<IModuleResolverService>();

await moduleResolver.Init();

await host.RunAsync();