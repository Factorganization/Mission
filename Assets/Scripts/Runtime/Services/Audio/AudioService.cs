using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Runtime.Services.Audio.AudioContent;
using Runtime.Services.Data;
using Shared.Utils.ReadOnlyCustom;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Runtime.Services.Audio
{
    public class AudioService : AService
    {
        #region properties

        [field : SerializeField] public AudioAtlas Atlas { get; private set; }
        
        [field : ReadOnly][field : SerializeField] public float MasterVolume { get; set; }
        
        [field : ReadOnly][field : SerializeField] public float MusicVolume { get; set; }
        
        [field : ReadOnly][field : SerializeField] public float SfxVolume { get; set; }

        #endregion

        #region methodes

        #region service
        
        public override bool Init()
        {
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Musics");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
            
            return _masterBus.isValid() && _musicBus.isValid() && _sfxBus.isValid();
        }

        public override void Begin() //TODO setup musics
        {
            MasterVolume = ServiceLocator.Instance.Get<DataService>().masterVolume;
            MusicVolume = ServiceLocator.Instance.Get<DataService>().musicVolume;
            SfxVolume = ServiceLocator.Instance.Get<DataService>().sfxVolume;

            //SetMusic();
            //SetAmbience();
        }

        public override void Tick()
        {
            _masterBus.setVolume(MasterVolume);
            _musicBus.setVolume(MusicVolume);
            _sfxBus.setVolume(SfxVolume);
        }

        public override void Delete()
        {
            foreach (var inst in _eventInstances)
            {
                inst.stop(STOP_MODE.ALLOWFADEOUT);
                inst.release();
            }

            foreach (var emit in _eventEmitters)
            {
                emit.Stop();
            }
        }
        
        #endregion

        #region audio

        #region instance
        
        public EventInstance CreateInstance(EventReference path)
        {
            var eventInstance = RuntimeManager.CreateInstance(path);
            
            if (eventInstance.isValid())
                _eventInstances.Add(eventInstance);
            else
                Debug.LogError($"Failed to create instance for {path}");
            
            return eventInstance;
        }

        #endregion
        
        #region music
        
        private void SetMusic(EventReference path)
        {
            _musicEventInstance = CreateInstance(path);
            _musicEventInstance.start();
        }
        
        public void SetMusicImmediate(EventReference path)
        {
            _musicEventInstance.stop(STOP_MODE.IMMEDIATE);
            _musicEventInstance.release();
            SetMusic(path);
        }

        public void StopMusicSmooth()
        {
            _musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicEventInstance.release();
        }
        
        #endregion

        #region ambience

        private void SetAmbience(EventReference path)
        {
            _musicEventInstance = CreateInstance(path);
            _musicEventInstance.start();
        }
        
        public void SetAmbienceImmediate(EventReference path)
        {
            _musicEventInstance.stop(STOP_MODE.IMMEDIATE);
            _musicEventInstance.release();
            SetMusic(path);
        }

        public void StopAmbienceSmooth()
        {
            _musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicEventInstance.release();
        }

        public void SetAmbienceParameter(string parameterName, float parameterValue)
        {
            _ambientEventInstance.setParameterByName(parameterName, parameterValue);
        }

        #endregion

        #region emitters

        /// <summary>
        /// Play one time a sound effect at a specified position
        /// </summary>
        /// <param name="path">sound effect path</param>
        /// <param name="at">world position to emit the sound</param>
        public void PlayOneShot(EventReference path, Vector3 at)
        {
            RuntimeManager.PlayOneShot(path, at);
        }
        
        /// <summary>
        /// Here to replace the basic GetComponent in the start function of the emitting object
        /// </summary>
        /// <param name="path">sound effect path</param>
        /// <param name="go">self emitter game object</param>
        /// <returns>already attached emitter component but with set up value</returns>
        public StudioEventEmitter EmitterInit(EventReference path, GameObject go)
        {
            if (!go.TryGetComponent<StudioEventEmitter>(out var emitter))
            {
                Debug.LogError($"Failed to init emitter for {path}");
                return null;
            }

            emitter.EventReference = path;
            _eventEmitters.Add(emitter);
            return emitter;
        }

        #endregion
        
        #endregion

        #endregion

        #region fields

        private readonly List<EventInstance> _eventInstances = new();
        
        private readonly List<StudioEventEmitter> _eventEmitters = new();
        
        private EventInstance _musicEventInstance;
        
        private EventInstance _ambientEventInstance;
        
        private Bus _masterBus;
        
        private Bus _musicBus;
        
        private Bus _sfxBus;

        #endregion
    }
}