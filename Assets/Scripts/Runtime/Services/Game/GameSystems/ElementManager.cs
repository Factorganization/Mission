using System.Threading.Tasks;
using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;

namespace Runtime.Services.Game.GameSystems
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

        private /*async*/ void FixedUpdate()
        {
            var c = LevelGenerator.Generator.ElementHolders.Count;

            _delay += Time.fixedDeltaTime;
            
            if (_delay < 1)
                return;

            _delay = 0;
            //await Task.Delay(900);
            for (var i = 0; i < c; i++)
            {
                for (var j = 0; j < c; j++) // Tant pis on fait une loop en plus //TODO tester le async
                {
                    var ei = LevelGenerator.Generator.ElementHolders[i];
                    var ej = LevelGenerator.Generator.ElementHolders[j];
                    
                    if (Vector3.Distance(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center) > ei.ElementApplicationDistance + ej.ElementApplicationDistance)
                        continue;
                    
                    Physics.Linecast(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center, out var hit, blockLayer);
                    if (hit.transform is not null && !hit.transform.root.TryGetComponent<IElementHolder>(out _))
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