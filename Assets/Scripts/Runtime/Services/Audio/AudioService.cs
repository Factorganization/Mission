using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Runtime.Service;
using Runtime.Services.Audio.AudioContent;

namespace Runtime.Services.Audio
{
    public class AudioService : AService
    {
        #region properties

        [field : SerializeField] public AudioAtlas Atlas { get; private set; }
        
        public float MasterVolume { get; set; }
        
        public float MusicVolume { get; set; }
        
        public float SfxVolume { get; set; }

        #endregion

        #region methodes

        public override bool Init()
        {
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Musics");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
            
            return _masterBus.isValid() && _musicBus.isValid() && _sfxBus.isValid();
        }

        public override void Begin()
        {
            
        }

        public override void Tick()
        {
            
        }

        public override void Delete()
        {
            
        }

        #endregion

        #region fields

        private List<EventInstance> eventInstances = new();
        
        private List<StudioEventEmitter> eventEmitters = new();
        
        private EventInstance _musicEventInstance;
        
        private Bus _masterBus;
        
        private Bus _musicBus;
        
        private Bus _sfxBus;

        #endregion
    }
}