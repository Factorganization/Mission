using Runtime.Service;
using UnityEngine.InputSystem;

namespace Runtime.Services.Cursor
{
    public class CursorService : AService
    {
        #region methodes

        public override void Begin()
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            _mouseVisible = true;
        }

        public override void Tick()
        {
            if (!_mouseVisible)
            {
                _mousePos = new Vector2(Screen.width / 2f, Screen.height / 2f);
                return;
            }

            var mousePos = moveInput.action.ReadValue<Vector2>() * mouseSpeed;
            _mousePos += new Vector2(mousePos.x * 1920 / Screen.width, mousePos.y * 1080 / Screen.height);

			_mousePos.x =  Mathf.Clamp(_mousePos.x, 0, Screen.width);
			_mousePos.y =  Mathf.Clamp(_mousePos.y, 0, Screen.height);

            Mouse.current.WarpCursorPosition(_mousePos);
        }

        public void SetActive(bool active)
        {
            _mouseVisible = active;
			UnityEngine.Cursor.visible = active;
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