namespace Runtime.GameContent.Logics.LogicModels.ElementModels
{
    [Serializable]
    public struct ElementDuration
    {
        public float fireDuration;
        
        public float electricityDuration;

        public float waterDuration;

        [HideInInspector] public float fireTimer;
        
        [HideInInspector] public float electricityTimer;
        
        [HideInInspector] public float waterTimer;
    }
}