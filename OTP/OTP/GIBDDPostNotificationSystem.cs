namespace OTP
{
    public enum TrafficViolationType
    {
        Speeding = 1,
        RedLightRunning = 2,
        Unknown = 0
    }

    public interface IViolationHandler
    {
        void ProcessViolation();
    }

    public class SpeedingViolationHandler : IViolationHandler
    {
        public void ProcessViolation()
        {
            Console.WriteLine("Превышение скорости");
        }
    }

    public class RedLightViolationHandler : IViolationHandler
    {
        public void ProcessViolation()
        {
            Console.WriteLine("Проезд на красный свет");
        }
    }

    public class UnknownViolationHandler : IViolationHandler
    {
        public void ProcessViolation()
        {
            Console.WriteLine("Неизвестное нарушение");
        }
    }

    public class ViolationHandlerFactory
    {
        public IViolationHandler CreateHandler(TrafficViolationType violationType)
        {
            switch (violationType)
            {
                case TrafficViolationType.Speeding:
                    return new SpeedingViolationHandler();
                case TrafficViolationType.RedLightRunning:
                    return new RedLightViolationHandler();
                default:
                    return new UnknownViolationHandler();
            }
        }
    }

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
        private readonly ViolationHandlerFactory _violationFactory;

        public TrafficPoliceNotificationService()
        {
            _violationFactory = new ViolationHandlerFactory();
        }

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

        public void ProcessTrafficViolation(TrafficViolationType violationType)
        {
            IViolationHandler handler = _violationFactory.CreateHandler(violationType);
            handler.ProcessViolation();
        }

        // Перегрузка для обратной совместимости
        public void ProcessTrafficViolation(int violationTypeCode)
        {
            TrafficViolationType violationType = (TrafficViolationType)violationTypeCode;
            ProcessTrafficViolation(violationType);
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