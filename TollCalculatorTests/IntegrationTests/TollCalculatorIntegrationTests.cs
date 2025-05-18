using FluentAssertions;
using TollCalculator;
using TollCalculator.Services;

namespace TollCalculatorTests;

public class TollCalculatorIntegrationTests
{
    private TollCalculator.TollCalculator? tollCalculator;
    private int? result;

    [Theory]
    [InlineData("2025-03-07 00:00:00", 0)]
    [InlineData("2025-03-07 03:00:00", 0)]
    [InlineData("2025-03-07 06:00:00", 8)]
    [InlineData("2025-03-07 06:30:00", 13)]
    [InlineData("2025-03-07 06:59:00", 13)]
    [InlineData("2025-03-07 07:00:00", 18)]
    [InlineData("2025-03-07 08:00:00", 13)]
    [InlineData("2025-03-07 08:30:00", 8)]
    [InlineData("2025-03-07 09:00:00", 8)]
    [InlineData("2025-03-07 11:22:00", 8)]
    [InlineData("2025-03-07 12:18:00", 8)]
    [InlineData("2025-03-07 15:00:00", 13)]
    [InlineData("2025-03-07 15:30:00", 18)]
    [InlineData("2025-03-07 16:34:00", 18)]
    [InlineData("2025-03-07 17:00:00", 13)]
    [InlineData("2025-03-07 17:59:00", 13)]
    [InlineData("2025-03-07 18:00:00", 8)]
    [InlineData("2025-03-07 18:30:00", 0)]
    public void GetTollFee_ReturnsExpectedFee_For_TimeOfDay(string timeOfDay, int expectedFee)
    {
        GivenTollCalculator();
        WhenCalculatingToll(Vehicle.Car, new List<DateTime> {timeOfDay.ToDateTime()} );
        ThenResultShouldBe(expectedFee);
    }

    [Fact]
    public void Only_MaximumFee_Is_Counted_For_Several_Passes_Within_An_Hour()
    {
        GivenTollCalculator();
        WhenCalculatingToll(
            vehicle: Vehicle.Car,
            timeOfDayForPassages: PassagesForOneHour()
        );
        ThenResultShouldBe(18);
    }

    [Fact]
    public void Getting_TollFee_Across_RangeOfHours_Is_Calculated_Correctly()
    {
        GivenTollCalculator();
        WhenCalculatingToll(
            vehicle: Vehicle.Car,
            timeOfDayForPassages: PassagesForMultipleHours()
        );

        ThenResultShouldBe(31);
    }

    [Fact]
    public void Toll_DoesNot_Exceed_DailyMaximum()
    {
        GivenTollCalculator();
        WhenCalculatingToll(
            vehicle: Vehicle.Car,
            timeOfDayForPassages: PassagesForDailyMaximum()
        );
        ThenResultShouldBe(60);
    }

    [Fact]
    public void CalculateToll_Throws_Exception_On_Missing_Dates()
    {
        GivenTollCalculator();
        
        Action act = () => WhenCalculatingToll(Vehicle.Car, new List<DateTime>());

        TheResultShouldThrow(act);
    }
    [Fact]
    public void CalculateToll_Throws_Exception_On_Multiple_Dates()
    {
        GivenTollCalculator();

        Action act = () => WhenCalculatingToll(Vehicle.Car, new List<DateTime>
        {
            new DateTime(2024,02,02),
            new DateTime(2025,02,03)
        });
        
        TheResultShouldThrow(act);
    }

    [Fact]
    public void CalculateToll_Returns_0_For_TollFree_Date()
    {
        GivenTollCalculator();
        WhenCalculatingToll(Vehicle.Car, new List<DateTime> { new DateTime(2025, 01, 01, 8, 0, 0) });
        ThenResultShouldBe(0);
    }
    [Fact]
    public void CalculateToll_Returns_0_For_TollFree_Vehicle()
    {
        GivenTollCalculator();
        WhenCalculatingToll(Vehicle.Motorbike, new List<DateTime> { new DateTime(2024, 10, 7, 8, 0, 0) });
        ThenResultShouldBe(0);
    }
    [Fact]
    public void CalculateToll_Returns_0_For_TollFree_VehicleAndDate()
    {
        GivenTollCalculator();
        WhenCalculatingToll(Vehicle.Motorbike, new List<DateTime> { new DateTime(2025, 01, 01, 8, 0, 0) });
        ThenResultShouldBe(0);
    }

    private void GivenTollCalculator()
    {
        var holidayProvider = new TollCalculator.Services.Holidays.HolidayProvider();
        var tollFreeService = new TollFreeService(holidayProvider);
        tollCalculator = new TollCalculator.TollCalculator(tollFreeService);
    }

    private void ThenResultShouldBe(int i)
    {
        result.Should().Be(i);
    }

    private void TheResultShouldThrow(Action act)
    {
        Assert.Throws<ArgumentException>(act);
    }

    private void WhenCalculatingToll(Vehicle vehicle, IEnumerable<DateTime> timeOfDayForPassages)
    {
        var timeOfPassages = new List<DateTime>();
        foreach (var timeOfPassage in timeOfDayForPassages)
        {
            timeOfPassages.Add(timeOfPassage);
        }

        result = tollCalculator?.GetTollFee(vehicle, timeOfPassages);
    }

    private static List<DateTime> PassagesForOneHour()
    {
        return new List<DateTime>
        {
            new DateTime(2025,04,07,06,15,00),
            new DateTime(2025,04,07,06,30,00),
            new DateTime(2025,04,07,07,14,00)
        };
    }

    private static List<DateTime> PassagesForMultipleHours()
    {
        return new List<DateTime>
        {
            new DateTime(2025,04,07,08,00,00),
            new DateTime(2025,04,07,08,15,00),
            new DateTime(2025,04,07,08,40,00),
            new DateTime(2025,04,07,15,00,00),
            new DateTime(2025,04,07,15,15,00),
            new DateTime(2025,04,07,15,30,00)
        };
    }

    private static List<DateTime> PassagesForDailyMaximum()
    {
        return new List<DateTime>
        {
            new DateTime(2025,04,07,06,00,00),
            new DateTime(2025,04,07,07,00,00),
            new DateTime(2025,04,07,08,00,00),
            new DateTime(2025,04,07,09,30,00),
            new DateTime(2025,04,07,11,30,00),
            new DateTime(2025,04,07,12,30,00),
            new DateTime(2025,04,07,15,30,00),
            new DateTime(2025,04,07,16,30,00),
            new DateTime(2025,04,07,17,30,00)
        };
    }
}