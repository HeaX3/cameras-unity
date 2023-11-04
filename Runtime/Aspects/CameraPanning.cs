using System;
using System.Linq;
using Cameras.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras.Aspects
{
    /// <summary>
    /// Add manual panning capabilities
    /// </summary>
    [RequireComponent(typeof(CameraInput))]
    public class CameraPanning : MonoBehaviour, ICameraComponent<CameraPanning.Context>, ICameraStateHandler
    {
        public new CameraController camera { get; private set; }

        private CameraInput input;
        InputAction pointAction;
        InputAction interactAction;

        private bool panning = false;

        private Vector3 _startPosition;
        private Vector2 _startScreenPosition;
        private Vector3 _startPlanarPosition;

        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            input = controller.components.OfType<CameraInput>().FirstOrDefault();
            if (!input)
            {
                throw new Exception("Camera input missing");
            }

            pointAction = input.inputMaster.FindAction("Point");
            pointAction.Enable();

            interactAction = input.inputMaster.FindAction(InputActionId.Pan);
            interactAction.Enable();
        }

        public CameraState UpdateState(CameraState state, CameraState modified)
        {
            if (camera.HasInputOverride(InputActionId.Pan) || !interactAction.inProgress)
            {
                panning = false;
                return modified;
            }

            var input = pointAction.ReadValue<Vector2>();
            var planarPosition = GetPlanarPosition(input) - state.position;
            if (!panning)
            {
                panning = true;
                _startPosition = state.position;
                _startScreenPosition = input;
                _startPlanarPosition = planarPosition;
            }

            var delta = planarPosition - _startPlanarPosition;
            modified.position = Vector3.Lerp(modified.position, _startPosition - delta, Time.deltaTime * 10f);
            return modified;
        }

        private Vector3 GetPlanarPosition(Vector2 screenPoint)
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = camera.camera.ScreenPointToRay(screenPoint);
            return ray.GetPoint(plane.Raycast(ray, out var distance) ? distance : 0);
        }

        public class Context : CameraController.Context
        {
        }
    }
}