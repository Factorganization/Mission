using UnityEngine.SceneManagement;

namespace Runtime.Management.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        #region methodes
        
        private void Start()
        {
            foreach (var s in scenes)
                SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
        }
        
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        
        #endregion

        #region fields

        [SerializeField] private string[] scenes;

        #endregion
    }
}