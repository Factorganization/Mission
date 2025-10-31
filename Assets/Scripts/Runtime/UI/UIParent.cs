using UnityEngine;

public class UIParent : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
