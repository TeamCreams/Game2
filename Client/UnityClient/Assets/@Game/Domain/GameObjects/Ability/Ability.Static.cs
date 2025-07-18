using Data;
using UnityEngine;

public partial class Ability
{

    // Static
    private void SpawnStaticWeapons()
    {
        for (int cnt = 0; cnt < _info.Count; cnt++)
        {
            Debug.Log("SpawnStaticWeapons" + cnt);
            Vector3 spawnPosition = _ownerTransform != null ? _ownerTransform.position : transform.position;
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, _info.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetOwner(_ownerObjectId);
                weaponObj.SetAbilityDataEType(_info.Type);
                _weaponList.Add(weaponObj);
            }
        }
    }
}