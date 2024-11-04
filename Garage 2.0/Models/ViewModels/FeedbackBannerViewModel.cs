using Garage_2._0.Enums;
using Garage_2._0.Models.Entities;

namespace Garage_2._0.Models.ViewModels
{
    public class FeedbackBannerViewModel
    {
        public string Message { get; set; }
        public AlertType AlertType { get; set; }

        /// <summary>
        /// empty constructor
        /// </summary>
        public FeedbackBannerViewModel() 
        { 
            Message = string.Empty;
            AlertType = AlertType.warning;
        }

        /// <summary>
        /// Feedback message constructor
        /// </summary>
        /// <param name="message">banner message to be displayed</param>
        /// <param name="alertType">display type</param>
        /// <exception cref="ArgumentNullException">The Feedback banner message shouldnt be null!</exception>
        public FeedbackBannerViewModel(string message, AlertType alertType)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            AlertType = alertType;
        }

        public FeedbackBannerViewModel(FeedbackMessage feedbackMessage)
        {
            Message = feedbackMessage.Message;
            AlertType = feedbackMessage.AlertType;
        }
    }
}
