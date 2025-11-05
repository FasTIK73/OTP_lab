using OTP;

namespace OTP
{
    class Program
    {
        static void Main()
        {
            // Создаем зависимости
            INotificationSender notificationSender = new PostNotificationSender();
            IViolationHandlerFactory violationFactory = new ViolationHandlerFactory();
            IMessageValidator validator = new MessageValidator();
            IMessageFormatter formatter = new MessageFormatter();

            // Внедряем зависимости через конструктор
            var notificationService = new TrafficPoliceNotificationService(
                notificationSender: notificationSender,
                violationFactory: violationFactory,
                messageValidator: validator,
                messageFormatter: formatter);

            // Альтернативно: можно использовать значения по умолчанию
            // var notificationService = new TrafficPoliceNotificationService();

            notificationService.DistributeMessageToPosts(
                1234,
                "Москва",
                true,
                DateTime.Now,
                new List<string> { "Пост_1", "Пост_2" },
                "Центральный отдел",
                "Дополнительная информация о происшествии");
            notificationService.ProcessTrafficViolation(TrafficViolationType.Speeding);
            notificationService.ProcessTrafficViolation(TrafficViolationType.RedLightRunning);
            notificationService.ProcessTrafficViolation(1);

            notificationService.BroadcastRegionalAlert(3, "Санкт-Петербург", new[] {
                "Адмиралтейский район", "Приморский район" });

            // Демонстрация гибкости - можно легко подменить реализации
            TestWithCustomDependencies();
        }

        static void TestWithCustomDependencies()
        {
            Console.WriteLine("\n--- Тест с кастомными зависимостями ---");

            // Кастомная реализация для тестирования
            var testNotificationSender = new TestNotificationSender();
            var testViolationFactory = new TestViolationHandlerFactory();

            var testService = new TrafficPoliceNotificationService(
                notificationSender: testNotificationSender,
                violationFactory: testViolationFactory);

            testService.ProcessTrafficViolation(TrafficViolationType.Speeding);
        }
    }

    // Тестовые реализации для демонстрации слабой связанности
    public class TestNotificationSender : INotificationSender
    {
        public void Send(string message)
        {
            Console.WriteLine($"[ТЕСТ] Отправка сообщения: {message}");
        }
    }

    public class TestViolationHandlerFactory : IViolationHandlerFactory
    {
        public IViolationHandler CreateHandler(TrafficViolationType violationType)
        {
            return new TestViolationHandler();
        }
    }

    public class TestViolationHandler : IViolationHandler
    {
        public void ProcessViolation()
        {
            Console.WriteLine("[ТЕСТ] Обработка нарушения");
        }
    }
}