using Garage_2._0.Enums;
using Garage_2._0.Models.Entities;
using Garage_2._0.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;

namespace Garage_2._0.Models.ViewModels
{
    public class UnitedIndexViewCollection
    {
        public IEnumerable<IndexViewModel> IndexViews { get; set; }
        public GarageStatisticsViewModel GarageStatistics { get; set; }
        public FeedbackBannerViewModel FeedbackBannerMessage { get; set; }
        public UIVC_State UIVCModelState { get; set; }

        public UnitedIndexViewCollection()
        {
            IndexViews = default!;
            GarageStatistics = default!;
            FeedbackBannerMessage = default!;
            UIVCModelState = UIVC_State.empty;
        }

        /// <summary>
        /// Container model for all index view view models
        /// </summary>
        /// <param name="indexViews">IEnumerable of index view models</param>
        /// <param name="garageStatistics">garage statistics object</param>
        /// <param name="modelState">State of model, defaults to partial in this mode, set to full if the model contains all vehicles</param>
        /// <param name="feedbackBannerMessage">feedback banner message, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<IndexViewModel> indexViews, GarageStatisticsViewModel garageStatistics,UIVC_State modelState = UIVC_State.partial, FeedbackBannerViewModel feedbackBannerMessage = null)
        {
            if (NullBailer(indexViews.Count()))
                return;
            IndexViews = indexViews;
            GarageStatistics = garageStatistics;
            UIVCModelState = modelState;
            FeedbackBannerMessage = feedbackBannerMessage;
        }


        /// <summary>
        /// Automated constructor for the container model.
        /// </summary>
        /// <param name="vehicles">IEnumerable of garage vehicles</param>
        /// <param name="modelState">State of model, defaults to full in htis mode, set to partial if the model only contains subset of vehicles</param>
        /// <param name="feedbackBannerMessage">Feedback message to display, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<Vehicle> vehicles, decimal Price, UIVC_State modelState = UIVC_State.full, FeedbackBannerViewModel feedbackBannerMessage = null!)
        {
            if(NullBailer(vehicles.Count()))
                return;


            //set message
            FeedbackBannerMessage = feedbackBannerMessage;

            UIVCModelState = modelState;

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
            foreach (KeyValuePair<VehicleType, int> kp in VehiclesByType)
            {
                VehiclesByTypeStrings.Add($"{kp.Key}: {kp.Value}");
            }


            GarageStatistics = new GarageStatisticsViewModel(VehiclesByTypeStrings, WheelsTotal, cost);
            IndexViews = tIndexViews;
        }

        /// <summary>
        /// Automated constructor for the container model.
        /// </summary>
        /// <param name="indexViews">IEnumerable of index views</param>
        /// <param name="modelState">State of model, defaults to partial in this mode, set to full if the model contains all vehicles</param>
        /// <param name="feedbackBannerMessage">Feedback message to display, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<IndexViewModel> indexViews, decimal Price, UIVC_State modelState = UIVC_State.partial, FeedbackBannerViewModel feedbackBannerMessage = null!)
        {
            if (NullBailer(indexViews.Count()))
                return;

            //set message
            FeedbackBannerMessage = feedbackBannerMessage;

            UIVCModelState = modelState;

            // Initialize Statistics calculation variables
            Dictionary<VehicleType, int> VehiclesByType = new Dictionary<VehicleType, int>();
            long TicksTotal = 0;

            List<IndexViewModel> tIndexViews = new List<IndexViewModel>();

            //do the damn thing
            foreach (IndexViewModel iv in indexViews)
            {
                VehiclesByType.TryGetValue(iv.VehicleType, out int val);
                VehiclesByType[iv.VehicleType] = val + 1;


                tIndexViews.Add(iv);

                TicksTotal += iv.ParkedDuration.Ticks;
            }

            decimal cost = ViewUtilities.getPrice(TicksTotal, Price);

            List<string> VehiclesByTypeStrings = new();

            //convert dictionary pairs to displayable strings 
            foreach (KeyValuePair<VehicleType, int> kp in VehiclesByType)
            {
                VehiclesByTypeStrings.Add($"{kp.Key}: {kp.Value}");
            }


            GarageStatistics = new GarageStatisticsViewModel(VehiclesByTypeStrings, uint.MinValue, cost);
            IndexViews = tIndexViews;
        }
        public bool NullBailer(int count)
        {
            if (count == 0)
            {
                IndexViews = default!;
                UIVCModelState = UIVC_State.empty;
                GarageStatistics = default!;
                FeedbackBannerMessage = default!;
                return true;
            }
            return false;
        }
    }

}
