using System.Collections;
using GameContent.Player.Controller.BaseMachine;
using GameContent.Player.Controller.LocalMachine.Model;
using GameContent.Player.Controller.LocalMachine.View;
using UnityEngine;

namespace GameContent.Player.Controller.LocalMachine.Controller
{
    public class BasePlayerState : BaseState
    {
        #region properties

        public ControllerState StateFlag { get; }

        #endregion
        
        #region

        protected BasePlayerState(GenericStateMachine machine, GameObject go, PlayerModel model, ControllerState state) : base(machine, go)
        {
            playerModel = model;
            StateFlag = state;
        }
        
        #endregion

        #region methodes
        
        public override void OnInit(GenericStateMachine machine)
        {
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }
        
        protected static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        protected void HandleInputGather()
        {
            playerModel.inputDir = playerModel.data.inputData.moveInput.action.ReadValue<Vector2>();
            
            playerModel.currentHeightTarget = playerModel.data.devsData.groundCheckData.castBaseLength;
        }

        protected void HandleRotateInputGather()
        {
            playerModel.lookDir = playerModel.data.inputData.lookInput.action.ReadValue<Vector2>() / Time.deltaTime;
        }
        
        protected void Move(float moveMultiplier)
        {
            var tempForward = Vector3.ProjectOnPlane(playerModel.cam.forward, Vector3.up).normalized;
            var tempRight = Vector3.ProjectOnPlane(playerModel.cam.right, Vector3.up).normalized;
            
            //Impropre ici
            if (playerModel.inputDir.sqrMagnitude > 0.1f)
                playerModel.lastLookDir = tempForward;
            
            playerModel.acceleration = (tempForward * playerModel.inputDir.y + tempRight * playerModel.inputDir.x) * (playerModel.data.moveData.playerSpeed * moveMultiplier * GameConstants.ConstFixedDeltaTime);

            playerModel.tempLinearVelocity = playerModel.rb.linearVelocity;
            playerModel.tempLinearVelocity.y = 0;

            playerModel.targetDir = playerModel.acceleration - playerModel.tempLinearVelocity;

            playerModel.rb.AddForce(playerModel.targetDir * playerModel.data.moveData.accelDecelMultiplier, ForceMode.Acceleration);
        }

        protected void Look()
        {
            playerModel.camYaw += playerModel.lookDir.x * playerModel.data.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerModel.camPitch -= playerModel.lookDir.y * playerModel.data.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerModel.camPitch = ClampSymmetric(playerModel.camPitch, playerModel.data.cameraData.maxPitchAngle);
            
            playerModel.cam.Rotate(new Vector3(-playerModel.lookDir.y * playerModel.data.cameraData.camSensitivity, 0, 0));
            playerModel.cam.localRotation = Quaternion.Euler(playerModel.camPitch, playerModel.camYaw, 0);
        }
        
        protected void HandleGravity()
        {
            var sphereGroundCheck = Physics.SphereCast(goRef.transform.position,
                playerModel.data.devsData.groundCheckData.sphereCastRadius,
                Vector3.down,
                out var hit1,
                playerModel.currentHeightTarget + playerModel.castAddLength,
                playerModel.data.devsData.groundCheckData.groundLayer);
            
            if (!sphereGroundCheck)
            {
                playerModel.castAddLength = 0;
                playerModel.vertVelocity -= Time.deltaTime * playerModel.data.devsData.gravityData.fallAccelerationMultiplier;
                
                playerModel.vertVelocity = ClampSymmetric(playerModel.vertVelocity, playerModel.data.devsData.gravityData.maxFallSpeed * Time.fixedDeltaTime);
                playerModel.rb.linearVelocity += Vector3.up * playerModel.vertVelocity;
            }

            else
            {
                playerModel.vertVelocity = 0;
                playerModel.castAddLength = playerModel.data.devsData.groundCheckData.additionalCastLength;

                var pointGroundCheck = Physics.Raycast(goRef.transform.position,
                    Vector3.down,
                    out var hit2,
                    playerModel.currentHeightTarget + 0.5f + playerModel.castAddLength, // 0.5f pour compenser le sphereCast radius en Rey
                    playerModel.data.devsData.groundCheckData.groundLayer);
                
                if (pointGroundCheck)
                {
                    var d = playerModel.currentHeightTarget + 0.5f - Mathf.Abs(goRef.transform.position.y - hit2.point.y);
                    playerModel.rb.linearVelocity = new Vector3(playerModel.rb.linearVelocity.x, d * playerModel.data.devsData.gravityData.slopeClosingSpeedMultiplier, playerModel.rb.linearVelocity.z);
                    return;
                }
                
                var d2 = playerModel.currentHeightTarget + 0.5f - Mathf.Abs(goRef.transform.position.y - hit1.point.y);
                playerModel.rb.linearVelocity = new Vector3(playerModel.rb.linearVelocity.x, d2 * playerModel.data.devsData.gravityData.slopeClosingSpeedMultiplier, playerModel.rb.linearVelocity.z);
            }
        }
        
        protected bool CheckGround()
        {
            var sphereGroundCheck = Physics.SphereCast(goRef.transform.position,
                playerModel.data.devsData.groundCheckData.sphereCastRadius,
                Vector3.down,
                out _,
                playerModel.currentHeightTarget + playerModel.castAddLength,
                playerModel.data.devsData.groundCheckData.groundLayer);
            
            return sphereGroundCheck;
        }
        
        #endregion

        #region fields
        
        protected readonly PlayerModel playerModel;

        #endregion
    }
}