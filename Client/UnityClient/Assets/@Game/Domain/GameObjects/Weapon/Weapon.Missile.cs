using Data;
using UnityEngine;
public partial class Weapon
{
    private void Event_SpawnMissile()
    {
        // 시간마다
        // 개수
        // 플레이어를 중심으로 5개부터 시작해서 최대 20개까지
        Debug.Log("Event_SpawnMissile");
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

            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);

            if (bulletObj != null)
            {
                bulletObj.SetInfo(_info.BulletId);
                bulletObj.SetBulletType(_info.Type);
                bulletObj.SetDirection(direction);
            }
        }
    }
    private void SpawnMissileBullets()
    {
        // 피젯스피너에서 나오는 총알은 lifetime을 짧게
        Event_SpawnMissile();
    }
    
}