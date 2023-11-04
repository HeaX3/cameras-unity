using System.Linq;
using Cameras.Interfaces;
using UnityEngine;

namespace Cameras.Aspects
{
    /// <summary>
    /// (Temporarily) focus on a specific target
    /// </summary>
    [RequireComponent(typeof(CameraZoom))]
    public class CameraFocus : MonoBehaviour, ICameraComponent<CameraFocus.Context>
    {
        Transform focusTarget;
        private CameraZoom zoom { get; set; }
        
        public new CameraController camera { get; private set; }
        
        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            zoom = controller.components.OfType<CameraZoom>().FirstOrDefault();
        }

        public void FocusOn(Transform target, int zoomLevel)
        {
            focusTarget = target;
            zoom.zoomLevel = zoomLevel;
        }

        public void FocusOn(Transform _target)
        {
            focusTarget = _target;
        }
        
        public class Context : CameraController.Context
        {
            
        }
    }
}