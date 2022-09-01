using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.Kafka;
using Confluent.Kafka;

namespace AbpKafkaDemo {
    [DependsOn(typeof(Volo.Abp.Kafka.AbpKafkaModule))]
    public class WorkerServiceModule : AbpModule {
        public override void ConfigureServices(ServiceConfigurationContext context) {
            context.Services.AddHostedService<Topic1HostedService>();
            context.Services.AddHostedService<Topic2HostedService>();
            Configure<AbpKafkaOptions>(options => {
                options.ConfigureConsumer = config => {
                    config.AutoOffsetReset = AutoOffsetReset.Earliest;
                    config.EnablePartitionEof = true;
                };
                options.ConfigureProducer = config => {
                    config.Acks = Acks.All;
                    config.EnableIdempotence = true;
                    config.LingerMs = 100;
                };
            });
        }
    }
}
