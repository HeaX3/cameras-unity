namespace Cameras.Interfaces
{
    public interface ICameraStateHandler : ICameraComponent
    {
        CameraState UpdateState(CameraState state, CameraState modified);
    }
}