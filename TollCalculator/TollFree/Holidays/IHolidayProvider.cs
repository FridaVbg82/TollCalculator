using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.TollFree.Holidays
{
    internal interface IHolidayProvider
    {
        /// <summary>
        /// Gets a list of holidays for the given year.
        /// </summary>
        /// <param name="year">The year for the holidays.</param>>
        /// <returns>A list of holidays.</returns>
        IEnumerable<DateTime> GetHolidays(int year);
    }
}
