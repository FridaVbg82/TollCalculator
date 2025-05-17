using FluentAssertions;
using TollCalculator.TollFree.Holidays;

namespace TollCalculatorTests;

public class HolidayProviderTests
{
    private IHolidayProvider? holidayProvider;
    private IEnumerable<DateTime>? dateTimeResult;
    private bool isDayBeforeHoliday;

    [Fact]
    public void GetHolidays_Returns_NationalDay()
    {
        GivenHolidayProvider();
        WhenGettingHolidaysFor(2025);
        ThenResultContains(new List<DateTime>
        {
            new DateTime(2025, 6, 6)
        });
    }

    [Fact]
    public void GetHolidays_Ordinary_Year_Returns_EasterDates()
    {
        GivenHolidayProvider();
        WhenGettingHolidaysFor(2025);
        ThenResultContains(new List<DateTime>
        {
            new DateTime(2025, 4, 18), // Långfredag
            new DateTime(2025, 4, 19), // Påskafton
            new DateTime(2025, 4, 20), // Påskdagen
            new DateTime(2025, 4, 21), // Annandag Påsk
            new DateTime(2025, 5, 29), // Kristihimmelfärd
            new DateTime(2025, 6, 8)   // Pingst
        });
    }

    [Fact]
    public void GetHolidays_Leap_Year_Returns_EasterDates()
    {
        GivenHolidayProvider();
        WhenGettingHolidaysFor(2028);
        ThenResultContains(new List<DateTime>
        {
            new DateTime(2028, 4, 14), // Långfredag
            new DateTime(2028, 4, 15), // Påskafton
            new DateTime(2028, 4, 16), // Påskdagen
            new DateTime(2028, 4, 17), // Annandag Påsk
            new DateTime(2028, 5, 25),  // Kristihimmelfärd
            new DateTime(2028, 6, 4)  // Pingst
        });
    }

    [Fact]
    public void IsDayBeforeHoliday_Returns_True()
    {
        GivenHolidayProvider();
        WhenGettingDayBeforHolidays(new DateTime(2025, 6, 5));
        ThenIsDayBeforeHolidayContains(true);
    }

    [Fact]
    public void IsDayBeforeHoliday_Returns_False()
    {
        GivenHolidayProvider();
        WhenGettingDayBeforHolidays(new DateTime(2025, 8, 5));
        ThenIsDayBeforeHolidayContains(false);
    }

    private void GivenHolidayProvider()
    {
        holidayProvider = new HolidayProvider();
    }

    private void WhenGettingHolidaysFor(int year)
    {
        dateTimeResult = holidayProvider!.GetHolidays(year);
    }

    private void WhenGettingDayBeforHolidays(DateTime dateToCheck)
    {
        isDayBeforeHoliday = holidayProvider!.IsDayBeforeHoliday(dateToCheck);
    }

    private void ThenResultContains(IEnumerable<DateTime> expectedResult)
    {
        dateTimeResult!.Should().ContainInOrder(expectedResult);
    }

    private void ThenIsDayBeforeHolidayContains(bool expectedResult)
    {
        isDayBeforeHoliday!.Should().Be(expectedResult);
    }
}