using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Shared.RapaEngineUtils.Maths;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller
{
    internal static class PlayerController
    {
        private static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        internal static void HandleContinuousInputGather(this PlayerModel playerModel)
        {
            playerModel.inputDir = playerModel.data.inputData.moveInput.action.ReadValue<Vector2>();
             
            playerModel.jumpBufferTime -= Time.deltaTime;
            
            if (playerModel.data.inputData.jumpInput.action.WasPressedThisFrame())
                playerModel.jumpBufferTime = playerModel.data.jumpData.jumpBufferTime;
            
            if (playerModel.data.inputData.crouchInput.action.IsPressed() && playerModel.currentHeightTarget >= playerModel.data.moveData.playerHeight - playerModel.data.moveData.crouchDepth - 1)
            {
                playerModel.isCrouching = true;
                playerModel.currentHeightTarget = playerModel.data.moveData.playerHeight - playerModel.data.moveData.crouchDepth - 1;
                playerModel.currentMoveMultiplier = playerModel.data.moveData.crouchSpeedMultiplier;
            }
            else if (!playerModel.data.inputData.crouchInput.action.IsPressed() && playerModel.currentHeightTarget <= playerModel.data.moveData.playerHeight)
            {
                playerModel.isCrouching = false;
                playerModel.currentHeightTarget = playerModel.data.moveData.playerHeight - 1;
                playerModel.currentMoveMultiplier = 1;
            }
        }

        internal static void HandleRotateInputGather(this  PlayerModel playerModel)
        {
            playerModel.lookDir = playerModel.data.inputData.lookInput.action.ReadValue<Vector2>() / Time.deltaTime;
        }

        /// <summary>
        /// Input dependant output int
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <returns>
        /// <list type="return cases">
        /// <item>1 : try Grab or Possess / try Drop or Unpossess</item>
        /// <item>2 : possess action pressed</item>
        /// <item>3 : possess action released</item>
        /// <item>4 : interact while grabbing</item>
        /// <item>5 : throw item</item>
        /// </list>
        /// </returns>
        internal static byte HandleMonoInputGather(this PlayerModel playerModel)
        {
            if (playerModel.data.inputData.grabPossessInput.action.WasPressedThisFrame())
                return 1;

            if (playerModel.data.inputData.possessInteractInput.action.IsPressed())
                return 2;

            if (playerModel.data.inputData.possessInteractInput.action.WasReleasedThisFrame())
                return 3;

            if (playerModel.data.inputData.grabInteractInput.action.WasPressedThisFrame())
                return 4;

            if (playerModel.data.inputData.throwInput.action.WasPressedThisFrame())
                return 5;

            return 0;
        }
        
        internal static void Move(this PlayerModel playerModel, float moveMultiplier)
        {
            var tempForward = Vector3.ProjectOnPlane(playerModel.cam.forward, Vector3.up).normalized;
            var tempRight = Vector3.ProjectOnPlane(playerModel.cam.right, Vector3.up).normalized;
            var tempDir = tempForward * playerModel.inputDir.y + tempRight * playerModel.inputDir.x;
            //var tempDir = (tempForward * playerModel.inputDir.y + tempRight * playerModel.inputDir.x).normalized; //si vitesse constante sans analogic
            
            playerModel.acceleration = tempDir * (playerModel.data.moveData.playerSpeed * moveMultiplier * GameConstants.ConstFixedDeltaTime);

            playerModel.tempLinearVelocity = playerModel.rb.linearVelocity;
            playerModel.tempLinearVelocity.y = 0;

            playerModel.targetDir = playerModel.acceleration - playerModel.tempLinearVelocity;
            
            //Impropre ici
            if (playerModel.inputDir.sqrMagnitude > 0.1f)
                playerModel.lastLookDir = tempDir.normalized;

            playerModel.rb.AddForce(playerModel.targetDir * playerModel.data.moveData.accelDecelMultiplier, ForceMode.Acceleration);
        }

        internal static void Look(this PlayerModel playerModel)
        {
            playerModel.camYaw += playerModel.lookDir.x * playerModel.data.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerModel.camPitch -= playerModel.lookDir.y * playerModel.data.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerModel.camPitch = ClampSymmetric(playerModel.camPitch, playerModel.data.cameraData.maxPitchAngle);
            
            playerModel.cam.Rotate(new Vector3(-playerModel.lookDir.y * playerModel.data.cameraData.camSensitivity, 0, 0));
            playerModel.cam.localRotation = Quaternion.Euler(playerModel.camPitch, playerModel.camYaw, 0);
        }
        
        internal static void HandleGravity(this PlayerModel playerModel, GameObject goRef)
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
        
        internal static bool CheckGround(this PlayerModel playerModel, GameObject goRef)
        {
            var sphereGroundCheck = Physics.SphereCast(goRef.transform.position,
                playerModel.data.devsData.groundCheckData.sphereCastRadius,
                Vector3.down,
                out _,
                playerModel.currentHeightTarget + playerModel.castAddLength,
                playerModel.data.devsData.groundCheckData.groundLayer);
            
            return sphereGroundCheck;
        }

        internal static void SetCameraPivotLocalPos(this PlayerModel playerModel, Vector3 targetPos)
        {
            if ((playerModel.cam.localPosition - targetPos).sqrMagnitude < 0.005f)
                return;
            
            playerModel.cam.localPosition += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.localPosition, targetPos, 0.1f);
            if ((playerModel.cam.localPosition - targetPos).sqrMagnitude < 0.01f)
                playerModel.cam.localPosition = targetPos;
        }

        internal static void SetGrabbedObjectLocalPos(this PlayerModel playerModel)
        {
            if (playerModel.currentGrabbedObject is null)
                return;
            
            if ((playerModel.currentGrabbedObject.Transform.localPosition - playerModel.grab.position).sqrMagnitude < 0.005f)
                return;
            
            playerModel.currentGrabbedObject.Rigidbody.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.currentGrabbedObject.Rigidbody.position, playerModel.grab.position, 0.1f);
            if (playerModel.currentGrabbedObject.Transform.localPosition.sqrMagnitude < 0.01f)
                playerModel.currentGrabbedObject.Rigidbody.position = playerModel.grab.position;
        }
        
        internal static void SetCameraPivotPos(this PlayerModel playerModel, Vector3 targetPos)
        {
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.005f)
                return;
            
            playerModel.cam.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.position, targetPos, 0.1f);
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.01f)
                playerModel.cam.position = targetPos;
        }
    }
}