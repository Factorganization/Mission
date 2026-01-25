using Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;

namespace Runtime.Services.Game.GameContent.Logics.LogicModels.MissionModels
{
    [Serializable]
    public struct MissionModel : IEquatable<MissionModel>
    {
        #region constructors
        
        public MissionModel(MissionType mission, ObjectType objectType, ElementFlag toApply, RoomType room, int number)
        {
            this.mission = mission;
            this.objectType = objectType;
            this.toApply = toApply;
            this.room = room;
            this.number = number;
        }
        
        public MissionModel(MissionType mission, ObjectType objectType, ElementFlag toApply, RoomType room)
        {
            this.mission = mission;
            this.objectType = objectType;
            this.toApply = toApply;
            this.room = room;
            number = 0;
        }

        #endregion
        
        #region methodes

        public static bool operator ==(MissionModel a, MissionModel b)
        {
            return 
                a.mission == b.mission &&
                (a.objectType == b.objectType ||
                 (a.objectType is ObjectType.Affectable && Enum.GetName(typeof(ObjectType), b.objectType)!.StartsWith('A')) ||
                 (a.objectType is ObjectType.Item && Enum.GetName(typeof(ObjectType), b.objectType)!.StartsWith('I')) ||
                 (a.objectType is ObjectType.Possessable && Enum.GetName(typeof(ObjectType), b.objectType)!.StartsWith('P')) ||
                 (Enum.GetName(typeof(ObjectType), a.objectType)!.StartsWith('A') && b.objectType is ObjectType.Affectable) ||
                 (Enum.GetName(typeof(ObjectType), a.objectType)!.StartsWith('I') && b.objectType is ObjectType.Item) ||
                 (Enum.GetName(typeof(ObjectType), a.objectType)!.StartsWith('P') && b.objectType is ObjectType.Possessable) ||
                 a.objectType is ObjectType.All || b.objectType is ObjectType.All) &&
                (a.room == b.room || a.room is RoomType.House || b.room is RoomType.House) &&
                ((int)a.toApply & (int)b.toApply) != 0;
        }

        public static bool operator !=(MissionModel a, MissionModel b) => !(a == b);

        public bool Equals(MissionModel other)
        {
            return mission == other.mission && objectType == other.objectType && toApply == other.toApply && room == other.room;
        }

        public override bool Equals(object obj)
        {
            return obj is MissionModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)mission, (int)objectType, (int)toApply, (int)room, number);
        }

        #endregion
        
        #region fields
        
        public MissionType mission;
        
        [Tooltip("Object or category to affect or use")]
        public ObjectType objectType;

        [Tooltip("element or state to apply on object(s), only useful if element affection mission type")]
        public ElementFlag toApply;

        [Tooltip("where the objects have to be affected")]
        public RoomType room;
        
        [Tooltip("number of object to affect or use")]
        public int number;

        #endregion
    }
}