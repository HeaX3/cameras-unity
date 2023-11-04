namespace Cameras.Interfaces
{
    public interface ICameraStateModifier : ICameraComponent
    {
        CameraState ModifyState(CameraState state, CameraState currentState);
    }
}