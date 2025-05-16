using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.TollFree
{
    public interface ITollFreeProvider
    {
        bool IsTollFreeDate(DateTime dateTime);
        bool IsTollFreeVehicle(Vehicle vehicle);
    }
}
