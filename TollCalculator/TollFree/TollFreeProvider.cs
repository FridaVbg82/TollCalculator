using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Common;
using TollCalculator.TollFree.Holidays;

namespace TollCalculator.TollFree
{
    public class TollFreeProvider(IHolidayProvider holidayProvider) : ITollFreeProvider
    {
        public bool IsTollFreeDate(DateTime dateTime) =>
            IsWeekend(dateTime) ||
            IsTollFreeMonth(dateTime) ||
            IsHolidayOrDayBefore(dateTime);

        public bool IsTollFreeVehicle(Vehicle vehicle) =>
            vehicle switch
            {
                Vehicle.Motorbike => true,
                Vehicle.Tractor => true,
                Vehicle.Emergency => true,
                Vehicle.Diplomat => true,
                Vehicle.Foreign => true,
                Vehicle.Military => true,
                _ => false
            };

        private bool IsWeekend(DateTime date) => 
            date.DayOfWeek == DayOfWeek.Saturday || 
            date.DayOfWeek == DayOfWeek.Sunday;

        private bool IsHolidayOrDayBefore(DateTime date) => 
            holidayProvider.GetHolidays(date.Year).Any(holiday => holiday.Date == date) ||
            holidayProvider.IsDayBeforeHoliday(date);

        private bool IsTollFreeMonth(DateTime date) =>
            date.Month == Constants.TOLLFREE_MONTH;
    }
}
