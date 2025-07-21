using Data;
using UnityEngine;
public partial class Weapon
{
    private void Event_SpawnGuidedMissile()
    {
        //Debug.Log("Event_SpawnGuidedMissile");

        SetWeaponPosition(); // 위치 조정
        int maxCount = _info.Count;
        float angleStep = 360f / maxCount;
        for (int cnt = 0; cnt < maxCount; cnt++)
        {
            float currentAngle = cnt * angleStep;

            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            );

            Transform target = Managers.Enemy.GetNearestEnemy(_ownerTransform.position);
            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);
            if (bulletObj != null && target != null)
            {
                bulletObj.SetInfo(_info.BulletId);
                bulletObj.SetBulletType(_info.Type);
                bulletObj.SetTarget(target);
            }
            Debug.Log("Success Event_SpawnGuidedMissile");
        }
    }

    private void SpawnGuidedMissileBullets()
    {
        Event_SpawnGuidedMissile();
    }
}