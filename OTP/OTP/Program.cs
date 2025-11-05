using OTP;

namespace OTP
{
    class Program
    {
        static void Main()
        {
            var notificationService = new TrafficPoliceNotificationService();

            notificationService.DistributeMessageToPosts(
                1234,
                "Москва",
                true,
                DateTime.Now,
                new List<string> { "Пост_1", "Пост_2" },
                "Центральный отдел",
                "Дополнительная информация о происшествии");

            // Использование enum для типов нарушений
            notificationService.ProcessTrafficViolation(TrafficViolationType.Speeding);
            notificationService.ProcessTrafficViolation(TrafficViolationType.RedLightRunning);

            // Обратная совместимость с int
            notificationService.ProcessTrafficViolation(1);

            notificationService.BroadcastRegionalAlert(3, "Санкт-Петербург", new[] {
                "Адмиралтейский район", "Приморский район" });
        }
    }
}