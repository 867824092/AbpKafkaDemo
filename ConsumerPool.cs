using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Kafka;

namespace AbpKafkaDemo {
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IConsumerPool))]
    internal class ConsumerPool : Volo.Abp.Kafka.ConsumerPool {
        public ConsumerPool(IOptions<AbpKafkaOptions> options) : base(options) {
        }
        private static readonly object _lock = new();
        public override IConsumer<string, byte[]> Get(string groupId, string connectionName = null) {
            connectionName ??= KafkaConnections.DefaultConnectionName;
            return Consumers.GetOrAdd(
                connectionName, connection => new Lazy<IConsumer<string, byte[]>>(() => {
                    lock (_lock) {
                        var config = new ConsumerConfig(Options.Connections.GetOrDefault(connection)) {
                            GroupId = groupId,
                            EnableAutoCommit = false,
                            PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky //消费者粘性策略
                        };
                        Options.ConfigureConsumer?.Invoke(config);
                        return new ConsumerBuilder<string, byte[]>(config).Build();
                    }
                })
            ).Value;
        }
    }
}
