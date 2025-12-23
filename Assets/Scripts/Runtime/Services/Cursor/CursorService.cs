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
            
            var mousePos = moveInput.action.ReadValue<Vector2>();
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
            
            Debug.Log($"Mouse pos: {_mousePos.x}, {_mousePos.y}");
            Debug.Log($"Mouse real pos: {Mouse.current.position.ReadValue().x}, {Mouse.current.position.ReadValue().y}");
        }

        public void SetActive(bool active)
        {
            _mouseVisible = active;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireCube();
        }

        #endregion

        #region fields
    
        [SerializeField] private InputActionReference moveInput;
        
        private Vector2 _mousePos;

        private bool _mouseVisible;

        #endregion
    }
}