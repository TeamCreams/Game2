using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
    [Serializable]
    public class AbilityData : IData
    {
        public enum EType
        {
            Static,
            Around,
            Follow,
        }

        [JsonProperty("Id")]
        public int Id { get; set; }
        public int WeaponId { get; set; }

        //WEAPON을 어떻게 생성할건지에 대한
        public EType Type { get; set; }
        public int Count { get; set; }
    }


    [Serializable]
    public class WeaponData : IData
    {
        public enum EType
        {
            Missile,
            GuidedMissile,
            MagneticField,
            Laser,
            //드릴
        }

        [JsonProperty("Id")]
        public int Id { get; set; }
        public int BulletId { get; set; }

        //BULLET을 어떻게 생성할건지에 대한
        public EType Type { get; set; }
        public int Count { get; set; }
        public float CoolDown { get; set; }

    }


    [Serializable]
    public class BulletData : IData
    {
        public enum EPositionType
        {
            Parent,
            World,
        }

        [JsonProperty("Id")]
        public int Id { get; set; }
        
        //BULLET을 어떻게 생성할건지에 대한
        public EPositionType Type { get; set; }
        public float LifeTime { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
    }


}