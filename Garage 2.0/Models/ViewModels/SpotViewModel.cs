using Garage_2._0.Models.Entities;

namespace Garage_2._0.Models.ViewModels
{
    public class SpotViewModel
    {
        public IEnumerable<Spot>? SpotCollection { get; set; }

        public int AvailableSpots
        {
            get => SpotCollection!.Count();
        }

        /// <summary>
        /// empty spot constructor
        /// </summary>
        public SpotViewModel() 
        { }

        /// <summary>
        /// Spot constructor
        /// </summary>
        /// <param name="spotCollection">the number of available spots</param>
        public SpotViewModel(IEnumerable<Spot>? spotCollection) 
        {
            SpotCollection = spotCollection;
        }

    }
}
