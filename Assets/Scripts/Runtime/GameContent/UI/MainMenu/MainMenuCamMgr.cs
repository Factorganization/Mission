using UnityEngine;

namespace Runtime.GameContent.UI.MainMenu
{
    public class MainMenuCamMgr : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mainMenuCam.transform.position = _startPos.position;
        }
        
        private void MoveCameraToPosition(Transform targetPos)
        {
            if (_mainMenuCam != null && targetPos != null)
            {
                _mainMenuCam.transform.Translate(targetPos.position);
                _mainMenuCam.transform.Rotate(targetPos.rotation.eulerAngles);
            }
        }

        #endregion

        #region Fields
        
        [SerializeField] private Camera _mainMenuCam;
        [SerializeField] private Transform _startPos, _endPos;

        #endregion

    }

}