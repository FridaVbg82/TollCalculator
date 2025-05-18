using TollCalculator.Common;
using TollCalculator.Services;

namespace TollCalculator;

public class TollCalculator(ITollFreeService tollFreeService)
{
    /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total toll fee for that day
 */
    public int GetTollFee(Vehicle vehicle, IEnumerable<DateTime> dates)
    {
        ValidateDateRange(dates);

        // Make sure the dates are in order
        dates = dates.OrderBy(date => date);
        if (tollFreeService.IsTollFreeDate(dates.First()) || tollFreeService.IsTollFreeVehicle(vehicle)) 
            return 0;

        var fee = 0;
        foreach (var hourlyPassages in SplitDateTimeOnHour(dates))
        {
            fee += CalculateFeeForHour(hourlyPassages);
        }

        return Math.Min(fee, Constants.DAILY_MAXIMUM_FEE);
    }

    private void ValidateDateRange(IEnumerable<DateTime> dates)
    {
        if (!dates.Any())
            throw new ArgumentException("Expected at least one date");

        if (IsMultipleDays(dates)) 
            throw new ArgumentException("Dates span multiple days");
    }

    private int CalculateFeeForHour(List<DateTime> hourlyPassages)
    {
        List<int> fee = new List<int>();
        foreach (var time in hourlyPassages)
        {
            fee.Add(GetTollFeeForTime(time));
        } 
        return fee.Max();
    }

    private int GetTollFeeForTime(DateTime timeOfPassage)
    {
        return timeOfPassage.Hour switch
        {
            6 => timeOfPassage.Minute < 30 ? 8 : 13,
            7 => 18,
            8 => timeOfPassage.Minute < 30 ? 13 : 8,
            9 or 10 or 11 or 12 or 13 or 14 => 8,
            15 => timeOfPassage.Minute < 30 ? 13 : 18,
            16 => 18,
            17 => 13,
            18 => timeOfPassage.Minute < 30 ? 8 : 0,
            _ => 0,
        };
    }

    private bool IsMultipleDays(IEnumerable<DateTime> dates)
    {
        return dates.DistinctBy(date => date.Date).Count() != 1;
    }

    private List<List<DateTime>> SplitDateTimeOnHour(IEnumerable<DateTime> dateTimes)
    {
        List<List<DateTime>> hourlyPassageList = new List<List<DateTime>>();
        List<DateTime> currentHourList = new List<DateTime>();

        foreach (var date in dateTimes)
        {
            if (currentHourList.Count > 0 && (date - currentHourList.First()) > TimeSpan.FromHours(1))
            {
                hourlyPassageList.Add(currentHourList);
                currentHourList = new List<DateTime>();
            }

            currentHourList.Add(date);
        }

        if (currentHourList.Count > 0)
        {
            hourlyPassageList.Add(currentHourList);
        }

        return hourlyPassageList;
    }
}