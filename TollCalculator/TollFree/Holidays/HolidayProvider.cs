using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollCalculator.TollFree.Holidays;
public class HolidayProvider : IHolidayProvider
{
    public IEnumerable<DateTime> GetHolidays(int year)
    {
        var holidays = new List<DateTime>();
        holidays.AddRange(GetFixedHolidays(year));
        holidays.AddRange(GetEasterHolidayDates(year));
        holidays.Add(GetMidsummersDayDate(year));
        holidays.Add(GetAllSaintsDayDate(year));
        return holidays.Distinct().OrderBy(date => date);
    }

    public bool IsDayBeforeHoliday(DateTime date)
    {
        var daysBeforeHoliday = new List<DateTime>();
        daysBeforeHoliday.AddRange(GetDaysBeforeFixedHolidays(date.Year));
        daysBeforeHoliday.AddRange(GetDaysBeforeDynamicHolidays(date.Year));
        return daysBeforeHoliday.Any(dayBeforeHoliday => dayBeforeHoliday.Date == date);
    }

    private IEnumerable<DateTime> GetEasterHolidayDates(int year)
    {
        DateTime easterSunday = GetEasterSunday(year);
        return new List<DateTime>
        {
            easterSunday.AddDays(-2), // Långfredag
            easterSunday.AddDays(-1), //Påskafton
            easterSunday, // Påskdagen
            easterSunday.AddDays(1),  // Annandag Påsk
            easterSunday.AddDays(39), // Kristihimmelfärd
            easterSunday.AddDays(49)  // Pingst
        };
    }

    private DateTime GetEasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }

    private DateTime GetMidsummersDayDate(int year)
    {
        DateTime midsummersDayDate = new DateTime(year, 6, 19);
        while (midsummersDayDate.DayOfWeek != DayOfWeek.Friday)
        {
            midsummersDayDate = midsummersDayDate.AddDays(1);
        }

        return midsummersDayDate.AddDays(1);
    }

    private DateTime GetAllSaintsDayDate(int year)
    {
        DateTime allSaintsDayDate = new DateTime(year, 10, 31);
        while (allSaintsDayDate.DayOfWeek != DayOfWeek.Saturday)
        {
            allSaintsDayDate = allSaintsDayDate.AddDays(1);
        }

        return allSaintsDayDate;
    }

    private IEnumerable<DateTime> GetFixedHolidays(int year)
    {
        return new List<DateTime>
        {
            new DateTime(year, 1, 1),
            new DateTime(year, 1, 6),
            new DateTime(year, 6, 6),
            new DateTime(year, 12, 24),
            new DateTime(year, 12, 25),
            new DateTime(year, 12, 26),
            new DateTime(year, 12, 31)
        };
    }

    private IEnumerable<DateTime> GetDaysBeforeFixedHolidays(int year)
    {
        return new List<DateTime>
        {
            new DateTime(year, 1, 5),
            new DateTime(year, 6, 5),
            new DateTime(year, 12, 23),
            new DateTime(year, 12, 30),
        };
    }

    private IEnumerable<DateTime> GetDaysBeforeDynamicHolidays(int year)
    {
        var easterSunday = GetEasterSunday(year);
        var midsummerDay = GetMidsummersDayDate(year);
        var allSaintsDay = GetAllSaintsDayDate(year);

        return new List<DateTime>
        {
            easterSunday.AddDays(-3),
            easterSunday.AddDays(38),
            easterSunday.AddDays(48),
            midsummerDay.AddDays(-1),
            allSaintsDay.AddDays(-1)
        };
    }
}