using FluentAssertions;
using TollCalculator;

namespace TollCalculatorTests.ServicesTests;

public class TollFreeServiceTests : TollFreeServiceTestsBase
{
    [Fact]
    public void IsTollFreeDate_Returns_True_For_Weekend()
    {
        GivenTollFreeService();
        WhenCheckingIfTollFreeDate(new DateTime(2023, 10, 7));
        ThenResultShouldBe(true);
    }
    [Fact]
    public void IsTollFreeDate_Returns_True_For_TollFreeMonth()
    {
        GivenTollFreeService();
        WhenCheckingIfTollFreeDate(new DateTime(2023, 7, 15));
        ThenResultShouldBe(true);
    }
    [Fact]
    public void IsTollFreeDate_Returns_False_For_OrdinaryDay()
    {
        GivenTollFreeService();
        WhenCheckingIfTollFreeDate(new DateTime(2025, 5, 5));
        ThenResultShouldBe(false);
    }

    [Theory]
    [InlineData("2024-12-24 09:00:00")]
    [InlineData("2024-12-31 09:00:00")]
    [InlineData("2025-04-21 09:00:00")]
    [InlineData("2025-05-01 09:00:00")]
    [InlineData("2025-05-29 09:00:00")]
    public void IsTollFreeDate_Returns_True_For_Holiday(string date)
    {
        GivenTollFreeService();
        GivenHolidays();
        WhenCheckingIfTollFreeDate(date);
        ThenResultShouldBe(true);
    }

    [Theory]
    [InlineData("2024-12-23 09:00:00")]
    [InlineData("2024-12-30 09:00:00")]
    [InlineData("2025-04-20 09:00:00")]
    [InlineData("2025-04-30 09:00:00")]
    [InlineData("2025-05-28 09:00:00")]
    public void IsTollFreeDate_Returns_True_For_DayBeforeHoliday(string date)
    {
        GivenTollFreeService();
        GivenDaysBeforeHolidays();
        WhenCheckingIfTollFreeDate(date);
        ThenResultShouldBe(true);
    }

    [Theory]
    [InlineData(Vehicle.Motorbike)]
    [InlineData(Vehicle.Tractor)]
    [InlineData(Vehicle.Emergency)]
    [InlineData(Vehicle.Diplomat)]
    [InlineData(Vehicle.Foreign)]
    [InlineData(Vehicle.Military)]
    public void IsTollFreeVehicle_Returns_True_For_TollFreeVehicle(Vehicle vehicle)
    {
        GivenTollFreeService();
        WhenCheckingIfTollFreeVehicle(vehicle);
        ThenResultShouldBe(true);
    }

    [Fact]
    public void IsTollFreeVehicle_Returns_False_For_VehicleType_Car()
    {
        GivenTollFreeService();
        WhenCheckingIfTollFreeVehicle(Vehicle.Car);
        ThenResultShouldBe(false);
    }

    private void WhenCheckingIfTollFreeDate(DateTime date)
    {
        result = tollFreeService?.IsTollFreeDate(date);
    }

    private void WhenCheckingIfTollFreeDate(string date)
    {
        var dateTime = date.ToDateTime();
        result = tollFreeService?.IsTollFreeDate(dateTime);
    }
    private void WhenCheckingIfTollFreeVehicle(Vehicle vehicle)
    {
        result = tollFreeService?.IsTollFreeVehicle(vehicle);
        
    }
    private void ThenResultShouldBe(bool expectedResult)
    {
        result.Should().Be(expectedResult);
    }
}