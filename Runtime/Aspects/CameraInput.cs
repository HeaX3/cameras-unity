using Cameras.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras.Aspects
{
    public class CameraInput : MonoBehaviour, ICameraComponent<CameraInput.Context>
    {
        [SerializeField] public InputActionAsset inputMaster;
        
        public new CameraController camera { get; private set; }

        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
        }

        public class Context : CameraController.Context
        {
            
        }
    }
}