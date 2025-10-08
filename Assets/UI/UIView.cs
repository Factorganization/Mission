using System;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class UIView : IDisposable
    {
        protected bool _hideOnAwake { get; set; }
        protected bool _isOverlay { get; set; }
        public VisualElement Root { get; set; }
        public bool IsHidden => Root.ClassListContains("hidden");

        public UIView() {}

        public UIView(VisualElement root)
        {
            Initialize(root);
        }

        public virtual void Initialize(VisualElement root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            SetVisualElements();
            RegisterButtonCallbacks();
            if (_hideOnAwake)
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            Root.RemoveFromClassList("hidden");
            Root.AddToClassList("visible");
        }

        public virtual void Hide()
        {
            Root.RemoveFromClassList("visible");
            Root.AddToClassList("hidden");
        }

        protected virtual void RegisterButtonCallbacks() {}

        protected virtual void SetVisualElements() {}

        public abstract void Dispose();
    }
}
