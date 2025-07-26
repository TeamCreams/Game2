using Data;
using UnityEngine;
public partial class Weapon
{
    private void Event_SpawnMagneticFieldBullets()
    {
        Debug.Log("Event_SpawnMagneticFieldBullets");
        MagneticField bulletObj = Managers.Object.Spawn<MagneticField>(_position, 0, _info.BulletId, this.transform);

        if (bulletObj != null)
        {
            bulletObj.SetInfo(_info.BulletId);
            //bulletObj.SetBulletType(_info.Type);
            bulletObj.SetOwner(_ownerTransform);
        }
    }
    private void SpawnMagneticFieldBullets()
    {
        // 총알은 lifetime을 짧게
        Event_SpawnMagneticFieldBullets();
    }

}