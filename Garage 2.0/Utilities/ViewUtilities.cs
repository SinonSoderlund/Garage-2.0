using System.Globalization;

namespace Garage_2._0.Utilities
{
    public static class ViewUtilities
    {
        /// <summary>
        /// Calculates the cost of parking
        /// </summary>
        /// <param name="time">time in timespand ticks for parking duration</param>
        /// <param name="price">hourly price of parking</param>
        /// <returns>cost of parking</returns>
        public static decimal getPrice(long time, decimal price)
        {
            decimal dTime = Convert.ToDecimal(time);
            decimal dDevide = Convert.ToDecimal(TimeSpan.TicksPerHour);
            return (dTime / dDevide) * price;
        }
        /// <summary>
        /// Turns price into text based representation in SEK
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string PriceToString(decimal price)
        {
            return price.ToString("C", CultureInfo.GetCultureInfo("sv-SE"));
        }
    }
}
