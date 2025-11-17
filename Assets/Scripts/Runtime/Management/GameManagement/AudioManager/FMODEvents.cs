using FMODUnity;
using UnityEngine;

namespace Runtime.GameContent.AudioManager
{
    public class FMODEvents : MonoBehaviour
    {
        [field : Header("Ambience Music")]
        [field: SerializeField] public EventReference MasterVolume { get; private set; }
    
        public static FMODEvents Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of FMODEvents detected. Destroying the new instance.");
                return;
            }
        
            Instance = this;
        }
    } 
}
