using Runtime.Services.Audio;
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
            var s = ServiceLocator.Instance.Get<AudioService>();
            
            switch (volumeType)
            {
                case VolumeType.Master:
                    volumeSlider.value = s.MasterVolume;
                    break;
                case VolumeType.Music:
                    volumeSlider.value = s.MusicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = s.SfxVolume;
                    break;
            }
        }
    
        public void OnSliderValueChanged()
        {
            var s = ServiceLocator.Instance.Get<AudioService>();
            
            switch (volumeType)
            {
                case VolumeType.Master:
                    s.MasterVolume = volumeSlider.value;
                    break;
                case VolumeType.Music:
                    s.MusicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    s.SfxVolume = volumeSlider.value;
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