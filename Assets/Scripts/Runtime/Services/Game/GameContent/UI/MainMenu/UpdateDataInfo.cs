using Runtime.Services.Data;
using TMPro;
using UnityEngine;

namespace Runtime.Services.Game.GameContent.UI.MainMenu
{
    public class UpdateDataInfo : MonoBehaviour
    {
        #region Functions

        private void Start()
        {
            UpdateData();
        }

        public void UpdateData()
        {
            var dataService = ServiceLocator.Instance.Get<DataService>();
            if (_devilDollars != null)
                _devilDollars.text = dataService.DevilDollars.ToString("N0");
            if (_malicePoints != null)
                _malicePoints.text = dataService.MalicePoints.ToString("N0");
        }

        #endregion

        #region Fields

        // Define fields here
        [SerializeField] private TextMeshProUGUI _devilDollars, _malicePoints;

        #endregion
    }
}

