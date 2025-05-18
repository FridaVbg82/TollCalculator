using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TollCalculator;
using TollCalculator.TollFree;
using TollCalculator.TollFree.Holidays;

namespace TollCalculatorTests;

public class TollFreeServiceTestsBase
{
    internal bool? result;
    internal ITollFreeService? tollFreeService;
    private Mock<IHolidayProvider> mockHolidayProvider = new Mock<IHolidayProvider>();

    public void Initialize()
    {
        mockHolidayProvider
            .Setup(provider => provider.GetHolidays(It.IsAny<int>()))
            .Returns(new List<DateTime>());
    }

    internal void GivenTollFreeService()
    {
        tollFreeService = new TollFreeService(mockHolidayProvider.Object);
    }

    public void GivenHolidays()
    {
        mockHolidayProvider
            .Setup(provider => provider.GetHolidays(It.IsAny<int>()))
            .Returns(new List<DateTime>
            {
                new DateTime(2024, 12, 24),
                new DateTime(2024, 12, 31),
                new DateTime(2025, 4, 21),
                new DateTime(2025, 5, 1),
                new DateTime(2025, 5, 29)
            });
    }

    public void GivenDaysBeforeHolidays()
    {
        mockHolidayProvider
            .Setup(provider => provider.GetDaysBeforeHoliday(It.IsAny<DateTime>()))
            .Returns(new List<DateTime>
            {
                new DateTime(2024, 12, 23),
                new DateTime(2024, 12, 30),
                new DateTime(2025, 4, 20),
                new DateTime(2025, 4, 30),
                new DateTime(2025, 5, 28)
            });
    }
}