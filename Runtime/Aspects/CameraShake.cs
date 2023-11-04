using Cameras.Interfaces;
using UnityEngine;

namespace Cameras.Aspects
{
    public class CameraShake : MonoBehaviour, ICameraComponent<CameraShake.Context>, ICameraStateModifier
    {
        private Vector2 shake;
        private float shakeDuration;
        private Vector2 currentShakePosition;
        private Vector2 currentShakeTarget;
        private Vector2 currentShakeVelocity;
        
        public new CameraController camera { get; private set; }

        public void Initialize(CameraController controller, Context context)
        {
            camera = controller;
            throw new System.NotImplementedException();
        }

        public void Shake(Vector2 shake, float duration)
        {
            this.shake.x = Mathf.Max(this.shake.x, shake.x);
            this.shake.y = Mathf.Max(this.shake.y, shake.y);
            shakeDuration = duration;
            currentShakeTarget = Random.insideUnitCircle;
        }

        public class Context : CameraController.Context
        {
        }

        public CameraState ModifyState(CameraState state, CameraState currentState)
        {
            if (shakeDuration > 0)
            {
                shakeDuration -= Time.deltaTime;
                currentShakePosition =
                    Vector2.MoveTowards(currentShakePosition, currentShakeTarget, 100 * Time.deltaTime);
                if ((currentShakeTarget - currentShakePosition).sqrMagnitude < 0.001f)
                {
                    currentShakeTarget = Random.insideUnitCircle;
                }

                var shift = currentState.shift;
                shift.x += currentShakePosition.x * shake.x * Mathf.Clamp01(shakeDuration * 4);
                shift.y += currentShakePosition.y * shake.y * Mathf.Clamp01(shakeDuration * 4);
                currentState.shift = shift;
            }

            return currentState;
        }
    }
}