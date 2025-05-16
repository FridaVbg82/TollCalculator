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
            IsHoliday(dateTime);

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
            date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        private bool IsHoliday(DateTime date) => 
            holidayProvider.GetHolidays(date.Year).Any(holiday => holiday.Date == date);

        private bool IsTollFreeMonth(DateTime date) =>
            date.Month == Constants.TOLLFREE_MONTH;
    }
}
