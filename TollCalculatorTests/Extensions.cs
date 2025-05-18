using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculatorTests;

public static class Extensions
{
    public static DateTime ToDateTime(this string date)
    {
        return DateTime.Parse(date);
    }
}