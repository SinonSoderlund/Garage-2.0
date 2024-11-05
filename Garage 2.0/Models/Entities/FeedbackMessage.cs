using Garage_2._0.Enums;
using Garage_2._0.Models.ViewModels;

namespace Garage_2._0.Models.Entities
{
    public class FeedbackMessage
    {
        public int Id { get; set; } 
        public string Message { get; set; }
        public AlertType AlertType { get; set; }

        public FeedbackMessage() { }

        public FeedbackMessage(string message, AlertType alertType)
        {
            Message = message;
            AlertType = alertType;
        }

        public FeedbackMessage(FeedbackBannerViewModel bannerViewModel)
        {
            Message = bannerViewModel.Message;
            AlertType = bannerViewModel.AlertType;
        }
        public FeedbackMessage(FeedbackMessage feedback)
        {
            Message = feedback.Message;
            AlertType = feedback.AlertType;
        }

        public void UpdateMessage(FeedbackMessage message)
        {
            Message = message.Message;
            AlertType = message.AlertType;
        }
        public void Erase()
        { 
            Message = string.Empty;
            AlertType = default;
        }
        public bool HasValue()
        {
            return Message != string.Empty;
        }
    }
}
