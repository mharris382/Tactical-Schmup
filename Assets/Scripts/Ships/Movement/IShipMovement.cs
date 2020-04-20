namespace Ships.Movement
{
    public interface IShipMovement
    {
        void HandleMovement(RtsShipController ship);
    }
    
    public interface IShipController : IShipMovement, IShipRotation
    {
    }
}