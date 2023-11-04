using System.Linq;
using Cameras.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Cameras.Aspects
{
    [RequireComponent(typeof(CameraZoom))]
    public class CameraDepthOfField : MonoBehaviour, ICameraComponent<CameraDepthOfField.Context>
    {
        [SerializeField] private Volume volume;

        private Context context;
        private CameraZoom zoom;
        private VolumeProfile profile;
        private DepthOfField depthOfField;
        
        public new CameraController camera { get; private set; }
        
        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            this.context = context;
            zoom = controller.components.OfType<CameraZoom>().FirstOrDefault();
            profile = Instantiate(volume.profile);
            volume.profile = profile;
            depthOfField = profile.components.OfType<DepthOfField>().FirstOrDefault();
        }

        private void LateUpdate()
        {
            if (depthOfField && zoom)
            {
                depthOfField.gaussianStart.value = zoom.currentZoom + context.blurStart;
                depthOfField.gaussianEnd.value = zoom.currentZoom + context.blurEnd;
            }
        }

        private void OnDestroy()
        {
            if (profile) Destroy(profile);
        }
        
        public class Context : CameraController.Context
        {
            public float blurStart = 5;
            public float blurEnd = 40;
        }
    }
}