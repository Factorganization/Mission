using UnityEngine;
using UnityEngine.UIElements;

public class CustomSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _slider = _root.Q<VisualElement>("MySlider");
        _dragger = _slider.Q<VisualElement>("unity-dragger");
        
        AddElements();
    }

    void AddElements()
    {
        _bar = new VisualElement();
        _dragger.Add(_bar);
        _bar.name = "Bar";
        _bar.AddToClassList("bar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region  Fields

    private VisualElement _root;
    private VisualElement _slider;
    private VisualElement _dragger;
    private VisualElement _bar;

    #endregion
}
