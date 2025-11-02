namespace OTP
{
    class Program
    {
        static void Main()
        {
            var notificationSystem = new GIBDDPostNotificationSystem();

            notificationSystem.SendMessageToAllPosts(
            1234, // Идентификатор поста
            "Москва", // Местоположение
            true, // Флаг чрезвычайной ситуации
            DateTime.Now, // Время события
            new List<string> { "Пост_1", "Пост_2" }, // Список получателей
            "Центральный отдел",
            "Дополнительная информация о происшествии"); // Дополнительная
            //информация
        // Демонстрация метода с переключением на случай нарушения ПДД
 notificationSystem.HandleTrafficViolation(1); // Нарушение номер 1
            //(превышение скорости)
 notificationSystem.SendSpecialAlert(3, "Санкт-Петербург", new[] {
"Адмиралтейский район", "Приморский район" });
        }
    }
}