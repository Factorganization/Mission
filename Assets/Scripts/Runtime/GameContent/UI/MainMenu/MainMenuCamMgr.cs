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

        private void Update()
        {

        }

        private void Initialize()
        {

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
        
        #endregion

    }

}