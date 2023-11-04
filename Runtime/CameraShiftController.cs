namespace Cameras
{
    // [RequireComponent(typeof(IsoCameraController))]
    // public class CameraShiftController : OrientationLogic
    // {
    //     [SerializeField] [HideInInspector] private IsoCameraController _camera;
    //
    //     private Canvas DrawerCanvas { get; set; }
    //
    //     private bool _mainNavVisible;
    //     private bool _drawerVisible;
    //
    //     protected override void OnEnable()
    //     {
    //         if (!Client || Client.Store == null) return;
    //         DrawerCanvas = Client.Drawers.GetComponentInParent<Canvas>().rootCanvas;
    //         RecalculateShift();
    //         Client.Store.Ui.gameViewportInsetsChanged += OnInsetsChanged;
    //     }
    //
    //     private void OnDisable()
    //     {
    //         if (!Client || Client.Store == null) return;
    //         Client.Store.Ui.gameViewportInsetsChanged -= OnInsetsChanged;
    //     }
    //
    //     private void OnInsetsChanged(Vector4 insets)
    //     {
    //         _camera.horizontalShift = CalculateShift(Screen.width - (insets.z - insets.x));
    //     }
    //
    //     private void RecalculateShift()
    //     {
    //         OnInsetsChanged(Client.Store.Ui.gameViewportInsets);
    //     }
    //
    //     private float CalculateShift(float playAreaWidth)
    //     {
    //         if (isPortrait) return 0;
    //         var fullScreenWidth = Screen.width / (float)Screen.height * DrawerCanvas.scaleFactor;
    //         return (Screen.width - playAreaWidth) / Screen.width * fullScreenWidth * _camera.orthographicSize;
    //     }
    //
    //     public override void ApplyLandscapeState()
    //     {
    //         RecalculateShift();
    //     }
    //
    //     public override void ApplyPortraitState()
    //     {
    //         RecalculateShift();
    //     }
    //
    //     private void OnValidate()
    //     {
    //         if (!_camera) _camera = GetComponent<IsoCameraController>();
    //     }
    // }
}