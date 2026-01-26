using Runtime.Services.Game.GameContent.Logics.LogicInterfaces;
using Runtime.Services.Game.GameContent.Player.Controller.LocalMachine.View;

namespace Runtime.Services.Game.GameSystems
{
    public class ElementManager : MonoBehaviour
    {
        #region properties

        public static ElementManager Element { get; private set; }
        
        public static int CurrentCombo { get; set; }
        
        public int TempMalice { get; set; }

        #endregion
        
        #region methodes

        private void Awake()
        {
            if (Element is not null)
                Debug.LogWarning("LevelGenerator already instantiated");
            
            Element = this;
        }

        private void Start()
        {
            CurrentCombo = 0;
        }

        private void FixedUpdate()
        {
            var c = LevelGenerator.Generator.ElementHolders.Count;

            _delay += Time.fixedDeltaTime;
            
            /*if (_delay < 0.15f)
                return;*/

            _delay = 0;
            
            for (var i = _lowerThreshold; i < _upperThreshold; i++)
            {
                for (var j = 0; j < c; j++) 
                {
                    if (i == j)
                        continue;

                    var ei = LevelGenerator.Generator.ElementHolders[i];
                    var ej = LevelGenerator.Generator.ElementHolders[j];

                    if (ei.Transform is null || ej.Transform is null)
                        continue;

                    if (Vector3.Distance(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center) > ei.ElementApplicationDistance + ej.ElementApplicationDistance)
                        continue;
                    
                    Physics.Linecast(ei.Transform.position + ei.Collider.center, ej.Transform.position + ej.Collider.center, out var hit, blockLayer);
                    
                    if (hit.transform is null)
                        continue;

                    if (!hit.transform.root.TryGetComponent<IElementHolder>(out var h))
                    {
                        if (!TryGetComponent<PlayerStateMachine>(out var p))
                            continue;
                        
                        var pcg = p.PlayerModel.currentGrabbedObject;
                        
                        if (pcg is null)
                            continue;

                        h = pcg as IElementHolder;
                    }

                    if (h.Id != ej.Id)
                        continue;

                    if (!ei.Active || !ej.Active)
                        continue;
                    
                    ei.CheckOtherElement(ej);
                }
            }

            _upperThreshold += _threshold;
            if (_upperThreshold > _total)
                _upperThreshold = _total;
            
            _lowerThreshold += _threshold;
            if (_lowerThreshold > _total)
            {
                _lowerThreshold = 0;
                _upperThreshold = _threshold;

                var combo = CurrentCombo % 5;
                TempMalice += combo * 50;
                CurrentCombo = 0;
            }
        }

        public void SetThreshold(int total)
        {
            _total = total;
            _threshold = (int)Mathf.Floor(total / 10f);

            _lowerThreshold = 0;
            _upperThreshold = _threshold;
        }

        #endregion

        #region fields
        
        [SerializeField] private LayerMask blockLayer;
        
        private float _delay;

        private int _threshold;

        private int _total;
        
        private int _lowerThreshold;
        
        private int _upperThreshold;

        #endregion
    }
}