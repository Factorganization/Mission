using UnityEngine;
using UnityEngine.InputSystem;

namespace GameContent.Player.Controller.LocalMachine.Model
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
    public sealed class PlayerDataSo : ScriptableObject
    {
        public InputData inputData;
        
        public MoveData moveData;
        
        public CameraData cameraData;
        
        [Header("trucs chiants, touchez pas trop au cas ou")]
        public DevsData devsData;
    }

    [System.Serializable]
    public class InputData
    {
        public InputActionReference moveInput;
        
        public InputActionReference lookInput;
        
        public InputActionReference wheelInput;

        public InputActionReference interactInput;

        public InputActionReference menuInput;
    }

    [System.Serializable]
    public class MoveData
    {
        [Range(0.5f, 2f)]
        public float playerHeight;
        
        public float playerSpeed;
        
        public float accelDecelMultiplier;

        public float graphRotationSpeed;
    }

    [System.Serializable]
    public class CameraData
    {
        [Range(0f, 1f)]
        public float camSensitivity;
        
        [Range(0f, 90f)]
        public float maxPitchAngle;
    }
    
    [System.Serializable]
    public class GroudCheckData
    {
        public LayerMask groundLayer;
        
        public float sphereCastRadius;

        public float castBaseLength;

        public float additionalCastLength;
    }
    
    [System.Serializable]
    public class GravityData
    {
        public float slopeClosingSpeedMultiplier;
        
        public float fallAccelerationMultiplier;

        public float maxFallSpeed;
    }
    
    [System.Serializable]
    public class DevsData
    {
        public GroudCheckData groundCheckData;
        
        public GravityData gravityData;
    }
}