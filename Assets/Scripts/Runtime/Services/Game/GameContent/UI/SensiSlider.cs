using Runtime.Services.Data;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI;

public class SensiSlider : MonoBehaviour
{
    #region methodes

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        var d = ServiceLocator.Instance.Get<DataService>();

        _slider.value = d.sensi;
    }

    public void OnValueChanged(float value)
    {
        var d = ServiceLocator.Instance.Get<DataService>();
        
        d.sensi = value;
    }

    #endregion
    
    #region fields

    private Slider _slider;

    #endregion
}