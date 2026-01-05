using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Runtime.Services.Game.GameContent.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _containerRectTransform;
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _containerRectTransform, Mouse.current.position.value, null, out localPos);

            // Clamp inside Window
            localPos.x = Mathf.Clamp(localPos.x, _containerRectTransform.rect.xMin + _rectTransform.rect.width/2, _containerRectTransform.rect.xMax - _rectTransform.rect.width/2);
            localPos.y = Mathf.Clamp(localPos.y, _containerRectTransform.rect.yMin + _rectTransform.rect.height/2, _containerRectTransform.rect.yMax - _rectTransform.rect.height/2);

            _rectTransform.position = _containerRectTransform.TransformPoint(localPos);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}