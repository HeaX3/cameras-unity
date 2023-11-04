using System;
using System.Linq;
using Cameras.Interfaces;
using Essentials;
using MobileInputs;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Cameras.Aspects
{
    /// <summary>
    /// Add manual zoom capabilities
    /// </summary>
    [RequireComponent(typeof(CameraInput))]
    public class CameraZoom : MonoBehaviour, ICameraComponent<CameraZoom.Context>, ICameraStateHandler
    {
        [SerializeField] private MinMaxRange _zoomLimits = new(7, 70);
        [SerializeField] private float _zoomLerpSpeed = 10;
        [SerializeField] private float _minZoomEventTimeInterval = 0.2f;
        [SerializeField] private Transform maxZoomAnchor;

        private CameraInput input;
        InputAction pointAction;
        InputAction zoomAction;

        public float minZoom => _zoomLimits.min;
        public float maxZoom => _zoomLimits.max;

        private int minZoomLevel = 0;
        private int maxZoomLevel => maxZoomAnchor ? 4 : 3;
        private int currentZoomLevel = 2;
        public float currentRelativeZoom { get; private set; }

        private float _lastZoomEventTime;

        public float currentZoom { get; private set; }

        public int zoomLevel
        {
            get => currentZoomLevel;
            set => ApplyZoomLevel(value);
        }

        public bool isZooming => Touch.activeTouches.Count > 1;
        protected float zoomLerpSpeed => _zoomLerpSpeed;

        public new CameraController camera { get; private set; }

        public void Initialize(CameraController controller, Context context)
        {
            if (context != null)
            {
                _zoomLimits = context.zoomLimits;
                _zoomLerpSpeed = context.zoomLerpSpeed;
            }

            camera = controller;
            input = controller.components.OfType<CameraInput>().FirstOrDefault();
            if (!input)
            {
                throw new Exception("Camera input missing");
            }

            pointAction = input.inputMaster.FindAction("Point");
            pointAction.Enable();

            zoomAction = input.inputMaster.FindAction(InputActionId.Zoom);
            zoomAction.performed += OnZoom;
            zoomAction.Enable();

            ApplyZoomLevel(zoomLevel);
        }

        public void ChangeZoomLevel(int zoomLevel)
        {
            ApplyZoomLevel(zoomLevel);
        }

        public void ZoomDefault()
        {
            ApplyZoomLevel(2);
        }

        public void ZoomCurrent()
        {
            ApplyZoomLevel(currentZoomLevel);
        }

        public void ZoomIn()
        {
            if (currentZoomLevel <= 0) return;
            ApplyZoomLevel(currentZoomLevel - 1);
        }

        public void ZoomOut()
        {
            if (currentZoomLevel >= maxZoomLevel) return;
            ApplyZoomLevel(currentZoomLevel + 1);
        }

        void OnZoom(InputAction.CallbackContext context)
        {
            if (camera.HasInputOverride(InputActionId.Zoom)) return;

            var input = context.ReadValue<float>();

            if (Mathf.Abs(input) <= 0 || Time.realtimeSinceStartup - _lastZoomEventTime < _minZoomEventTimeInterval)
            {
                return;
            }

            if (InputHelper.IsPointerOverUIObject(pointAction.ReadValue<Vector2>(), out _))
            {
                return;
            }

            var scroll = input > 0 ? -1 : 1;
            ApplyZoomLevel(Mathf.Clamp(currentZoomLevel + scroll, minZoomLevel, maxZoomLevel));

            _lastZoomEventTime = Time.realtimeSinceStartup;
        }

        private void RecalculateCurrentRelativeZoom()
        {
            currentRelativeZoom = (camera.camera.orthographicSize - minZoom) / (maxZoom - minZoom);
        }

        private void ApplyZoomLevel(int zoomLevel)
        {
            currentZoomLevel = zoomLevel;
            currentZoom = minZoom + (maxZoom - minZoom) *
                Mathf.Pow(1f / (maxZoomLevel - minZoomLevel) * currentZoomLevel, 2f);
            // if (zoomLevel == maxZoomLevel)
            // {
            //     MessageTypeId.Area.SpeechBubbles.HideSpeechBubbles();
            //     MessageTypeId.Area.TweetBubbles.HideSpeechBubbles();
            //     MessageTypeId.Area.NameTags.HideNameTags();
            // }
            // else
            // {
            //     MessageTypeId.Area.SpeechBubbles.ShowSpeechBubbles();
            //     MessageTypeId.Area.TweetBubbles.ShowSpeechBubbles();
            //     MessageTypeId.Area.NameTags.ShowNameTags();
            // }
        }

        public CameraState UpdateState(CameraState state, CameraState modified)
        {
            RecalculateCurrentRelativeZoom();
            modified.distance = Mathf.SmoothStep(modified.distance, currentZoom, Time.deltaTime * zoomLerpSpeed);
            return modified;
        }

        public class Context : CameraController.Context
        {
            public MinMaxRange zoomLimits = new(7, 70);
            public float zoomLerpSpeed = 10;
        }
    }
}