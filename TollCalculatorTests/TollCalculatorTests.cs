using FluentAssertions;
using TollCalculator;

namespace TollCalculatorTests;

public class TollCalculatorTests : TollCalculatorTestsBase
{
    [Theory]
    [InlineData("2025-03-08 00:00:00", 0)]
    [InlineData("2025-03-08 03:00:00", 0)]
    [InlineData("2025-03-08 06:00:00", 8)]
    [InlineData("2025-03-08 06:30:00", 13)]
    [InlineData("2025-03-08 06:59:00", 13)]
    [InlineData("2025-03-08 07:00:00", 18)]
    [InlineData("2025-03-08 08:00:00", 13)]
    [InlineData("2025-03-08 08:30:00", 8)]
    [InlineData("2025-03-08 09:00:00", 8)]
    [InlineData("2025-03-08 11:22:00", 8)]
    [InlineData("2025-03-08 12:18:00", 8)]
    [InlineData("2025-03-08 15:00:00", 13)]
    [InlineData("2025-03-08 15:30:00", 18)]
    [InlineData("2025-03-08 16:34:00", 18)]
    [InlineData("2025-03-08 17:00:00", 13)]
    [InlineData("2025-03-08 17:59:00", 13)]
    [InlineData("2025-03-08 18:00:00", 8)]
    [InlineData("2025-03-08 18:30:00", 0)]
    public void GetTollFee_ReturnsExpectedFee_For_TimeOfDay(string timeOfDay, int expectedFee)
    {
        GivenTollCalculator();
        GivenTollFreeService(tollFreeDate: false, tollFreeVehicle: false);
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
        GivenTollFreeService(tollFreeDate: false, tollFreeVehicle: false);

        Action act = () => WhenCalculatingToll(Vehicle.Car, new List<DateTime>());

        TheResultShouldThrow(act);
    }
    [Fact]
    public void CalculateToll_Throws_Exception_On_Multiple_Dates()
    {
        GivenTollCalculator();
        GivenTollFreeService(tollFreeDate: false, tollFreeVehicle: false);

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
        GivenTollFreeService(tollFreeDate: true, tollFreeVehicle: false);
        WhenCalculatingToll(Vehicle.Car, new List<DateTime> { new DateTime(2024, 10, 7, 8, 0, 0) });
        ThenResultShouldBe(0);
    }
    [Fact]
    public void CalculateToll_Returns_0_For_TollFree_Vehicle()
    {
        GivenTollCalculator();
        GivenTollFreeService(tollFreeDate: false, tollFreeVehicle: true);
        WhenCalculatingToll(Vehicle.Motorbike, new List<DateTime> { new DateTime(2024, 10, 7, 8, 0, 0) });
        ThenResultShouldBe(0);
    }
    [Fact]
    public void CalculateToll_Returns_0_For_TollFree_VehicleAndDate()
    {
        GivenTollCalculator();
        GivenTollFreeService(tollFreeDate: true, tollFreeVehicle: true);
        WhenCalculatingToll(Vehicle.Motorbike, new List<DateTime> { new DateTime(2024, 10, 7, 8, 0, 0) });
        ThenResultShouldBe(0);
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
            new DateTime(2025,04,06,06,15,00),
            new DateTime(2025,04,06,06,30,00),
            new DateTime(2025,04,06,07,14,00)
        };
    }

    private static List<DateTime> PassagesForMultipleHours()
    {
        return new List<DateTime>
        {
            new DateTime(2025,04,06,08,00,00),
            new DateTime(2025,04,06,08,15,00),
            new DateTime(2025,04,06,08,40,00),
            new DateTime(2025,04,06,15,00,00),
            new DateTime(2025,04,06,15,15,00),
            new DateTime(2025,04,06,15,30,00)
        };
    }

    private static List<DateTime> PassagesForDailyMaximum()
    {
        return new List<DateTime>
        {
            new DateTime(2025,04,06,06,00,00),
            new DateTime(2025,04,06,07,00,00),
            new DateTime(2025,04,06,08,00,00),
            new DateTime(2025,04,06,09,30,00),
            new DateTime(2025,04,06,11,30,00),
            new DateTime(2025,04,06,12,30,00),
            new DateTime(2025,04,06,15,30,00),
            new DateTime(2025,04,06,16,30,00),
            new DateTime(2025,04,06,17,30,00)
        };
    }
}