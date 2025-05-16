using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator;

namespace TollCalculator.TollFree
{
    public interface ITollFreeDecider
    {
        bool IsTollFreeDate(DateTime dateTime);
        bool IsTollFreeVehicle(Vehicle vehicle);
    }
}
