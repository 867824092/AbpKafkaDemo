using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Kafka;

namespace AbpKafkaDemo {
    internal class Topic2HostedService : IHostedService {
        private IKafkaMessageConsumerFactory MessageConsumerFactory { get; }
        protected IKafkaMessageConsumer KafkaMessageConsumer { get; private set; }
        readonly string GroupId = "cg-2";
        readonly string Topic = "topic2";
        public Topic2HostedService(IKafkaMessageConsumerFactory messageConsumerFactory) {
            MessageConsumerFactory = messageConsumerFactory;
            //初始化Kafka消费者
            KafkaMessageConsumer = MessageConsumerFactory.Create(Topic, GroupId, Topic);
        }
        public Task StartAsync(CancellationToken cancellationToken) {
            KafkaMessageConsumer.OnMessageReceived(async message => {
                await Task.Delay(100);
                Console.WriteLine($"ConsumerGroup：{GroupId} Receives  the message of Topic: [{Topic}], Msg : [{Encoding.UTF8.GetString(message.Value)}]");
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }
}
