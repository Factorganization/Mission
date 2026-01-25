using Runtime.Services.Data;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI.GameUI
{
    public class MaliceCounter : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            UpdateMaliceCount();
        }

        private void UpdateMaliceCount()
        {
            
        }

        #endregion

        #region Fields

        [SerializeField] private TextMeshProUGUI _mailiceText;

        #endregion
    }
}

