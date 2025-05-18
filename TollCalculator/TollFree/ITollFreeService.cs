namespace TollCalculator.TollFree;

public interface ITollFreeService
{
    bool IsTollFreeDate(DateTime dateTime);
    bool IsTollFreeVehicle(Vehicle vehicle);
}