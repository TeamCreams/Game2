using Data;
using UnityEngine;

public partial class Ability
{

    // Static
    private void SpawnStaticWeapons(AbilityData abilityData)
    {
        for (int cnt = 0; cnt < abilityData.Count; cnt++)
        {
            Vector3 spawnPosition = _ownerTransform != null ? _ownerTransform.position : transform.position;
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, abilityData.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetInfo(abilityData.WeaponId);
                weaponObj.SetOwner(_ownerObjectId);
                weaponObj.SetAbilityDataEType(abilityData.Type);
                _weaponList.Add(weaponObj);
            }
        }
    }
}