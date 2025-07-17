using Data;
using UnityEngine;
public partial class Weapon
{
    private void Event_SpawnMagneticFieldBullets()
    {
        Debug.Log("Event_SpawnMagneticFieldBullets");
        Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);

        if (bulletObj != null)
        {
            bulletObj.SetInfo(_info.BulletId);
        }
    }
    private void SpawnMagneticFieldBullets()
    {
        // 총알은 lifetime을 짧게
    }

}