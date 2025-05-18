using TollCalculator.Common;
using TollCalculator.Services.Holidays;

namespace TollCalculator.Services;

public class TollFreeService(IHolidayProvider holidayProvider) : ITollFreeService
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

    private bool IsHolidayOrDayBefore(DateTime date)
    {
        var holidays = holidayProvider.GetHolidays(date.Year);
        return holidayProvider.GetHolidays(date.Year).Any(holiday => holiday.Date == date.Date) ||
               holidayProvider.GetDaysBeforeHoliday(date).Any(dayBeforeHoliday => dayBeforeHoliday.Date == date.Date);
    }

    private bool IsTollFreeMonth(DateTime date) =>
        date.Month == Constants.TOLLFREE_MONTH;
}