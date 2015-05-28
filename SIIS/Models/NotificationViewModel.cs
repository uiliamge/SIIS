namespace SIIS.Models
{
    public class NotificationViewModel
    {
        public int Count { get; set; }
        public string NotificationType { get; set; }
        //Pode ser: danger, info, inverse, success ou warning.
        public string BadgeClass { get; set; }
    }
}