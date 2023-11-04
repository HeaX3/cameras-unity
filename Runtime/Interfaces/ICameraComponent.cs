using System.Linq;
using JetBrains.Annotations;

namespace Cameras.Interfaces
{
    public interface ICameraComponent
    {
        CameraController camera { get; }
        void Initialize(CameraController controller, CameraController.Context[] context);
        bool enabled { get; }
    }
    
    public interface ICameraComponent<in T> : ICameraComponent where T : CameraController.Context
    {
        void ICameraComponent.Initialize(CameraController controller, CameraController.Context[] context)
        {
            Initialize(controller, context.OfType<T>().FirstOrDefault());
        }

        void Initialize(CameraController controller, [CanBeNull] T context);
    }
}