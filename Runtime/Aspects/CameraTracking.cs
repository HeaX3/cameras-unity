using System;
using Cameras.Interfaces;
using UnityEngine;

namespace Cameras.Aspects
{
    public class CameraTracking : MonoBehaviour, ICameraComponent<CameraTracking.Context>
    {
        public new CameraController camera { get; private set; }
        
        public Transform target { get; set; }

        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
        }

        private void LateUpdate()
        {
            throw new NotImplementedException();
        }

        public class Context : CameraController.Context
        {
        }
    }
}