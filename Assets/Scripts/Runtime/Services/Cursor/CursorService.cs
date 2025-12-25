using Runtime.Service;
using UnityEngine.InputSystem;

namespace Runtime.Services.Cursor
{
    public class CursorService : AService
    {
        #region methodes

        private void Start()
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            _mouseVisible = true;
        }

        private void Update()
        {
            if (!_mouseVisible)
            {
                _mousePos = new Vector2(Screen.width / 2f, Screen.height / 2f);
                return;
            }
            
            var mousePos = moveInput.action.ReadValue<Vector2>() * mouseSpeed;
            _mousePos += new Vector2(mousePos.x * 1920 / Screen.width, mousePos.y * 1080 / Screen.height);
            
            if (_mousePos.x > Screen.width)
                _mousePos.x = Screen.width;
            if (_mousePos.y > Screen.height)
                _mousePos.y = Screen.height;
            if (_mousePos.x < 0)
                _mousePos.x = 0;
            if (_mousePos.y < 0)
                _mousePos.y = 0;
            
            Mouse.current.WarpCursorPosition(_mousePos);
        }

        public void SetActive(bool active)
        {
            _mouseVisible = active;
        }

        #endregion

        #region fields
    
        [SerializeField] private InputActionReference moveInput;

        [SerializeField] private float mouseSpeed;
        
        private Vector2 _mousePos;

        private bool _mouseVisible;

        #endregion
    }
}