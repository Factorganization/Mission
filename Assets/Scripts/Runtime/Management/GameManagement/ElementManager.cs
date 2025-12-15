using Runtime.GameContent.Logics.LogicInterfaces;

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
                    var ei = LevelGenerator.Generator.ElementHolders[i];
                    var ej = LevelGenerator.Generator.ElementHolders[j];
                    
                    if (Vector3.Distance(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center) > ei.ElementApplicationDistance + ej.ElementApplicationDistance)
                        continue;
                    
                    Physics.Linecast(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center, out var hit, blockLayer);
                    if (hit.transform is not null && !hit.transform.TryGetComponent<IElementHolder>(out _))
                        continue;

                    if (!ei.Active || !ej.Active)
                        continue;
                    
                    ei.CheckOtherElement(ej);
                }
            }
        }

        #endregion

        #region fields

        [SerializeField] private LayerMask blockLayer;
        
        private float _delay;

        #endregion
    }
}