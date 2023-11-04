using System;
using Cameras.Interfaces;
using UnityEngine;

namespace Cameras.Aspects
{
    /// <summary>
    /// Shift orthographic cameras when major UI elements obstruct part of the screen, to keep the center of attention
    /// within the center of the remaining major screen area
    /// </summary>
    public class CameraShift : MonoBehaviour, ICameraComponent<CameraShift.Context>, ICameraStateModifier
    {
        [SerializeField] private Vector3 _offset;

        public float orthographicSize { get; private set; }

        private float drawerScreenRatio;
        private float currentHorizontalShift;
        private float currentHorizontalShiftVelocity;

        public float horizontalShift { get; set; }
        
        public new CameraController camera { get; private set; }
        
        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            throw new System.NotImplementedException();
        }

        public class Context : CameraController.Context
        {
        }

        public CameraState ModifyState(CameraState state, CameraState currentState)
        {
            throw new NotImplementedException();
            //
            // orthographicSize = Mathf.Lerp(camera.orthographicSize, currentZoom, zoomLerpSpeed * Time.deltaTime);
            // currentHorizontalShift = Mathf.SmoothDamp(
            //     currentHorizontalShift, horizontalShift, ref currentHorizontalShiftVelocity, 0.1f
            // );
            // if (Mathf.Abs(camera.orthographicSize - orthographicSize) > 0.001f)
            // {
            //     camera.orthographicSize = orthographicSize;
            //     Client.Store.Ui.RecalculateGameViewportInset();
            // }
        }
    }
}