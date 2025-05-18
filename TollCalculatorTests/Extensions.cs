namespace TollCalculatorTests;

public static class Extensions
{
    public static DateTime ToDateTime(this string date)
    {
        return DateTime.Parse(date);
    }
}