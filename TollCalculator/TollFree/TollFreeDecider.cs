using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollCalculator.TollFree
{
    public class TollFreeDecider : ITollFreeDecider
    {
        public bool IsTollFreeDate(DateTime dateTime)
        {
            if (IsWeekend(dateTime) ||
                IsTollFreeMonth(dateTime) ||
                IsHoliday(dateTime)) return true;


            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

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

        private bool IsHoliday(DateTime date)
        {
            return false;
        }

        private bool IsTollFreeMonth(DateTime date) =>
            date.Month == 7;
    }
}
