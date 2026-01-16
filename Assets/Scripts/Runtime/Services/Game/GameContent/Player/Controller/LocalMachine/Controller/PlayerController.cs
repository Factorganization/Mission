using System.Runtime.CompilerServices;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller
{
    internal static class PlayerController
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        /// <summary>
        /// Gather inputs from keyboard or controller buttons
        /// </summary>
        /// <param name="playerModel"></param>
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

        /// <summary>
        /// Gather joystick inputs from controller or mouse
        /// </summary>
        /// <param name="playerModel"></param>
        internal static void HandleRotateInputGather(this  PlayerModel playerModel)
        {
            playerModel.lookDir = playerModel.data.inputData.lookInput.action.ReadValue<Vector2>() / Time.deltaTime;
            playerModel.isUsingMouse = playerModel.data.inputData.lookInput.action.activeControl?.name == "delta";
        }

        /// <summary>
        /// Input dependant output int
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <returns>
        /// <list type="return cases">
        /// <item>1 : try Possess / Unpossess</item>
        /// <item>2 : interact pressed</item>
        /// <item>3 : interact released</item>
        /// <item>4 : interact while grabbing</item>
        /// <item>5 : prep throw item</item>
        /// <item>6 : try Grab / Drop</item>
        /// <item>7 : cancel throw</item>
        /// <item>8 : mission input pressed</item>
        /// <item>9 : mission input released</item>
        /// <item>10 : menu input pressed</item>
        /// </list>
        /// </returns>
        internal static byte HandleMonoInputGather(this PlayerModel playerModel)
        {
            if (playerModel.data.inputData.tryPossessInput.action.WasPressedThisFrame())
                return 1;
            
            if (playerModel.data.inputData.interactInput.action.WasPressedThisFrame())
                return 4;

            if (playerModel.data.inputData.interactInput.action.IsPressed())
                return 2;

            if (playerModel.data.inputData.interactInput.action.WasReleasedThisFrame())
                return 3;

            if (playerModel.data.inputData.tryGrabInput.action.WasPressedThisFrame())
                return 6;

            if (playerModel.data.inputData.throwInput.action.IsPressed())
                return 5;

            if (playerModel.data.inputData.throwInput.action.WasReleasedThisFrame())
                return 7; //not that useful

            if (playerModel.data.inputData.missionInput.action.IsPressed())
                return 8;
            
            if (!playerModel.data.inputData.missionInput.action.IsPressed())
                return 9;
            
            if (playerModel.data.inputData.menuInput.action.WasPressedThisFrame())
                return 10;

            return 0;
        }
        
        /// <summary>
        /// Manage player movement
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="moveMultiplier">linear speed multiplier</param>
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

        /// <summary>
        /// Manage camera movement
        /// </summary>
        /// <param name="playerModel">self</param>
        internal static void Look(this PlayerModel playerModel)
        {
            playerModel.camYaw += playerModel.lookDir.x
                                  * (playerModel.isUsingMouse ? playerModel.data.cameraData.mouseCamSensitivity : playerModel.data.cameraData.gamepadCamSensitivity)
                                  * Time.fixedDeltaTime;
            playerModel.camPitch -= playerModel.lookDir.y
                                    * (playerModel.isUsingMouse ? playerModel.data.cameraData.mouseCamSensitivity : playerModel.data.cameraData.gamepadCamSensitivity)
                                    * Time.fixedDeltaTime;
            playerModel.camPitch = Mathf.Clamp(playerModel.camPitch, playerModel.data.cameraData.maxLowerPitchAngle, playerModel.data.cameraData.maxUpperPitchAngle);

            playerModel.cYsD = Mathf.SmoothDamp(playerModel.cYsD, playerModel.camYaw, ref playerModel.cVcY, playerModel.data.cameraData.smoothCamCoeff);
            playerModel.cPsD = Mathf.SmoothDamp(playerModel.cPsD, playerModel.camPitch, ref playerModel.cVcP, playerModel.data.cameraData.smoothCamCoeff);
            
            playerModel.cam.localEulerAngles = new Vector3(playerModel.data.cameraData.freeCam ? playerModel.cPsD : playerModel.cam.localEulerAngles.x, playerModel.cYsD, 0);
            //playerModel.cam.localEulerAngles += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.localEulerAngles, playerModel.targetLookDir, 0.1f);
        }
        
        /// <summary>
        /// Manage gravity, vertical acceleration of the player
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="goRef">ref of the scene game object of the player</param>
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
                    playerModel.currentHeightTarget + 0.5f + playerModel.castAddLength, // 0.5f pour compenser le sphereCast radius
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
        
        /// <summary>
        /// Check if player is grounded
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="goRef">ref of the scene game object of the player</param>
        /// <returns>True if grounded, False otherwise</returns>
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

        /// <summary>
        /// Set Camera Local Position SMOOTHLY on a specified target position
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="targetPos">target position for camera</param>
        internal static void SetCameraPivotLocalPos(this PlayerModel playerModel, Vector3 targetPos)
        {
            if ((playerModel.cam.localPosition - targetPos).sqrMagnitude < 0.005f)
                return;
            
            playerModel.cam.localPosition += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.localPosition, targetPos, 0.1f);
            if ((playerModel.cam.localPosition - targetPos).sqrMagnitude < 0.005f)
                playerModel.cam.localPosition = targetPos;
        }
        
        /// <summary>
        /// Set Camera Global Position SMOOTHLY on a specified target position
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="targetPos">target position for camera</param>
        internal static void SetCameraPivotGlobalPos(this PlayerModel playerModel, Vector3 targetPos)
        {
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.005f)
                return;
            
            playerModel.cam.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.position, targetPos, 0.1f);
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.005f)
                playerModel.cam.position = targetPos;
        }

        /// <summary>
        /// If player is grabbing an object, Set the local position of the object SMOOTHLY on a specified target position
        /// </summary>
        /// <param name="playerModel">self</param>
        internal static void SetGrabbedObjectLocalPos(this PlayerModel playerModel)
        {
            if (playerModel.currentGrabbedObject is null)
                return;

            if (playerModel.currentGrabbedObject.Active && playerModel.currentGrabbedObject.Transform.parent != playerModel.activeGrab)
                playerModel.currentGrabbedObject.Transform.SetParent(playerModel.activeGrab);
            
            if (!playerModel.currentGrabbedObject.Active && playerModel.currentGrabbedObject.Transform.parent != playerModel.grab)
                playerModel.currentGrabbedObject.Transform.SetParent(playerModel.grab);
            
            if (playerModel.currentGrabbedObject.Transform.localPosition.sqrMagnitude < 0.005f)
                return;
            
            playerModel.currentGrabbedObject.Transform.localPosition += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.currentGrabbedObject.Transform.localPosition, Vector3.zero, 0.1f);
            if (playerModel.currentGrabbedObject.Transform.localPosition.sqrMagnitude < 0.005f)
                playerModel.currentGrabbedObject.Transform.localPosition = Vector3.zero;
        }
        
        /// <summary>
        /// Set Camera World Position SMOOTHLY on a specified target position
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="targetPos">target position for the camera</param>
        internal static void SetCameraPivotPos(this PlayerModel playerModel, Vector3 targetPos)
        {
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.005f)
                return;
            
            playerModel.cam.position += Math.EasingFunction.SimpleQuadraticEase.V3SimpleQuadraticEaseOut(playerModel.cam.position, targetPos, 0.1f);
            if ((playerModel.cam.position - targetPos).sqrMagnitude < 0.005f)
                playerModel.cam.position = targetPos;
        }

        /// <summary>
        /// try throw currently grabbed item
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <returns>Le bool est au cas ou la state machine loop doit cut si le throw echoue</returns>
        internal static bool TryThrowGrabbedObject(this PlayerModel playerModel)
        {
            if (playerModel.currentGrabbedObject is null)
                return false;

            playerModel.currentGrabbedObject.Grabbed = false;
            playerModel.currentGrabbedObject.Rigidbody.isKinematic = false;
            playerModel.currentGrabbedObject.Transform.SetParent(null, true);
            
            playerModel.currentGrabbedObject.Rigidbody.AddForce(
                playerModel.graph.forward.normalized * playerModel.data.interactData.throwStrength.x
                + new Vector3(0, playerModel.data.interactData.throwStrength.y, 0),
                ForceMode.VelocityChange);
            
            if (playerModel.currentGrabbedObject is IElementHolder e)
                e.Active = true; //TODO a corriger apres refonte archi
            playerModel.currentGrabbedObject = null;
            
            return true;
        }

        /// <summary>
        /// When player is grabbing an object, will try to use the object and interact with it on an element source object 
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <returns>True if interaction was performed, False otherwise</returns>
        internal static bool TryInteractGrabbedObject(this PlayerModel playerModel)
        {
            return playerModel.currentGrabbedObject is not null && playerModel.currentGrabbedObject.Action();
        }

        /// <summary>
        /// Set the grabbed object collider and rb to the desired state to get grabbed
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="gb">the grabbable object that was grabbed</param>
        internal static void SetGrabbedObjectState(this PlayerModel playerModel, IGrabbable gb)
        {
            playerModel.currentGrabbedObject = gb;
            playerModel.currentGrabbedObject.Grabbed = true;
            playerModel.currentGrabbedObject.Rigidbody.isKinematic = true;
            playerModel.currentGrabbedObject.Transform.SetParent(playerModel.grab, true);
            if (playerModel.currentGrabbedObject is IElementHolder e)
                e.Active = false; //TODO a corriger apres refonte archi
        }

        /// <summary>
        /// Reset the grabbed object collier and rb states, reset the parent to null
        /// </summary>
        /// <param name="playerModel">self</param>
        internal static void ResetGrabbedObjectState(this PlayerModel playerModel)
        {
            playerModel.currentGrabbedObject.Grabbed = false;
            playerModel.currentGrabbedObject.Rigidbody.isKinematic = false;
            playerModel.currentGrabbedObject.Transform.SetParent(null, true);
            if (playerModel.currentGrabbedObject is IElementHolder e)
                e.Active = true; //TODO a corriger apres refonte archi
            playerModel.currentGrabbedObject = null;
        }

        /// <summary>
        /// Allow the player to end the level in addition to an interaction input
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="canEndLevel">true if player can End Level</param>
        public static void SetEndLevel(this PlayerModel playerModel, bool canEndLevel)
        {
            playerModel.canEndLevel = canEndLevel;
        }

        /// <summary>
        /// Set the weight of an animator layer
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="layerID">id of the layer</param>
        /// <param name="weight">desired weight of the layer</param>
        internal static void SetAnimWeight(this PlayerModel playerModel, int layerID, float weight)
        {
            playerModel.animator.SetLayerWeight(layerID, weight);
        }
        
        /// <summary>
        /// Set the value of an animator parameter
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="param">parameter id of the animator</param>
        internal static void SetAnimParam(this PlayerModel playerModel, int param)
        {
            playerModel.animator.SetTrigger(param);
        }

        /// <summary>
        /// Set the value of an animator parameter
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="param">parameter id of the animator</param>
        /// <param name="value">desired param value</param>
        internal static void SetAnimParam(this PlayerModel playerModel, int param, bool value)
        {
            playerModel.animator.SetBool(param, value);
        }

        /// <summary>
        /// Set the value of an animator parameter
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="param">parameter id of the animator</param>
        /// <param name="value">desired param value</param>
        internal static void SetAnimParam(this PlayerModel playerModel, int param, float value)
        {
            playerModel.animator.SetFloat(param, value);
        }

        /// <summary>
        /// Set the value of an animator parameter
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="param">parameter id of the animator</param>
        /// <param name="value">desired param value</param>
        internal static void SetAnimParam(this PlayerModel playerModel, int param, int value)
        {
            playerModel.animator.SetInteger(param, value);
        }
    }
}