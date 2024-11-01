namespace Garage_2._0.Models.ViewModels
{
    public class UnitedIndexViewCollection
    {
        public IEnumerable<IndexViewModel> IndexViews { get; set; }
        public GarageStatisticsViewModel GarageStatistics { get; set; }
        public FeedbackBannerViewModel FeedbackBannerMessage { get; set; }

        /// <summary>
        /// Container model for all index view view models
        /// </summary>
        /// <param name="indexViews">IEnumerable of index view models</param>
        /// <param name="garageStatistics">garage statistics object</param>
        /// <param name="feedbackBannerMessage">feedback banner message, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<IndexViewModel> indexViews, GarageStatisticsViewModel garageStatistics, FeedbackBannerViewModel feedbackBannerMessage = null)
        {
            IndexViews = indexViews;
            GarageStatistics = garageStatistics;
            FeedbackBannerMessage = feedbackBannerMessage;
        }
    }
}
