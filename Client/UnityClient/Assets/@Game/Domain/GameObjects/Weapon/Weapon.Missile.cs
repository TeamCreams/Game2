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
        // 예시: 360도 전체가 아니라, 120도 부채꼴에 10개 발사
        int maxCount = _info.Count;
        if (maxCount == 1)
        {
            // 그냥 정면으로 발사
            Vector3 worldDir = this.transform.forward;

            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);
            if (bulletObj != null)
            {
                bulletObj.SetInfo(_info.BulletId);
                bulletObj.SetBulletType(_info.Type);
                bulletObj.SetDirection(worldDir);
            }
        }
        else
        {
            float totalAngle = 120f;
            float startAngle = -totalAngle * 0.5f;
            float angleStep = totalAngle / (maxCount - 1);

            for (int cnt = 0; cnt < maxCount; cnt++)
            {
                float currentAngle = startAngle + angleStep * cnt;
                float radians = currentAngle * Mathf.Deg2Rad;
                Vector3 localDir = new Vector3(
                    Mathf.Sin(radians),
                    0f,
                    Mathf.Cos(radians)
                );
                Vector3 worldDir = this.transform.rotation * localDir;

                Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);
                if (bulletObj != null)
                {
                    bulletObj.SetInfo(_info.BulletId);
                    bulletObj.SetBulletType(_info.Type);
                    bulletObj.SetDirection(worldDir);
                }
            }
        }

    }
    private void SpawnMissileBullets()
    {
        // 피젯스피너에서 나오는 총알은 lifetime을 짧게
        Event_SpawnMissile();
    }
    
}