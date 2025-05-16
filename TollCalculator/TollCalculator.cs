using TollCalculator.TollFree;
using TollFeeCalculator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TollCalculator;

public class TollCalculator(ITollFreeDecider tollFreeDecider)
{

    /**
 * Calculate the total toll fee for one day
 *
 * @param vehicle - the vehicle
 * @param dates   - date and time of all passes on one day
 * @return - the total toll fee for that day
 */
    const int DAILY_MAXIMUM_FEE = 60;
    const int HOUR_IN_MINUTES = 60;
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        if (tollFreeDecider.IsTollFreeDate(intervalStart) || tollFreeDecider.IsTollFreeVehicle(vehicle)) return 0;

        int totalFee = 0;
        foreach (DateTime timeOfPassage in dates)
        {
            int nextFee = GetTollFeeForTime(timeOfPassage);
            int tempFee = GetTollFeeForTime(intervalStart);

            long diffInMillies = timeOfPassage.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies/1000/60;

            if (minutes <= HOUR_IN_MINUTES)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > DAILY_MAXIMUM_FEE)
        {
            totalFee = DAILY_MAXIMUM_FEE;
        }

        return totalFee;
    }

    public int GetTollFeeForTime(DateTime timeOfPassage)
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
}