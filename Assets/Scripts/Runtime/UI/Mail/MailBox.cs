using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MailBox : UIParent
    {
        #region Functions

        // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                
                if (mailGO.TryGetComponent<UnityEngine.RectTransform>(out var rt))
                {
                    rt.anchoredPosition3D = Vector3.zero;
                    rt.localScale = Vector3.one;
                }
                else
                {
                    mailGO.transform.localPosition = Vector3.zero;
                    mailGO.transform.localScale = Vector3.one;
                }

                var mailComponent = mailGO.GetComponent<Mail.Mail>();
                mailComponent.Bind(mail);
                mailComponent.OnMailSelected.AddListener(OnMailSelected);
            }
            
            _closeMailButton.onClick.AddListener(() => Hide());
        }
        
        public void OnMailSelected(MailLevel mailLevel)
        {
            Debug.Log(mailLevel.LevelName);
            _levelData._mailLevel = mailLevel;
            _mailLevelPopup.Bind(_levelData);
            _mailLevelPopup.Show();
        }
        
        #endregion
        
        #region Fields
        
        [SerializeField] private Button _closeMailButton;
        [SerializeField] private Transform _mailContainer;
        [SerializeField] private LevelPopup _mailLevelPopup;
        [SerializeField] private GameObject _mailPrefab;
        [SerializeField] private LevelDataSO _levelData;

        #endregion
    }  
}