namespace OTP
{
    public abstract class NotificationService
    {
        public static void NotifyPosts(string message)
        {
            Console.WriteLine("Сообщение для всех постов ГИБДД: {0}", message);
        }

        protected string FormatNotificationMessage(int postId, string location, string senderName)
        {
            return $"Сообщение от поста №{postId}, местоположение: {location}. Отправитель: {senderName}";
        }

        protected string FormatAlertMessage(int alertLevel, string region, string area)
        {
            return $"Спецуведомление уровня {alertLevel}: регион {region}, затронута область {area}";
        }

        protected void SendBulkNotifications(List<string> recipients, string message)
        {
            foreach (var recipient in recipients)
            {
                NotifyPosts(message);
            }
        }
    }

    public class GIBDDPostNotificationSystem : NotificationService
    {
        public string SendMessageToAllPosts(
            int postId,
            string location,
            bool isEmergency,
            DateTime timestamp,
            List<string> recipientsList,
            string senderName,
            string additionalInfo = null)
        {
            if (!IsValidRecipientList(isEmergency, recipientsList))
            {
                return GetErrorMessage(isEmergency, recipientsList);
            }

            string notificationMessage = FormatNotificationMessage(postId, location, senderName);
            SendBulkNotifications(recipientsList, notificationMessage);

            return "Сообщение успешно доставлено";
        }

        private bool IsValidRecipientList(bool isEmergency, List<string> recipientsList)
        {
            return (!isEmergency && recipientsList.Count > 0) ||
                   (isEmergency && recipientsList.Count > 0);
        }

        private string GetErrorMessage(bool isEmergency, List<string> recipientsList)
        {
            if (isEmergency && recipientsList.Count <= 0)
            {
                return "Отсутствуют получатели для экстренного уведомления!";
            }
            return "Сообщение успешно отправлено.";
        }

        public void HandleTrafficViolation(int violationCode)
        {
            string violationDescription = GetViolationDescription(violationCode);
            Console.WriteLine(violationDescription);
        }

        private string GetViolationDescription(int violationCode)
        {
            switch (violationCode)
            {
                case 1:
                    return "Превышение скорости";
                case 2:
                    return "Проезд на красный свет";
                default:
                    return "Неизвестное нарушение";
            }
        }

        public void SendSpecialAlert(int alertLevel, string region, string[] affectedAreas)
        {
            foreach (var area in affectedAreas)
            {
                string alertMessage = FormatAlertMessage(alertLevel, region, area);
                NotifyPosts(alertMessage);
            }
        }
    }
}