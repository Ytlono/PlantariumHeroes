using UnityEngine;

namespace MyGameProject
{
    public class CameraController : MonoBehaviour
    {
        #region Public fields

        [Space(10.0f)]
        [Tooltip("Point aimed by the camera")]
        public Transform Target;

        [Tooltip("Maximum distance between the camera and Target")]
        public float Distance = 2;

        [Tooltip("Distance lerp factor")]
        [Range(.0f, 1.0f)]
        public float LerpSpeed = .1f;

        [Space(10.0f)]
        [Tooltip("Collision parameters")]
        public TraceInfo RayTrace = new TraceInfo { Thickness = .2f };

        [Tooltip("Camera pitch limitations")]
        public LimitsInfo PitchLimits = new LimitsInfo { Minimum = -60.0f, Maximum = 60.0f };

        [Tooltip("Input axes used to control the camera")]
        public InputInfo InputAxes = new InputInfo
        {
            Horizontal = new InputAxisInfo { Name = "Mouse X", Sensitivity = 15.0f },
            Vertical = new InputAxisInfo { Name = "Mouse Y", Sensitivity = 15.0f }
        };

        [Tooltip("Scroll sensitivity for zooming the camera")]
        public float ScrollSensitivity = 2.0f;

        [Tooltip("Minimum allowed zoom distance")]
        public float MinZoomDistance = 1.0f;

        [Tooltip("Maximum allowed zoom distance")]
        public float MaxZoomDistance = 10.0f;

        #endregion

        #region Structs

        [System.Serializable]
        public struct LimitsInfo
        {
            public float Minimum;
            public float Maximum;
        }

        [System.Serializable]
        public struct TraceInfo
        {
            public float Thickness;
            public LayerMask CollisionMask;
        }

        [System.Serializable]
        public struct InputInfo
        {
            public InputAxisInfo Horizontal;
            public InputAxisInfo Vertical;
        }

        [System.Serializable]
        public struct InputAxisInfo
        {
            public string Name;
            public float Sensitivity;
        }

        #endregion

        private float _pitch;
        private float _distance;
        private CursorController cursorController;

        public void Start()
        {
            _pitch = Mathf.DeltaAngle(0, -transform.localEulerAngles.x);
            _distance = Distance;
            cursorController = FindObjectOfType<CursorController>();
        }

        public void Update()
        {
            if (cursorController != null && cursorController.IsMouseVisible)
            {
                return;
            }

            float yaw = transform.localEulerAngles.y + Input.GetAxis(InputAxes.Horizontal.Name) * InputAxes.Horizontal.Sensitivity;

            _pitch += Input.GetAxis(InputAxes.Vertical.Name) * InputAxes.Vertical.Sensitivity;
            _pitch = Mathf.Clamp(_pitch, PitchLimits.Minimum, PitchLimits.Maximum);

            transform.localEulerAngles = new Vector3(-_pitch, yaw, 0);

            HandleScroll();
        }

        public void LateUpdate()
        {
            if (cursorController != null && cursorController.IsMouseVisible)
            {
                return;
            }

            if (Target == null) return;

            var startPos = Target.position;
            var endPos = startPos - transform.forward * Distance;
            var result = Vector3.zero;

            RayCast(startPos, endPos, ref result, RayTrace.Thickness);
            var resultDistance = Vector3.Distance(Target.position, result);

            if (resultDistance <= _distance)
            {
                transform.position = result;
                _distance = resultDistance;
            }
            else
            {
                _distance = Mathf.Lerp(_distance, resultDistance, LerpSpeed);
                transform.position = startPos - transform.forward * _distance;
            }
        }

        private bool RayCast(Vector3 start, Vector3 end, ref Vector3 result, float thickness)
        {
            var direction = end - start;
            var distance = Vector3.Distance(start, end);

            RaycastHit hit;
            if (Physics.SphereCast(new Ray(start, direction), thickness, out hit, distance, RayTrace.CollisionMask.value))
            {
                result = hit.point + hit.normal * RayTrace.Thickness;
                return true;
            }
            else
            {
                result = end;
                return false;
            }
        }

        private void HandleScroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                Distance = Mathf.Clamp(Distance - scroll * ScrollSensitivity, MinZoomDistance, MaxZoomDistance);
            }
        }

        public float GetSensitivity()
        {
            return (InputAxes.Horizontal.Sensitivity + InputAxes.Vertical.Sensitivity + ScrollSensitivity) / 3.0f;
        }

        public void AdjustSensitivity(float value)
        {
            InputAxes.Horizontal.Sensitivity = value;
            InputAxes.Vertical.Sensitivity = value;
            ScrollSensitivity = value;
        }
    }
}
