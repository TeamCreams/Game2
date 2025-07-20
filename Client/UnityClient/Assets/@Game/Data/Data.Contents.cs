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

    public class DummyData
    {
        private Dictionary<int, AbilityData> _abilityDataDict = new Dictionary<int, AbilityData>()
        {
            { 10001, new AbilityData() { Id = 10001, WeaponId = 10101, Type = AbilityData.EType.Static, Count = 1 } },
            { 10002, new AbilityData() { Id = 10002, WeaponId = 10102, Type = AbilityData.EType.Around, Count = 5 } },
            { 10003, new AbilityData() { Id = 10003, WeaponId = 10103, Type = AbilityData.EType.Follow, Count = 3 } },
            { 10004, new AbilityData() { Id = 10004, WeaponId = 10104, Type = AbilityData.EType.Static, Count = 2 } },
            { 10005, new AbilityData() { Id = 10005, WeaponId = 10105, Type = AbilityData.EType.Static, Count = 1 } },

        };

        private Dictionary<int, WeaponData> _weaponDataDict = new Dictionary<int, WeaponData>()
        {
            { 10101, new WeaponData() { Id = 10101, BulletId = 10201, Type = WeaponData.EType.Missile, Count = 5, CoolDown = 3.5f } },
            { 10102, new WeaponData() { Id = 10102, BulletId = 10202, Type = WeaponData.EType.Missile, Count = 10, CoolDown = 0.1f } },
            { 10103, new WeaponData() { Id = 10103, BulletId = 10203, Type = WeaponData.EType.GuidedMissile, Count = 1, CoolDown = 3.0f } },
            { 10104, new WeaponData() { Id = 10104, BulletId = 10204, Type = WeaponData.EType.MagneticField, Count = 1, CoolDown = 4.0f } },
            { 10105, new WeaponData() { Id = 10105, BulletId = 10205, Type = WeaponData.EType.Laser, Count = 1, CoolDown = 3.0f } },
        };

        private Dictionary<int, BulletData> _bulletDataDict = new Dictionary<int, BulletData>()
        {
            { 10201, new BulletData() { Id = 10201, Type = BulletData.EPositionType.Parent, LifeTime = 2.0f, Speed = 5.0f, Damage = 5.0f } },
            { 10202, new BulletData() { Id = 10202, Type = BulletData.EPositionType.World, LifeTime = 0.2f, Speed = 5.0f, Damage = 8.0f } },
            { 10203, new BulletData() { Id = 10203, Type = BulletData.EPositionType.Parent, LifeTime = 3.0f, Speed = 7.0f, Damage = 10.0f } },
            { 10204, new BulletData() { Id = 10204, Type = BulletData.EPositionType.World, LifeTime = 1.5f, Speed = 3.0f, Damage = 6.0f } },
            { 10205, new BulletData() { Id = 10205, Type = BulletData.EPositionType.World, LifeTime = 1.5f, Speed = 3.0f, Damage = 6.0f } },
        };

        // 딕셔너리 접근자 프로퍼티
        public IReadOnlyDictionary<int, AbilityData> AbilityDataDict => _abilityDataDict;

        public IReadOnlyDictionary<int, WeaponData> WeaponDataDict => _weaponDataDict;

        public IReadOnlyDictionary<int, BulletData> BulletDataDict => _bulletDataDict;
    }

}