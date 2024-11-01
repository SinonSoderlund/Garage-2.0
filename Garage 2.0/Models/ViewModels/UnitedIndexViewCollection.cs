using Garage_2._0.Models.Entities;
using Garage_2._0.Utilities;
using System.Collections;

namespace Garage_2._0.Models.ViewModels
{
    public class UnitedIndexViewCollection
    {
        public IEnumerable<IndexViewModel> IndexViews { get; set; }
        public GarageStatisticsViewModel GarageStatistics { get; set; }
        public FeedbackBannerViewModel FeedbackBannerMessage { get; set; }

        public UnitedIndexViewCollection()
        {
            IndexViews = default!;
            GarageStatistics = default!;
            FeedbackBannerMessage = default!;
        }

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


        /// <summary>
        /// Automated constructor for the container model.
        /// </summary>
        /// <param name="vehicles">IEnumerable of garage vehicles</param>
        /// <param name="feedbackBannerMessage">Feedback message to display, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<Vehicle> vehicles, decimal Price, FeedbackBannerViewModel feedbackBannerMessage = null)
        {
            //set message
            FeedbackBannerMessage = feedbackBannerMessage;

            // Initialize Statistics calculation variables
            Dictionary<VehicleType, int> VehiclesByType = new Dictionary<VehicleType, int>();
            uint WheelsTotal = 0;
            long TicksTotal = 0;

            List<IndexViewModel> tIndexViews = new List<IndexViewModel>();

            //do the damn thing
            foreach (Vehicle v in vehicles)
            {
                VehiclesByType.TryGetValue(v.VehicleType, out int val);
                VehiclesByType[v.VehicleType] = val + 1;

                WheelsTotal += v.Wheels;

                IndexViewModel index = new IndexViewModel(v);
                tIndexViews.Add(index);

                TicksTotal += index.ParkedDuration.Ticks;
            }

            decimal cost = ViewUtilities.getPrice(TicksTotal, Price);

            List<string> VehiclesByTypeStrings = new();

            //convert dictionary pairs to displayable strings 
            foreach (KeyValuePair<VehicleType, int> de in VehiclesByType)
            {
                VehiclesByTypeStrings.Add($"{de.Key}: {de.Value}");
            }


            GarageStatistics = new GarageStatisticsViewModel(VehiclesByTypeStrings, WheelsTotal, cost);
            IndexViews = tIndexViews;
        }
    }
}
