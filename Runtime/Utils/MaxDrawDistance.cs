using System.Collections;
using UnityEngine;

namespace Cameras.Utils
{
    public class MaxDrawDistance : MonoBehaviour
    {
        [SerializeField] private float maxDrawDistance = 100;
        [SerializeField] private Renderer[] _renderers;

        private float maxDrawDistanceSquared;
        private new Transform transform;
        private Transform referenceTransform;

        private bool _visible;
        private bool _renderersVisible = true;
        private float _size = 1;
        private float _sizeChangeSpeed;

        private bool visible
        {
            get => _visible;
            set => ApplyVisible(value);
        }

        private bool renderersVisible
        {
            get => _renderersVisible;
            set => ApplyRenderersVisible(value);
        }

        private float size
        {
            get => _size;
            set => ApplySize(value);
        }

        private void OnEnable()
        {
            transform = base.transform;
            maxDrawDistanceSquared = maxDrawDistance * maxDrawDistance;
            if (referenceTransform) StartCoroutine(UpdateRoutine());
            visible = visible;
        }

        private void LateUpdate()
        {
            var targetSize = visible ? 1 : 0;
            if (Mathf.Abs(targetSize - _size) <= 0.001f) return;
            size = Mathf.SmoothDamp(size, targetSize, ref _sizeChangeSpeed, 0.5f);
        }

        private void ApplyVisible(bool visible)
        {
            _visible = visible;
        }

        private void ApplySize(float size)
        {
            _size = size;
            transform.localScale = Vector3.one * size;
            if (size > 0.001f && !renderersVisible) renderersVisible = true;
            else if (size <= 0.001f && renderersVisible) renderersVisible = false;
        }

        private void ApplyRenderersVisible(bool visible)
        {
            _renderersVisible = visible;
            foreach (var r in _renderers)
            {
                r.enabled = visible;
            }
        }

        private IEnumerator UpdateRoutine()
        {
            while (this)
            {
                var distanceSquared = (transform.position - referenceTransform.position).sqrMagnitude;
                var visible = distanceSquared <= maxDrawDistanceSquared;
                if (visible != this.visible) this.visible = visible;
                yield return new WaitForSeconds(1);
            }
        }
    }
}