using AbpKafkaDemo;
using Volo.Abp;

IHost host = Host.CreateDefaultBuilder(args)
                 .ConfigureServices(services => {
                     services.AddApplication<WorkerServiceModule>();
                 })
                 .UseConsoleLifetime().Build();
var application = host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
await application.InitializeAsync(host.Services);
await host.RunAsync();
