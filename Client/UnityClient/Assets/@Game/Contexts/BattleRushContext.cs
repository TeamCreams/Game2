using System;
using UniRx;
using UnityEngine;

public partial class BattleRushContext
{
    public Vector2 PlayerDir { get; set; }
    public int? PlayerObjectId { get; set; } = null;


    private Subject<int> _spawnAbilityEvent = new Subject<int>();
    public IObservable<int> SpawnAbilityEvent => _spawnAbilityEvent;
    public void SpawnAbility(int abilityId)
    {
        _spawnAbilityEvent.OnNext(abilityId);
    }

}