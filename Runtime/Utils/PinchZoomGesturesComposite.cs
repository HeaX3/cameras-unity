using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


namespace Cameras.Utils
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [DisplayStringFormat("{touch0}+{touch1}")]
    public class PinchZoomGesturesComposite : InputBindingComposite<float>
    {
        [InputControl(layout = "Value")]
        public int touch0;
        [InputControl(layout = "Value")]
        public int touch1;

        public float negativeScale = 1f;
        public float positiveScale = 1f;

        private struct TouchStateComparer : IComparer<TouchState>
        {
            public int Compare(TouchState x, TouchState y) => 1;
        }

        public override float ReadValue(ref InputBindingCompositeContext context)
        {
            var touch_0 = context.ReadValue<TouchState, TouchStateComparer>(touch0);
            var touch_1 = context.ReadValue<TouchState, TouchStateComparer>(touch1);

            if (touch_0.phase != TouchPhase.Moved || touch_1.phase != TouchPhase.Moved)
                return 0f;

            var startDistance = Vector2.Distance(touch_0.startPosition, touch_1.startPosition);
            var distance = Vector2.Distance(touch_0.position, touch_1.position);

            var unscaledValue = 1f - startDistance / distance;
            return unscaledValue * (unscaledValue < 0 ? negativeScale : positiveScale);
        }

        public override float EvaluateMagnitude(ref InputBindingCompositeContext context) => 1f;

        static PinchZoomGesturesComposite() => InputSystem.RegisterBindingComposite<PinchZoomGesturesComposite>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() { }
    }
}