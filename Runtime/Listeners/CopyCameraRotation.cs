using Cameras.Aspects;
using UnityEngine;

namespace Cameras.Listeners
{
    public class CopyCameraRotation : MonoBehaviour
    {
        private new Transform transform;
        private Transform _camera;

        protected void Awake()
        {
            transform = base.transform;
        }

        private void OnEnable()
        {
            var controller = transform.root.gameObject.GetComponentInChildren<CameraFollow>(true);
            _camera = controller ? controller.camera.camera.transform : default;
        }

        private void LateUpdate()
        {
            if (_camera)
            {
                transform.rotation = _camera.rotation;
                //Debug.Log(transform.rotation);
            }
        }
    }
}