using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemySpawner : BaseObject
{
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        Spawner();
        return true;
    }
    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }

        Observable.Interval(TimeSpan.FromSeconds(5))
            .Subscribe(_ =>
            {
                 Spawner();
            }).AddTo(this);

        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Contexts.BattleRush.EnemyObjectIdList.Remove(this.ObjectId);
    }


    private void Spawner()
    {
        var enemy = Managers.Object.Spawn<Enemy>(Vector3.zero, 0, 10000);
        //var enemy =  Managers.Resource.Instantiate("Enemy");
        //Contexts.BattleRush.EnemyObjectIdList.Add(enemy.ObjectId);
    }
}