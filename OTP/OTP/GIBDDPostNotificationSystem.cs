namespace OTP
{
    public abstract class NotificationBase
    {
        protected static void SendNotification(string message)
        {
            Console.WriteLine("Сообщение для всех постов ГИБДД: {0}", message);
        }

        protected string CreateStandardMessage(int postId, string location, string department)
        {
            return $"Сообщение от поста №{postId}, местоположение: {location}. Отдел: {department}";
        }

        protected string CreateAlertMessage(int alertLevel, string region, string district)
        {
            return $"Спецуведомление уровня {alertLevel}: регион {region}, затронут район {district}";
        }

        protected void SendMultipleNotifications(List<string> recipients, string messageTemplate)
        {
            foreach (var recipient in recipients)
            {
                string personalizedMessage = $"{messageTemplate} | Получатель: {recipient}";
                SendNotification(personalizedMessage);
            }
        }
    }

    public class TrafficPoliceNotificationService : NotificationBase
    {
        public string DistributeMessageToPosts(
            int postIdentifier,
            string postLocation,
            bool isUrgentSituation,
            DateTime eventTime,
            List<string> recipientPosts,
            string departmentName,
            string incidentDetails = null)
        {
            if (!ValidateRecipients(isUrgentSituation, recipientPosts))
            {
                return GetDistributionErrorMessage(isUrgentSituation, recipientPosts);
            }

            string notificationContent = CreateStandardMessage(postIdentifier, postLocation, departmentName);
            SendMultipleNotifications(recipientPosts, notificationContent);

            return "Сообщение успешно доставлено всем постам";
        }

        private bool ValidateRecipients(bool isUrgentSituation, List<string> recipientPosts)
        {
            return (!isUrgentSituation && recipientPosts.Count > 0) ||
                   (isUrgentSituation && recipientPosts.Count > 0);
        }

        private string GetDistributionErrorMessage(bool isUrgentSituation, List<string> recipientPosts)
        {
            if (isUrgentSituation && recipientPosts.Count <= 0)
            {
                return "Требуются получатели для экстренного уведомления!";
            }
            return "Сообщение успешно отправлено.";
        }

        public void ProcessTrafficViolation(int violationTypeCode)
        {
            string violationDetails = GetViolationTypeDescription(violationTypeCode);
            Console.WriteLine(violationDetails);
        }

        private string GetViolationTypeDescription(int violationTypeCode)
        {
            switch (violationTypeCode)
            {
                case 1:
                    return "Превышение скорости";
                case 2:
                    return "Проезд на красный свет";
                default:
                    return "Неизвестное нарушение";
            }
        }

        public void BroadcastRegionalAlert(int alertSeverity, string regionName, string[] affectedDistricts)
        {
            foreach (var district in affectedDistricts)
            {
                string alertContent = CreateAlertMessage(alertSeverity, regionName, district);
                SendNotification(alertContent);
            }
        }
    }
}