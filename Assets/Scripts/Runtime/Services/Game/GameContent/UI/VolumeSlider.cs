using Runtime.Services.Audio.AudioSystems;
using UnityEngine.UI;

namespace Runtime.Services.Game.GameContent.UI
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
                    volumeSlider.value = AudioManager.Instance.masterVolume;
                    break;
                case VolumeType.Music:
                    volumeSlider.value = AudioManager.Instance.musicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = AudioManager.Instance.SFXVolume;
                    break;
            }
        }
    
        public void OnSliderValueChanged()
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    AudioManager.Instance.masterVolume = volumeSlider.value;
                    break;
                case VolumeType.Music:
                    AudioManager.Instance.musicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    AudioManager.Instance.SFXVolume = volumeSlider.value;
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