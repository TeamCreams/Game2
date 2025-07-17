using Data;
using UnityEngine;
public partial class Weapon
{
    private void Event_SpawnGuidedMissile(int bulletId)
    {
        // 시간마다
        // 개수
        // 플레이어를 중심으로 5개부터 시작해서 최대 20개까지
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
            if (bulletObj != null)
            {
                bulletObj.SetInfo(_info.BulletId);
                bulletObj.MoveToTargetBullet(target);
            }
        }
    }

    private void SpawnGuidedMissileBullets(WeaponData weaponData)
    {
        _isSetting = true;
        Event_SpawnGuidedMissile(weaponData.BulletId);
    }
}