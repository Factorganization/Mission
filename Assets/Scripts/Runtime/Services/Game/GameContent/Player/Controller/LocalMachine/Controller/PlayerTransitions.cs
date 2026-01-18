using System.Runtime.CompilerServices;
using Runtime.Services.Game.GameContent.Actors.ActorInterfaces;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Model;
using Runtime.Services.Game.GameSystems;

namespace Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.Controller
{
    internal static class PlayerTransitions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool OnIdle(this PlayerModel playerModel)
        {
            return playerModel.inputDir.sqrMagnitude < 0.1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool OnMove(this PlayerModel playerModel)
        {
            return playerModel.inputDir.sqrMagnitude >= 0.1f;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool OnJump(this PlayerModel playerModel)
        {
            return playerModel.coyoteTime > 0 && playerModel.jumpBufferTime > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// <param name="playerModel">self</param>
        /// <returns>
        /// <list type="return case">
        /// <item>0 : nothing happened</item>
        /// <item>1 : object possessed</item>
        /// </list>
        /// </returns>
        internal static sbyte OnTryPossess(this PlayerModel playerModel)
        {
            IPossessable tp = null;

            if (LevelGenerator.Generator is null)
                return 0;
            
            var minDist = 100f;
            var minAngle = 45f;
            
            foreach (var p in LevelGenerator.Generator.Possessables)
            {
                if (p.Destroyed)
                    continue;
                
                var d = Vector3.Distance(p.Transform.position + p.Collider.center, playerModel.rb.position);
                var a = Vector3.Angle(playerModel.graph.forward, ((p.Transform.position + p.Collider.center - playerModel.rb.position) * GameConstants.VectorUpFilter).normalized);
                Physics.Raycast(playerModel.rb.position, 
                    (p.Transform.position + p.Collider.center - playerModel.rb.position).normalized,
                    out var hit,
                    playerModel.data.interactData.possessDistance,
                    playerModel.data.interactData.possessedBlockLayer);
                
                if (d >= playerModel.data.interactData.possessDistance || a > GameConstants.MaxInteractionAngle || d > minDist || a > minAngle || !hit.transform.root.TryGetComponent<IPossessable>(out _))
                    continue;
                
                minDist = d;
                minAngle = a;
                tp = p;
            }

            if (tp is null)
                return 0;
            
            playerModel.currentPossessedObject = tp;
            return 1;
        }

        /// <summary>
        /// Object dependant output int
        /// </summary>
        /// <param name="playerModel">self</param>
        /// <param name="gb">the grabbable object possibly grabbed</param>
        /// <returns>
        /// <list type="return case">
        /// <item>0 : nothing happened</item>
        /// <item>1 : object grabbed</item>
        /// </list>
        /// </returns>
        internal static sbyte OnTryGrab(this PlayerModel playerModel, out IGrabbable gb)
        {
            gb = null;
            
            if (LevelGenerator.Generator is null)
                return 0;
            
            var minDist = 100f;
            var minAngle = 45f;

            foreach (var g in LevelGenerator.Generator.Grabbables)
            {
                if (playerModel.currentGrabbedObject is not null && playerModel.currentGrabbedObject == g)
                    continue;
                
                var d = Vector3.Distance(g.Transform.position, playerModel.rb.position);
                var a = Vector3.Angle(playerModel.graph.forward, ((g.Transform.position - playerModel.rb.position) * GameConstants.VectorUpFilter).normalized);
                Physics.Raycast(playerModel.rb.position, 
                    (g.Transform.position - playerModel.rb.position).normalized,
                    out var hit,
                    playerModel.data.interactData.grabDistance,
                    playerModel.data.interactData.grabbableBlockLayer);
                
                if (d <= playerModel.data.interactData.securityMinGrabDistance)
                {
                    minDist = d;
                    gb = g;
                    continue;
                }
                
                if (d >= playerModel.data.interactData.grabDistance || a > GameConstants.MaxInteractionAngle || d > minDist || a > minAngle || !hit.transform.root.TryGetComponent<IGrabbable>(out _))
                    continue;
                
                minDist = d;
                minAngle = a;
                gb = g;
            }

            return gb is null ? (sbyte)0 : (sbyte)1;
        }
    }
}