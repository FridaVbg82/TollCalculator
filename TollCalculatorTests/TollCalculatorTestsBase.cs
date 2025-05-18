using Moq;
using TollCalculator;
using TollCalculator.TollFree;

namespace TollCalculatorTests;

public class TollCalculatorTestsBase
{
    internal int? result;
    internal TollCalculator.TollCalculator? tollCalculator;
    private Mock<ITollFreeService> mockTollFreeService = new Mock<ITollFreeService>();

    public void Initialize()
    {
        mockTollFreeService
            .Setup(provider => provider.IsTollFreeDate(It.IsAny<DateTime>()))
            .Returns(false);
    }

    internal void GivenTollCalculator()
    {
        tollCalculator = new TollCalculator.TollCalculator(mockTollFreeService.Object);
    }

    public void GivenTollFreeService(bool tollFreeDate, bool tollFreeVehicle)
    {
        mockTollFreeService.Setup(service => service.IsTollFreeVehicle(It.IsAny<Vehicle>())).Returns(tollFreeVehicle);
        mockTollFreeService.Setup(service => service.IsTollFreeDate(It.IsAny<DateTime>())).Returns(tollFreeDate);
    }
}