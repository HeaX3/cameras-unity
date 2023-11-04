using System;
using System.Collections.Generic;
using System.Linq;
using Cameras.Interfaces;
using UnityEngine;

namespace Cameras
{
    public class CameraController : MonoBehaviour
    {
        public delegate void StateEvent(CameraState previous, CameraState state);

        public event StateEvent stateUpdated = delegate { };

        [SerializeField]
        [Tooltip("This is the main camera. The target distance and Z angle will be applied to this transform.")]
        private Camera _camera;

        [SerializeField]
        [Tooltip(
            "This is the root transform of the entire camera stack. The position will be applied to this transform.")]
        private Transform _anchor;

        [SerializeField]
        [Tooltip(
            "This is where the camera will rotate around. Relative position offsets and the YX angles will be applied to this transform.")]
        private Transform _pivot;

        private Transform _cameraTransform;

        private CameraState _state;
        private CameraState _currentState;

        public ICameraComponent[] components { get; private set; } = Array.Empty<ICameraComponent>();
        private ICameraStateHandler[] stateHandlers { get; set; } = Array.Empty<ICameraStateHandler>();
        private ICameraStateModifier[] stateModifiers { get; set; } = Array.Empty<ICameraStateModifier>();
        public new Camera camera => _camera;

        private readonly Dictionary<string, List<object>> _inputOverrides = new();

        public CameraState state
        {
            get => _state;
            set => ApplyState(value);
        }

        public CameraState currentState
        {
            get => _currentState;
            private set => ApplyCurrentState(value);
        }

        public bool HasInputOverride(string action) =>
            _inputOverrides.TryGetValue(action, out var count) && count.Count > 0;

        private void OnEnable()
        {
            if (!_cameraTransform) Initialize();
        }

        private void Initialize()
        {
            _cameraTransform = _camera.transform;
            state = CalculateCurrentCameraState();
            currentState = state;
        }

        public void Initialize(params Context[] context)
        {
            if (!_cameraTransform) Initialize();
            var components = GetComponentsInChildren<ICameraComponent>();
            this.components = components;
            stateHandlers = components.OfType<ICameraStateHandler>().ToArray();
            stateModifiers = components.OfType<ICameraStateModifier>().ToArray();
            foreach (var component in components) component.Initialize(this, context);
        }

        public CameraState CalculateCurrentCameraState()
        {
            var localCameraPosition = _cameraTransform.localPosition;
            var localPivotRotation = _pivot.localEulerAngles;
            return new CameraState
            {
                position = _anchor.localPosition,
                offset = _pivot.localPosition,
                rotation = new Vector3(
                    localPivotRotation.x,
                    localPivotRotation.y,
                    _cameraTransform.localEulerAngles.z
                ),
                distance = -localCameraPosition.z,
                shift = new Vector2(localCameraPosition.x, localCameraPosition.y),
                size = _camera.orthographic ? _camera.orthographicSize * 2 : _camera.fieldOfView
            };
        }

        public void AddInputOverride(string action, object source)
        {
            if (!_inputOverrides.ContainsKey(action)) _inputOverrides[action] = new List<object>();
            _inputOverrides[action].Add(source);
        }

        public void RemoveInputOverride(string action, object source)
        {
            if (!_inputOverrides.TryGetValue(action, out var list)) return;
            list.Remove(source);
        }

        private void LateUpdate()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            var previous = state;
            var result = previous;
            foreach (var c in stateHandlers.Where(h => h.enabled))
            {
                result = c.UpdateState(previous, result);
            }

            state = result;
        }

        private void ApplyState(CameraState state)
        {
            _state = state;
            UpdateCurrentState(state);
        }

        private void UpdateCurrentState(CameraState state)
        {
            var result = state;
            foreach (var c in stateModifiers.Where(h => h.enabled))
            {
                result = c.ModifyState(state, result);
            }

            currentState = result;
        }

        private void ApplyCurrentState(CameraState state)
        {
            var previous = currentState;
            _currentState = state;
            _anchor.localPosition = state.position;
            _pivot.localPosition = state.offset;
            _pivot.localEulerAngles = new Vector3(state.rotation.x, state.rotation.y, 0);
            _cameraTransform.localPosition = new Vector3(state.shift.x, state.shift.y, -state.distance);
            if (_camera.orthographic) _camera.fieldOfView = state.size;
            else _camera.orthographicSize = state.size / 2;
            stateUpdated(previous, state);
        }

        public interface Context
        {
        }
    }
}