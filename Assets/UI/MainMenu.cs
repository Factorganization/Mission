using UI.Modals.MailPage;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _mailButton.RegisterCallback<ClickEvent>(OpenMailPage);
            _customizationButton.RegisterCallback<ClickEvent>(OpenCustomizationPage);
            _optionsButton.RegisterCallback<ClickEvent>(OpenOptionsPage);
            _quitButton.RegisterCallback<ClickEvent>(QuitPopUP);
            
            if (_levelDataSO._allLevels.Length == 0)
            {
                _levelDataSO._allLevels = Resources.LoadAll<MailLevel>("UI/Mail/");
                System.Array.Sort(_levelDataSO._allLevels, (a,b) => a.name.CompareTo(b.name));
            }
            
            _mailPage = new MailPage(_document.rootVisualElement.Q<VisualElement>("mailpage"), 
                _levelDataSO._allLevels);
            
            _mailPage.OnMailSelected += OnMailSelected;
        }
        
        private void OpenMailPage(ClickEvent _ = null)
        {
            _mailPage.Show();
        }
        
        private void OpenCustomizationPage(ClickEvent _ = null)
        {
           
        }
        
        private void OpenOptionsPage(ClickEvent _ = null)
        {
           
        }

        private void QuitPopUP(ClickEvent _ = null)
        {
            
        }
        
        private void OnMailSelected(MailLevel mailSignet)
        {
            _levelDataSO._mailLevel = mailSignet;
            Debug.Log("Selected mail: " + mailSignet.Subject);
        }

        #region Fields

        private Button _mailButton => _document.rootVisualElement.Q<Button>("mail-button");
        private Button _customizationButton => _document.rootVisualElement.Q<Button>("customization-button");
        private Button _optionsButton => _document.rootVisualElement.Q<Button>("options-button");
        private Button _quitButton => _document.rootVisualElement.Q<Button>("quit-button");
        
        private UIDocument _document;
        private MailPage _mailPage;
        
        [SerializeField] private LevelDataSO _levelDataSO;

        #endregion
    }
}
