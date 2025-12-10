using UnityEngine;
using UnityEngine.UI;

namespace Runtime.GameContent.UI
{
    public class VolumeSlider : MonoBehaviour
    {
        #region Functions
        private void Awake()
        {
            volumeSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    volumeSlider.value = Management.AudioManager.AudioManager.Instance.masterVolume;
                    break;
                case VolumeType.Music:
                    volumeSlider.value = Management.AudioManager.AudioManager.Instance.musicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = Management.AudioManager.AudioManager.Instance.SFXVolume;
                    break;
            }
        }
    
        public void OnSliderValueChanged()
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    Management.AudioManager.AudioManager.Instance.masterVolume = volumeSlider.value;
                    break;
                case VolumeType.Music:
                    Management.AudioManager.AudioManager.Instance.musicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    Management.AudioManager.AudioManager.Instance.SFXVolume = volumeSlider.value;
                    break;
            }
        }
        #endregion
        
        #region Fields
        private enum VolumeType
        {
            Master,
            Music,
            SFX,
        }
    
        [Header("Type")]
        [SerializeField] private VolumeType volumeType;
    
        [Header("Slider")]
        private Slider volumeSlider;
        #endregion
    }
}
