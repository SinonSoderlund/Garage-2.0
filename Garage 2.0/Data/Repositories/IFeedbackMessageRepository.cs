using Garage_2._0.Models.Entities;
using Garage_2._0.Models.ViewModels;

namespace Garage_2._0.Data.Repositories
{
    public interface IFeedbackMessageRepository
    {
        /// <summary>
        /// Gets the currently stored feedback message, wipes data on retrieval
        /// </summary>
        /// <returns></returns>
        public Task<FeedbackMessage> GetMessage();

        /// <summary>
        /// Sets the currently stored feedback message
        /// </summary>
        /// <param name="message">the message to be set</param>
        /// <returns></returns>
        public Task<bool> SetMessage(FeedbackMessage message);

    }
}
