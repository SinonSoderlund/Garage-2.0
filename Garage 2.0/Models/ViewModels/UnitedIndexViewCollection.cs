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
        public SpotViewModel SpotViewModel { get; set; }
        public UIVC_State UIVCModelState { get; set; }

        public UnitedIndexViewCollection()
        {
            IndexViews = default!;
            GarageStatistics = default!;
            FeedbackBannerMessage = default!;
            SpotViewModel = default!;
            UIVCModelState = UIVC_State.empty;
        }






        /// <summary>
        /// Automated constructor for the container model.
        /// </summary>
        /// <param name="vehicles">IEnumerable of garage vehicles</param>
        /// <param name="Price">hourly parking price sek</param>
        /// <param name="spots">Spots collection</param>
        /// <param name="modelState">State of model, defaults to full in this mode, set to partial if the model only contains subset of vehicles</param>
        /// <param name="feedbackMessage">Feedback message to display, leave empty if there is no message</param>
        public UnitedIndexViewCollection(IEnumerable<Vehicle> vehicles, decimal Price, IEnumerable<Spot>? spots, UIVC_State modelState = UIVC_State.full, FeedbackMessage feedbackMessage = null!)
        {
            //set message
            if(feedbackMessage != null)
            FeedbackBannerMessage = new FeedbackBannerViewModel(feedbackMessage);

            SpotViewModel = new SpotViewModel(spots);

            if (NullBailer(vehicles.Count()))
                return;




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
        /// Checks if there are any items passed in into the constructor, if not set related items to null or default
        /// </summary>
        /// <param name="count">the count for the passed in collection</param>
        /// <returns>true if collection is empty, you should then exit contrsutor, returns true if collection is populated.</returns>
        public bool NullBailer(int count)
        {
            if (count == 0)
            {
                IndexViews = default!;
                UIVCModelState = UIVC_State.empty;
                GarageStatistics = default!;
                return true;
            }
            return false;
        }
    }

}
