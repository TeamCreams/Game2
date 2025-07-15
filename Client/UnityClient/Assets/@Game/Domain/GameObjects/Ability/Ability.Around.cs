using Data;
using UnityEngine;
public partial class Ability
{
    // Around
    private void SpawnAroundWeapons(AbilityData abilityData)
    {
        float angleStep = 360f / abilityData.Count;

        for (int cnt = 0; cnt < abilityData.Count; cnt++)
        {
            float angle = cnt * angleStep;
            Vector3 spawnPosition = CalculateOrbitPosition(angle);

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
    private Vector3 CalculateOrbitPosition(float angle)
    {
        if (_ownerTransform == null) return transform.position;

        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * _orbitRadius;
        float z = Mathf.Sin(radian) * _orbitRadius;
        return _ownerTransform.position + new Vector3(x, 0, z);
    }
    // Around
    private void UpdateAroundWeapons()
    {
        if (_weaponList.Count == 0) return;
        
        _currentRotationAngle += _rotationSpeed * Time.deltaTime;
        if (360 <= _currentRotationAngle)
        {
            _currentRotationAngle -= 360f;
        }
        
        for (int i = 0; i < _weaponList.Count; i++)
        {
            if (_weaponList[i] == null) continue;
            
            float baseAngle = (float)(360 / _weaponList.Count) * i;
            float currentAngle = baseAngle + _currentRotationAngle;
            
            Vector3 newPosition = CalculateOrbitPosition(currentAngle);
            _weaponList[i].transform.position = newPosition;
            
            Vector3 direction = (newPosition - _ownerTransform.position).normalized;
            if (direction != Vector3.zero)
            {
                _weaponList[i].transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}