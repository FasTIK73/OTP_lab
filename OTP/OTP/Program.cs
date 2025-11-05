using OTP;

namespace OTP
{
    class Program
    {
        static void Main()
        {
            var notificationService = new TrafficPoliceNotificationService();

            notificationService.DistributeMessageToPosts(
                1234, // Идентификатор поста
                "Москва", // Местоположение
                true, // Флаг чрезвычайной ситуации
                DateTime.Now, // Время события
                new List<string> { "Пост_1", "Пост_2" }, // Список получателей
                "Центральный отдел",
                "Дополнительная информация о происшествии");

            // Демонстрация метода с переключением на случай нарушения ПДД
            notificationService.ProcessTrafficViolation(1); // Нарушение номер 1 (превышение скорости)
            notificationService.BroadcastRegionalAlert(3, "Санкт-Петербург", new[] {
                "Адмиралтейский район", "Приморский район" });
        }
    }
}