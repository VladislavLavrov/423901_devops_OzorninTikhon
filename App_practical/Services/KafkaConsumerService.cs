using Confluent.Kafka;
using App_practical.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using App_practical.Controllers;
using App_practical.Data;
using App_practical.Libraries;

namespace App_practical.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _clientFactory;

        public KafkaConsumerService(IConfiguration config, IServiceProvider serviceProvider, IHttpClientFactory clientFactory) {
            // Конфигурирование настроек Kafka и инициализация компонентов
            var consumerConfig = new ConsumerConfig();
            config.GetSection("Kafka:ConsumerSettings").
                Bind(consumerConfig);
            _topic = config.GetValue<string>("Kafka:TopicName");
            _kafkaConsumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _serviceProvider = serviceProvider;
            _clientFactory = clientFactory;
        }
        /// <summary>
        /// Выполнение работы Kafka Consumer’а.
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }
        /// <summary>
        /// Цикл обработки сообщений из Kafka.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            Console.WriteLine("StartConsumerLoop Start");
            _kafkaConsumer.Subscribe(_topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("StartConsumerLoop Start while");
                try
                {
                    Console.WriteLine("StartConsumerLoop Before Consume");
                    var cr = _kafkaConsumer.Consume(cancellationToken);
                    Console.WriteLine("StartConsumerLoop After Consume");
                    var ip = cr.Message.Value;
                    // Исходные данные
                    var inputData = JsonSerializer.Deserialize<Variant>(cr.Message.Value);
                    // Выполнение расчета
                    var result = CalculationLibrary.Calculate(inputData.Value1, inputData.Value2, inputData.Operation);
                    inputData.Result = result;
                    var httpClient = _clientFactory.CreateClient();
                    Console.WriteLine("StartConsumerLoop Before await PostAsJsonAsync");
                    await httpClient.PostAsJsonAsync("http://localhost:5009/Home/Callback", inputData);
                    // Обработка сообщения...
                    Console.WriteLine($"Message key: {cr.Message.Key}, value: {cr.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("StartConsumerLoop OperationCanceledException");
                    break;
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine("StartConsumerLoop ConsumeException " + e.Message);
                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("StartConsumerLoop Exception " + e.Message);
                    break;
                }
                Console.WriteLine("StartConsumerLoop End while");
            }
            Console.WriteLine("StartConsumerLoop End");
        }
        /// <summary>
        /// Очистка ресурсов Consumer’а при завершении работы сервиса.
        /// </summary>
        public override void Dispose()
        {
            _kafkaConsumer.Close(); // Фиксация оффсетов и корректное выход из группы.
            _kafkaConsumer.Dispose();
            base.Dispose();
        }
    }
}