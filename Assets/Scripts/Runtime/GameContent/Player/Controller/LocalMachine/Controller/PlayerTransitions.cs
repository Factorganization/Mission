using Runtime.GameContent.Actors.ActorInterfaces;
using Runtime.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Management.GameManagement;
using UnityEngine;

namespace Runtime.GameContent.Player.Controller.LocalMachine.Controller
{
    internal static class PlayerTransitions
    {
        internal static bool OnIdle(this PlayerModel playerModel)
        {
            return playerModel.inputDir.sqrMagnitude < 0.1f;
        }

        internal static bool OnMove(this PlayerModel playerModel)
        {
            return playerModel.inputDir.sqrMagnitude >= 0.1f;
        }
        
        internal static bool OnJump(this PlayerModel playerModel)
        {
            return playerModel.coyoteTime > 0 && playerModel.jumpBufferTime > 0;
        }

        internal static bool OnFall(this PlayerModel playerModel)
        {
            return playerModel.rb.linearVelocity.y < 0;
        }

        internal static void OnAction(this PlayerModel playerModel)
        {
            playerModel.currentPossessedObject.Action();
        }

        internal static void OnDestructiveAction(this PlayerModel playerModel)
        {
            playerModel.currentPossessedObject.DestructiveAction();
        }

        /// <summary>
        /// Object dependant output int
        /// </summary>
        /// <param name="playerModel"></param>
        /// <returns>
        /// <list type="return case">
        /// <item>0 : nothing happened</item>
        /// <item>1 : object possessed</item>
        /// <item>2 : object grabbed</item>
        /// </list>
        /// </returns>
        internal static int OnTryPossessGrab(this PlayerModel playerModel)
        {
            var minDist = 100f;
            var minAngle = 45f;
            IPossessable tp = null;
            IGrabbable gb = null;
            
            foreach (var p in LevelGenerator.Generator.Possessables)
            {
                var d = Vector3.Distance(p.Transform.position, playerModel.rb.position);
                var a = Vector3.Angle(playerModel.graph.forward, ((p.Transform.position - playerModel.rb.position) * GameConstants.VectorUpFilter).normalized);

                if (d >= GameConstants.MaxPossessDistance || a > GameConstants.MaxInteractionAngle || d > minDist || a > minAngle)
                    continue;
                
                minDist = d;
                minAngle = a;
                tp = p;
            }

            foreach (var g in LevelGenerator.Generator.Grabbables)
            {
                var d = Vector3.Distance(g.Transform.position, playerModel.rb.position);
                var a = Vector3.Angle(playerModel.graph.forward, ((g.Transform.position - playerModel.rb.position) * GameConstants.VectorUpFilter).normalized);
                
                if (d >= GameConstants.MaxPossessDistance || a > GameConstants.MaxInteractionAngle || d > minDist || a > minAngle)
                    continue;
                
                tp = null;
                minDist = d;
                minAngle = a;
                gb = g;
            }

            if (tp is not null)
            {
                playerModel.currentPossessedObject = tp;
                return 1;
            }

            if (gb is not null)
            {
                if (playerModel.currentGrabbedObject is not null)
                {
                    playerModel.currentGrabbedObject.Transform.SetParent(null, true);
                    playerModel.currentGrabbedObject.Rigidbody.isKinematic = false;
                }
                
                playerModel.currentGrabbedObject = gb;
                return 2;
            }

            return 0;
        }
    }
}