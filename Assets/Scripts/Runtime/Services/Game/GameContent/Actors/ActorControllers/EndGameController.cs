using Runtime.Services.Game.GameContent.Actors.ActorModels;
using UnityEngine;

namespace Runtime.Services.Game.GameContent.Actors.ActorControllers
{
    public static class EndGameController
    {
        #region methodes

        public static void SetActive(EndGameModel model, bool state)
        {
            model._active = state;
        }

        #endregion
    }
}


