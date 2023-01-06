using Wasmtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new Engine(new Config()
    .WithReferenceTypes(true)
));
builder.Services.AddSingleton<IModuleHost, ModuleHost>();

var app = builder.Build();


app.MapGet("/", (IModuleHost _moduleHost) => {
    var ctx = _moduleHost.GetContext();
    var hello = ctx.Instance.GetFunction("hello");
    hello?.Invoke();
});


var moduleHost = app.Services.GetRequiredService<IModuleHost>();
await moduleHost.Init();

app.Run();
