using Garage_2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data.Repositories
{
    public class FeedbackMessageRepository : IFeedbackMessageRepository
    {
        private readonly Garage_2_0Context _context;

        public FeedbackMessageRepository(Garage_2_0Context context)
        {
            _context = context;
        }

        public async Task<FeedbackMessage> GetMessage()
        {

            FeedbackMessage feedback = await _context.FeedbackMessage.FirstOrDefaultAsync();
            if (feedback == null || !feedback.HasValue())
                return null;
            var output = new FeedbackMessage(feedback);
            feedback.Erase();
            _context.FeedbackMessage.Update(feedback);
            await _context.SaveChangesAsync();
            return output;
        }


        public async Task<bool> SetMessage(FeedbackMessage message)
        {
            if(message == null) throw new ArgumentNullException("message should not be null");
            FeedbackMessage feedbackMessage = await _context.FeedbackMessage.FirstOrDefaultAsync();
            if (feedbackMessage == null)
                _context.FeedbackMessage.Add(message);
            else
            {
                feedbackMessage.UpdateMessage(message);
                _context.FeedbackMessage.Update(feedbackMessage);
            }
            await _context.SaveChangesAsync();
                return true;
        }
    }
}
