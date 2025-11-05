using UnityEngine;

namespace Runtime.Utils.Customization
{
    public class CustomCameraMatrix : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            var a = Screen.width / (float)Screen.height;
            var o = Matrix4x4.Ortho(-_cam.orthographicSize * a,
                _cam.orthographicSize * a,
                -_cam.orthographicSize,
                _cam.orthographicSize,
                _cam.nearClipPlane,
                _cam.farClipPlane);
            var p = Matrix4x4.Perspective(_cam.fieldOfView,
                a,
                _cam.nearClipPlane,
                _cam.farClipPlane);

            var taRace = Matrix4x4.identity;

            for (var i = 0; i < 16; i++)
            {
                taRace[i] = Mathf.Lerp(p[i], o[i], Mathf.Log(camFov));
            }

            _cam.projectionMatrix = taRace;
        }
        
        #endregion
        
        #region fields
        
        [Header("A passer dans le Start des que possible !!!!!!!!")]
        [SerializeField] [Range(1, 2.71828f)] private float camFov;
        
        private Camera _cam;
        
        #endregion
    }
}