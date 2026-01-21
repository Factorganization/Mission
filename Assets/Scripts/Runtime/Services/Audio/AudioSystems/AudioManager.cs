using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

namespace Runtime.Services.Audio.AudioSystems
{
    public class AudioManager : MonoBehaviour
    {
        #region Functions
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of AudioManager detected. Destroying the new instance.");
            }
        
            Instance = this;
        
            eventInstances = new List<EventInstance>();
            eventEmitters = new List<StudioEventEmitter>();
        
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            SFXBus = RuntimeManager.GetBus("bus:/SFX");
        }
    
        private void Start()
        {
            // Initialize ambience and music here with appropriate event references
            // Example:
            //InitializeAmbience(FMODEvents.Instance.MasterVolume);
            InitializeMusic(FMODEvents.Instance.MusicVolume);
        }

        private void Update()
        {
            masterBus.setVolume(masterVolume);
            musicBus.setVolume(musicVolume);
            SFXBus.setVolume(SFXVolume);
        }
    
        private void InitializeAmbience(EventReference eventPath)
        {
            ambienceEventInstance = CreateInstance(eventPath);
            ambienceEventInstance.start();
        }

        private void InitializeMusic(EventReference eventPath)
        {
            musicEventInstance = CreateInstance(eventPath);
            musicEventInstance.start();
        }

        /*public void SetMusicArea(MusicArea musicArea)
        {
            musicEventInstance.setParameterByName("area", (float)musicArea);
        }*/

        public void SetAmbienceParameter(string parameterName, float parameterValue)
        {
            ambienceEventInstance.setParameterByName(parameterName, parameterValue);
        }
    
        public void PlayOneShot(EventReference eventPath, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(eventPath, worldPosition);
        }
    
        private EventInstance CreateInstance(EventReference eventPath)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
            if (eventInstance.isValid())
            {
                eventInstances.Add(eventInstance);
            }
            else
            {
                Debug.LogError($"Failed to create FMOD event instance for {eventPath}");
            }
            return eventInstance;
        }

        private StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject gameObject)
        {
            StudioEventEmitter eventEmitter = gameObject.GetComponent<StudioEventEmitter>();
            eventEmitter.EventReference = eventReference;
            eventEmitters.Add(eventEmitter);
            return eventEmitter;
        }
    
        private void CleanupInstances()
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
            }

            foreach (StudioEventEmitter eventEmitter in eventEmitters)
            {
                eventEmitter.Stop();
            }
        }
    
        private void OnDestroy()
        {
            CleanupInstances();
        }
        
        #endregion

        #region Fields

        [Header("Volume")]
        [Range(0f, 1f)]
        public float masterVolume = 1;
        [Range(0f, 1f)]
        public float musicVolume = 1;
        [Range(0f, 1f)]
        public float SFXVolume = 1;
    
        private Bus masterBus;
        private Bus musicBus;
        private Bus SFXBus;
    
        private List<EventInstance> eventInstances;
        private List<StudioEventEmitter> eventEmitters;
    
        private EventInstance ambienceEventInstance;
        private EventInstance musicEventInstance;
    
        public static AudioManager Instance { get; private set; }

        #endregion
    }
}