using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Management.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync(scene1, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(scene2, LoadSceneMode.Additive);
        }

        #region fields

        [SerializeField] private string scene1;
        
        [SerializeField] private string scene2;

        #endregion
    }
}