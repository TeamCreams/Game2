using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager 
{
    //미사일
    public Transform GetNearestEnemy(Vector3 position, float maxDistance = 20f)
    {
        Transform nearestEnemy = null;
        float shortestDistanceSqr = maxDistance * maxDistance;

        foreach (int id in Contexts.BattleRush.EnemyObjectIdList)
        {
            Transform enemy = Managers.Object.ObjectDic[id].transform;

            float distanceSqr = (enemy.position - position).sqrMagnitude;

            if (distanceSqr < shortestDistanceSqr)
            {
                shortestDistanceSqr = distanceSqr;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
    //자기장
    public List<Transform> GetEnemiesInRange(Vector3 position, float range)
    {
        List<Transform> enemiesInRange = new List<Transform>();
        float rangeSqr = range * range;

        foreach (int id in Contexts.BattleRush.EnemyObjectIdList)
        {
            Transform enemy = Managers.Object.ObjectDic[id].transform;
            float distanceSqr = (enemy.position - position).sqrMagnitude;
            if (distanceSqr <= rangeSqr)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
}