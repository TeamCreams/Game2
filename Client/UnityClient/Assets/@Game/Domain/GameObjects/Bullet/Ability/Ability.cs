using System.Collections;
using Data;
using UnityEngine;

public class Ability : BaseObject
{
    private int _id = 0;
    private DummyData _dummyData;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _dummyData = new DummyData();

        return true;
    }
    private void OnDestroy()
    {

    }
    public override void SetInfo(int dataTemplate)
    {
         base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Ability Id : {_id}");

        if (_id <= 0)
        {
            Debug.LogError($"Invalid Ability ID: {_id}. Check if dataTemplate is properly set.");
            return;
        }

        if (!_dummyData.AbilityDataDict.ContainsKey(_id))
        {
            Debug.LogError($"Ability ID {_id} not found in AbilityDataDict. Available keys: {string.Join(", ", _dummyData.AbilityDataDict.Keys)}");
            return;
        }
        SpawnWeapon();
    }
    private void SpawnWeapon()
    {
        for (int cnt = 0; cnt < _dummyData.AbilityDataDict[_id].Count; cnt ++)
        {
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(this.transform.position, 0, _dummyData.AbilityDataDict[_id].WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetInfo(_dummyData.AbilityDataDict[_id].WeaponId);
            }   
        }
    }
    private void PushAbility()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}