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

        internal static bool OnAction(this PlayerModel playerModel)
        {
            return playerModel.currentPossessedObject.Action();
        }

        internal static bool OnPossess(this PlayerModel playerModel)
        {
            var min = 100f;
            IPossessable tp = null;
            
            foreach (var p in LevelGenerator.Generator.Possessables)
            {
                var d = Vector3.Distance(p.Transform.position, playerModel.rb.position);

                if (d >= GameConstants.MaxPossessDistance || d > min || Vector3.Angle(playerModel.graph.forward,
                        ((p.Transform.position - playerModel.rb.position) * GameConstants.VectorUpFilter).normalized) > GameConstants.MaxInteractionAngle)
                    continue;
                
                min = d;
                tp = p;
            }
            
            if (min > 2)
                return false;
            
            playerModel.currentPossessedObject = tp;
            return true;
        }
    }
}