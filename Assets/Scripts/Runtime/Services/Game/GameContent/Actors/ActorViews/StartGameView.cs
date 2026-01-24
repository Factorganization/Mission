using System.Collections;
using Runtime.Utils.Coroutines;

namespace Runtime.Services.Game.GameContent.Actors.ActorViews
{
    public class StartGameView : ActorView
    {
        #region methodes

        private void Start()
        {
            StartCoroutine(OnPartStart());
        }

        private IEnumerator OnPartStart()
        {
            yield return new WaitForSceneLoaded();

            startParticles.Play();
            yield return new WaitForSeconds(6f);
            startParticles.Stop();
        }

        #endregion

        #region fields

        [SerializeField] private ParticleSystem startParticles;

        #endregion
    }
}