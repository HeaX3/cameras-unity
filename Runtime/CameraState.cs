using UnityEngine;

namespace Cameras
{
    public struct CameraState
    {
        /// <summary>
        /// World position
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// Local position of the pivot inside the anchor
        /// </summary>
        public Vector3 offset;

        /// <summary>
        /// YXZ rotation where YX are applied to the pivot, and Z is applied to the camera
        /// </summary>
        public Vector3 rotation;

        /// <summary>
        /// Distance of the camera to the pivot
        /// </summary>
        public float distance;

        /// <summary>
        /// FOV of the camera in perspective mode, view height in orthographic mode
        /// </summary>
        public float size;
        
        /// <summary>
        /// Local shift of the camera relative to the screen plane
        /// </summary>
        public Vector2 shift;

        public override string ToString()
        {
            return $"{nameof(position)}: {position}, {nameof(offset)}: {offset}, {nameof(rotation)}: {rotation}, {nameof(distance)}: {distance}, {nameof(size)}: {size}, {nameof(shift)}: {shift}";
        }
    }
}