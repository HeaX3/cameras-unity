using Cameras.Interfaces;
using UnityEngine;

namespace Cameras.Aspects
{
    /// <summary>
    /// Automatically follow a target using a slow follow logic
    /// </summary>
    public class CameraFollow : MonoBehaviour, ICameraComponent<CameraFollow.Context>, ICameraStateHandler
    {
        [SerializeField] private float _positionLerpSpeed;
        
        private Context context;
        
        public new CameraController camera { get; private set; }
        
        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            this.context = context;
        }

        public CameraState UpdateState(CameraState state, CameraState modified)
        {
            if (!context?.target) return modified;
            var position = context.target.position + new Vector3(0, context.verticalOffset, 0);
            modified.position = Vector3.Lerp(transform.position, position, _positionLerpSpeed * Time.deltaTime);
            return modified;
        }
        
        public class Context : CameraController.Context
        {
            public Transform target;
            public float verticalOffset;
        }
    }
}