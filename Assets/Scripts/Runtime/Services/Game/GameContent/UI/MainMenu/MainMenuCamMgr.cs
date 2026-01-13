using UnityEngine.InputSystem;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class MainMenuCamMgr : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
            
            if (!_firstTimeGameOpened && _mainMenuCam != null && _endPos != null)
            {
                _mainMenuCam.transform.position = _endPos.position;
                _mainMenuCam.transform.rotation = _endPos.rotation;
                _isAtEndPos = true;
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && !_isAtEndPos && _firstTimeGameOpened)
            {
                StartMoveTo(_endPos);
                _isAtEndPos = true;
            }
            
            if (_isMoving)
            {
                SmoothUpdate(Time.deltaTime);
            }
        }

        private void Initialize()
        {
            if (_mainMenuCam != null && _startPos != null)
            {
                _mainMenuCam.transform.position = _startPos.position;
                _mainMenuCam.transform.rotation = _startPos.rotation;
            }
        }

        private void StartMoveTo(Transform targetPos)
        {
            _targetPos = targetPos;
            _isMoving = _targetPos != null;
        }

        private void SmoothUpdate(float deltaTime)
        {
            var camTransform = _mainMenuCam.transform;
            Vector3 targetPosition = _targetPos.position;
            camTransform.position = Vector3.SmoothDamp(camTransform.position, targetPosition, ref _positionVelocity, _positionSmoothTime, Mathf.Infinity, deltaTime);
            
            Quaternion targetRot = _targetPos.rotation;
            float t = 1f - Mathf.Exp(-_rotationSpeed * deltaTime); // frame-rate independent fraction
            camTransform.rotation = Quaternion.Slerp(camTransform.rotation, targetRot, t);
            
            bool closePos = Vector3.Distance(camTransform.position, targetPosition) <= _stopPositionThreshold;
            bool closeRot = Quaternion.Angle(camTransform.rotation, targetRot) <= _stopRotationThreshold;

            if (closePos && closeRot)
            {
                // Snap to final to avoid tiny residual differences
                camTransform.position = targetPosition;
                camTransform.rotation = targetRot;
                _isMoving = false;
                _firstTimeGameOpened = false;
            }
        }

        #endregion

        #region Fields
        
        [SerializeField] private Camera _mainMenuCam;
        [SerializeField] private Transform _startPos, _endPos;
        
        [Header("Smoothing")]
        [SerializeField] private float _positionSmoothTime = 0.2f;
        [SerializeField] private float _rotationSpeed = 8f; // higher = faster rotation
        [SerializeField] private float _stopPositionThreshold = 0.01f;
        [SerializeField] private float _stopRotationThreshold = 0.5f;

        [SerializeField] private bool _firstTimeGameOpened = true;

        private Vector3 _positionVelocity;
        private bool _isMoving;
        private Transform _targetPos;
        private bool _isAtEndPos;
        
        public bool FirstTimeGameOpened => _firstTimeGameOpened;
        
        #endregion

    }
}