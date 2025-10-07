using UI.Modals.MailPage;
using UI.Modals.ResearchPopUp;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        private Button _windowsButton => _document.rootVisualElement.Q<Button>("windows-button");
        
        private UIDocument _document;
        private MailPage _mailPage;
        private ResearchPopUp _researchPopUp;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            
            //_mailPage = new MailPage(_document.rootVisualElement.Q<VisualElement>("mail-page"));
            _researchPopUp = new ResearchPopUp(_document.rootVisualElement.Q<VisualElement>("research"));
            
            //_researchPopUp.SetMailPage(_mailPage);
            
            _windowsButton.RegisterCallback<ClickEvent>(OpenResearchPopUp);
        }
        
        private void OpenResearchPopUp(ClickEvent _ = null)
        {
            _researchPopUp.Show();
            Debug.Log("Open Research Pop Up");
        }
    }
}
