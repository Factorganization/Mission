using UnityEngine;

namespace Runtime.Management.GameManagement
{
    public class ElementManager : MonoBehaviour
    {
        #region properties

        public static ElementManager Element { get; private set; }

        #endregion
        
        #region methodes

        private void Awake()
        {
            if (Element is not null)
                Debug.LogWarning("LevelGenerator already instantiated");
            
            Element = this;
        }

        private void FixedUpdate()
        {
            var c = LevelGenerator.Generator.ElementHolders.Count;

            _delay += Time.fixedDeltaTime;
            
            if (_delay < 0.25f)
                return;

            _delay = 0;
            
            for (var i = 0; i < c - 1; i++)
            {
                for (var j = i + 1; j < c; j++)
                {
                    if (Vector3.Distance(LevelGenerator.Generator.ElementHolders[i].Transform.position, LevelGenerator.Generator.ElementHolders[j].Transform.position) > 1.5f)
                        continue;

					if (!LevelGenerator.Generator.ElementHolders[i].Active || !LevelGenerator.Generator.ElementHolders[j].Active)
						continue;
                    
                    LevelGenerator.Generator.ElementHolders[i].CheckOtherElement(LevelGenerator.Generator.ElementHolders[j]);
                }
            }
        }

        #endregion

        #region fields

        private float _delay;

        #endregion
    }
}