using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class BattleRushContext
{
    public Vector2 PlayerDir { get; set; }
    public int? PlayerObjectId { get; set; } = null;
    public List<int> EnemyObjectIdList { get; set; }
    public float? ScreenHeight { get; set; } = null;
    public float? ScreenWidth { get; set; } = null;


    private Subject<int> _spawnAbilityEvent = new Subject<int>();
    public IObservable<int> SpawnAbilityEvent => _spawnAbilityEvent;
    public void SpawnAbility(int abilityId)
    {
        _spawnAbilityEvent.OnNext(abilityId);
    }
    
    //int abilityId
    public Subject<int> OnClickSpawnAbilityButton { get; private set; } = new Subject<int>();

    private Subject<int> _spawnWeaponEvent = new Subject<int>();
    public IObservable<int> SpawnWeaponEvent => _spawnWeaponEvent;
    public void SpawnWeapon(int weaponId)
    {
        _spawnWeaponEvent.OnNext(weaponId);
    }

    private Subject<int> _spawnBulletEvent = new Subject<int>();
    public IObservable<int> SpawnBulletEvent => _spawnBulletEvent;
    public void SpawnBullet(int bulletId)
    {
        _spawnBulletEvent.OnNext(bulletId);
    }
    

    

}