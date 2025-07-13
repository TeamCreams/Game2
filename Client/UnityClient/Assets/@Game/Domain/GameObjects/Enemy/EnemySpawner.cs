using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : BaseObject
{
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {

    }

    private void Spawner()
    {
        var enemy = Managers.Object.Spawn<Enemy>(Vector3.zero, 0, 0);
        //var enemy =  Managers.Resource.Instantiate("Enemy");
        //Contexts.BattleRush.EnemyObjectIdList.Add(enemy.ObjectId);
    }
}