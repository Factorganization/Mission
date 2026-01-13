using UnityEngine.InputSystem;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
    public sealed class PlayerDataSo : ScriptableObject
    {
        public ControllerState activeStates;
        
        public InputData inputData;
        
        public MoveData moveData;
        
        public InteractData interactData;
        
        public JumpData jumpData;
        
        public CameraData cameraData;
        
        [Header("trucs chiants, touchez pas trop au cas ou")]
        public DevsData devsData;
    }

    [Serializable]
    public class InputData
    {
        public InputActionReference moveInput;
        
        public InputActionReference lookInput;
        
        public InputActionReference jumpInput;
        
        public InputActionReference crouchInput;
        
        public InputActionReference tryPossessInput;

        public InputActionReference tryGrabInput;

        public InputActionReference interactInput;
        
        public InputActionReference throwInput;

        public InputActionReference menuInput;
        
        public InputActionReference missionInput;
    }

    [Serializable]
    public class MoveData
    {
        [Range(0.5f, 2f)]
        public float playerHeight;
        
        public float playerSpeed;

        public float crouchSpeedMultiplier;

        [Range(0, 1)]
        public float crouchDepth;
        
        public float accelDecelMultiplier;

        public float graphRotationSpeed;
    }

    [Serializable]
    public class CameraData
    {
        [Range(0f, 10f)]
        public float gamepadCamSensitivity;
        
        [Range(0f, 10f)]
        public float mouseCamSensitivity;
        
        [Range(-90f, 90f)]
        public float maxUpperPitchAngle;
        
        [Range(-90f, 90f)]
        public float maxLowerPitchAngle;

        public bool freeCam;
    }

    [Serializable]
    public class JumpData
    {
        public float jumpStrength;
        
        public float jumpCoyoteTime;
        
        public float jumpBufferTime; 
    }

    [Serializable]
    public class InteractData
    {
        public Vector2 throwStrength;

        public float bigPossessActionTimer;

        public float throwTimer;
        
        public LayerMask grabbableBlockLayer;
        
        public LayerMask possessedBlockLayer;
    }
    
    [Serializable]
    public class GroudCheckData
    {
        public LayerMask groundLayer;
        
        public float sphereCastRadius;

        public float castBaseLength;

        public float additionalCastLength;
    }
    
    [Serializable]
    public class GravityData
    {
        public float slopeClosingSpeedMultiplier;
        
        public float fallAccelerationMultiplier;

        public float maxFallSpeed;
    }
    
    [Serializable]
    public class DevsData
    {
        public GroudCheckData groundCheckData;
        
        public GravityData gravityData;
    }
}