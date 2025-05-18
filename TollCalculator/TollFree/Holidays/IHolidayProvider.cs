namespace TollCalculator.TollFree.Holidays;

public interface IHolidayProvider
{
    /// <summary>
    /// Gets a list of holidays for the given year.
    /// </summary>
    /// <param name="year">The year for the holidays.</param>>
    /// <returns>A list of holidays.</returns>
    public IEnumerable<DateTime> GetHolidays(int year);

    public IEnumerable<DateTime> GetDaysBeforeHoliday(DateTime date);
}