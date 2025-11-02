/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTP
{
    internal class GIBDDPostNotificationSystem
    {
    }
}
*/
namespace OTP
{
    public class GIBDDPostNotificationSystem
    {

        public static void NotifyPosts(string message)
        {
            Console.WriteLine("Сообщение для всех постов ГИБДД: {0}", message);
        }

        public string SendMessageToAllPosts(
        int postId,
        string location,
        bool isEmergency,
        DateTime timestamp,
        List<string> recipientsList,
        string senderName,
        string additionalInfo = null)
        {
            if (!isEmergency && recipientsList.Count > 0)
                return $"Сообщение успешно отправлено.";
            else if (isEmergency && recipientsList.Count <= 0)
                return $"Отсутствуют получатели для экстренного уведомления!";
            foreach (var recipient in recipientsList)
            {
                NotifyPosts($"Сообщение от поста №{postId}, местоположение:{ location}. Отправитель: { senderName}");
            }
            return "Сообщение успешно доставлено";
        }

        public void HandleTrafficViolation(int violationCode)
        {
            switch (violationCode)
            {
                case 1:
                    Console.WriteLine("Превышение скорости");
                    break;
                case 2:
                    Console.WriteLine("Проезд на красный свет");
                    break;
                default:
                    Console.WriteLine("Неизвестное нарушение");
                    break;
            }
        }

        public void SendSpecialAlert(int alertLevel, string region, string[]
       affectedAreas)
        {
            for (int i = 0; i < affectedAreas.Length; i++)
            {
                NotifyPosts($"Спецуведомление уровня {alertLevel}: регион { region}, затронута область { affectedAreas[i]}");
            }
        }
    }
}