using Data;
using UnityEngine;
public partial class Weapon
{
    

    private void Event_SpawnLaserBullets()
    {
        Debug.Log("Event_SpawnLaserBullets");
        Bullet bulletObj = Managers.Object.Spawn<Bullet>(_position, 0, _info.BulletId);

        if (bulletObj != null)
        {
            bulletObj.SetInfo(_info.BulletId);
            bulletObj.SetBulletType(_info.Type);
            bulletObj.SetDirection(Vector2.up);
        }
    }

    private void SpawnLaserBullets()
    {
        
    }
}