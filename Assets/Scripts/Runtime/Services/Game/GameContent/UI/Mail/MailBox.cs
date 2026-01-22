using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.Mail
{
    public class MailBox : UIParent
    {
        #region Functions
        
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mailLevelPopup.Hide();
            
            foreach (var mail in _levelData._allLevels)
            {
                var mailGO = Instantiate(_mailPrefab);
                mailGO.transform.SetParent(_mailContainer, false);
                
                if (mailGO.TryGetComponent<RectTransform>(out var rt))
                {
                    rt.anchoredPosition3D = Vector3.zero;
                    rt.localScale = Vector3.one;
                }
                else
                {
                    mailGO.transform.localPosition = Vector3.zero;
                    mailGO.transform.localScale = Vector3.one;
                }

                var mailComponent = mailGO.GetComponent<Mail>();
                mailComponent.Bind(mail);
                mailComponent.OnMailSelected.AddListener(OnMailSelected);
            }
            
            _closeMailButton.onClick.AddListener(() => Hide());
        }

        private void OnMailSelected(MailLevel mailLevel)
        {
            Debug.Log(mailLevel.LevelName);
            _levelData._mailLevel = mailLevel;
            _mailLevelPopup.Bind(_levelData);
            _mailLevelPopup.Show();
        }
        
        public override void Show()
        {
            base.Show();
            _animator.Play("OpenMailPage");
        }
        
        public override void Hide()
        {
            _animator.Play("CloseMailPage");
        }
        
        #endregion
        
        #region Fields
        
        [SerializeField] private Button _closeMailButton;
        [SerializeField] private Transform _mailContainer;
        [SerializeField] private LevelPopup _mailLevelPopup;
        [SerializeField] private GameObject _mailPrefab;
        [SerializeField] private LevelDataSO _levelData;
        [SerializeField] private Animation _animator;

        #endregion
    }
}