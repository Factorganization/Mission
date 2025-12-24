using Runtime.Service;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

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

        private void OnEnable()
        {
            _currentMouse = Mouse.current;
            
            if (_virtualMouse is null)
            {
                _virtualMouse = InputSystem.AddDevice<Mouse>("VirtualMouse");
            }
            else if (!_virtualMouse.added)
            {
                InputSystem.AddDevice(_virtualMouse);
            }

            InputUser.PerformPairingWithDevice(_virtualMouse, playerInput.user);

            if (cursorTransform is not null)
            {
                var pos = cursorTransform.anchoredPosition;
                InputState.Change(_virtualMouse.position, pos);
            }
            
            InputSystem.onAfterUpdate += UpdateMotion;
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            if (_virtualMouse is not null && _virtualMouse.added)
                InputSystem.RemoveDevice(_virtualMouse);
            
            InputSystem.onAfterUpdate -= UpdateMotion;
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void UpdateMotion()
        {
            if (_virtualMouse is null || Gamepad.current is null)
                return;
            
            var deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;
            
            var currentPos = _virtualMouse.position.ReadValue();
            var newPos = currentPos + deltaValue;
            
            newPos.x = Mathf.Clamp(newPos.x, padding, Screen.width - padding);
            newPos.y = Mathf.Clamp(newPos.y, padding, Screen.height - padding);
            
            InputState.Change(_virtualMouse.position, newPos);
            InputState.Change(_virtualMouse.delta, deltaValue);
            
            var aButtonIsPressed = Gamepad.current.aButton.IsPressed();
            if (_previousMouseState != aButtonIsPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousMouseState = aButtonIsPressed;
            }
            
            AnchorCursor(newPos);
        }

        private void AnchorCursor(Vector2 anchor)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, anchor, null, out var anchoredPos);
            cursorTransform.anchoredPosition = anchoredPos;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            if (playerInput is null || _virtualMouse is null)
                return;

            if (playerInput.currentControlScheme == mouseScheme && _previousControlScheme != mouseScheme)
            {
                cursorTransform.gameObject.SetActive(false);
                UnityEngine.Cursor.visible = true;
                _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                _previousControlScheme = mouseScheme;
            }
            else if (playerInput.currentControlScheme == gamepadScheme && _previousControlScheme != gamepadScheme)
            {
                cursorTransform.gameObject.SetActive(true);
                UnityEngine.Cursor.visible = false;
                InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                AnchorCursor(_currentMouse.position.ReadValue());
                _previousControlScheme = gamepadScheme;
            }
        }

        public void SetActive(bool active)
        {
            _mouseVisible = active;
        }

        #endregion

        #region fields

        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private RectTransform canvasTransform;
        
        [SerializeField] private RectTransform cursorTransform;
        
        [SerializeField] private float cursorSpeed;

        [SerializeField] private float padding;
        
        private Mouse _virtualMouse;

        private Mouse _currentMouse;

        private string _previousControlScheme = "";
        
        private bool _previousMouseState;
        
        private bool _mouseVisible;

        private const string gamepadScheme = "Gamepad";
        
        private const string mouseScheme = "Keyboard&Mouse";

        #endregion
    }
}