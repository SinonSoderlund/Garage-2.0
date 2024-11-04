using Garage_2._0.Models.Entities;

namespace Garage_2._0.Data.Repositories
{
    public class FeedbackMessageRepository : IFeedbackMessageRepository
    {
        private readonly Garage_2_0Context _context;

        public FeedbackMessageRepository(Garage_2_0Context context)
        {
            _context = context;
        }

        public Task<FeedbackMessage> GetMessage()
        {

            FeedbackMessage output =  _context.FeedbackMessage!;
            _context.FeedbackMessage = null!;
            return Task.FromResult(output);
        }


        public Task<bool> SetMessage(FeedbackMessage message)
        {
            if(message == null) throw new ArgumentNullException("message");
            _context.FeedbackMessage = message;

                return Task.FromResult(true);
        }
    }
}
