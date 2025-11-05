namespace OTP
{
    public enum TrafficViolationType
    {
        Speeding = 1,
        RedLightRunning = 2,
        Unknown = 0
    }

    public interface INotificationSender
    {
        void Send(string message);
    }

    public class PostNotificationSender : INotificationSender
    {
        public void Send(string message)
        {
            Console.WriteLine("Сообщение для всех постов ГИБДД: {0}", message);
        }
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

    public interface IViolationHandlerFactory
    {
        IViolationHandler CreateHandler(TrafficViolationType violationType);
    }

    public class ViolationHandlerFactory : IViolationHandlerFactory
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

    public interface IMessageValidator
    {
        bool ValidateRecipients(bool isUrgentSituation, List<string> recipientPosts);
        string GetErrorMessage(bool isUrgentSituation, List<string> recipientPosts);
    }

    public class MessageValidator : IMessageValidator
    {
        public bool ValidateRecipients(bool isUrgentSituation, List<string> recipientPosts)
        {
            return (!isUrgentSituation && recipientPosts.Count > 0) ||
                   (isUrgentSituation && recipientPosts.Count > 0);
        }

        public string GetErrorMessage(bool isUrgentSituation, List<string> recipientPosts)
        {
            if (isUrgentSituation && recipientPosts.Count <= 0)
            {
                return "Требуются получатели для экстренного уведомления!";
            }
            return "Сообщение успешно отправлено.";
        }
    }

    public interface IMessageFormatter
    {
        string FormatStandardMessage(int postId, string location, string department);
        string FormatAlertMessage(int alertLevel, string region, string district);
    }

    public class MessageFormatter : IMessageFormatter
    {
        public string FormatStandardMessage(int postId, string location, string department)
        {
            return $"Сообщение от поста №{postId}, местоположение: {location}. Отдел: {department}";
        }

        public string FormatAlertMessage(int alertLevel, string region, string district)
        {
            return $"Спецуведомление уровня {alertLevel}: регион {region}, затронут район {district}";
        }
    }

    public class NotificationDistributor
    {
        private readonly INotificationSender _notificationSender;

        public NotificationDistributor(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public void SendToMultipleRecipients(List<string> recipients, string messageTemplate)
        {
            foreach (var recipient in recipients)
            {
                string personalizedMessage = $"{messageTemplate} | Получатель: {recipient}";
                _notificationSender.Send(personalizedMessage);
            }
        }
    }

    public interface ITrafficPoliceNotificationService
    {
        string DistributeMessageToPosts(
            int postIdentifier,
            string postLocation,
            bool isUrgentSituation,
            DateTime eventTime,
            List<string> recipientPosts,
            string departmentName,
            string incidentDetails = null);

        void ProcessTrafficViolation(TrafficViolationType violationType);
        void ProcessTrafficViolation(int violationTypeCode);
        void BroadcastRegionalAlert(int alertSeverity, string regionName, string[] affectedDistricts);
    }

    public class TrafficPoliceNotificationService : ITrafficPoliceNotificationService
    {
        private readonly INotificationSender _notificationSender;
        private readonly IViolationHandlerFactory _violationFactory;
        private readonly IMessageValidator _messageValidator;
        private readonly IMessageFormatter _messageFormatter;
        private readonly NotificationDistributor _notificationDistributor;

        // Конструктор с dependency injection
        public TrafficPoliceNotificationService(
            INotificationSender notificationSender = null,
            IViolationHandlerFactory violationFactory = null,
            IMessageValidator messageValidator = null,
            IMessageFormatter messageFormatter = null)
        {
            _notificationSender = notificationSender ?? new PostNotificationSender();
            _violationFactory = violationFactory ?? new ViolationHandlerFactory();
            _messageValidator = messageValidator ?? new MessageValidator();
            _messageFormatter = messageFormatter ?? new MessageFormatter();
            _notificationDistributor = new NotificationDistributor(_notificationSender);
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
            if (!_messageValidator.ValidateRecipients(isUrgentSituation, recipientPosts))
            {
                return _messageValidator.GetErrorMessage(isUrgentSituation, recipientPosts);
            }

            string notificationContent = _messageFormatter.FormatStandardMessage(
                postIdentifier, postLocation, departmentName);

            _notificationDistributor.SendToMultipleRecipients(recipientPosts, notificationContent);

            return "Сообщение успешно доставлено всем постам";
        }

        public void ProcessTrafficViolation(TrafficViolationType violationType)
        {
            IViolationHandler handler = _violationFactory.CreateHandler(violationType);
            handler.ProcessViolation();
        }

        public void ProcessTrafficViolation(int violationTypeCode)
        {
            TrafficViolationType violationType = (TrafficViolationType)violationTypeCode;
            ProcessTrafficViolation(violationType);
        }

        public void BroadcastRegionalAlert(int alertSeverity, string regionName, string[] affectedDistricts)
        {
            foreach (var district in affectedDistricts)
            {
                string alertContent = _messageFormatter.FormatAlertMessage(alertSeverity, regionName, district);
                _notificationSender.Send(alertContent);
            }
        }
    }
}