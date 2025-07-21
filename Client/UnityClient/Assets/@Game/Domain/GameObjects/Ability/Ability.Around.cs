using Data;
using UnityEngine;
public partial class Ability
{
    // Around
    private void SpawnAroundWeapons()
    {
        Debug.Log("SpawnAroundWeapons");

        float angleStep = 360f / _info.Count;

        for (int cnt = 0; cnt < _info.Count; cnt++)
        {
            float angle = cnt * angleStep;
            Vector3 spawnPosition = CalculateOrbitPosition(angle);

            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, _info.WeaponId, this.transform);
            if (weaponObj != null)
            {

                weaponObj.SetInfo(_info.WeaponId);
                weaponObj.SetOwner(_ownerObjectId);
                weaponObj.SetAbilityDataEType(_info.Type);
                weaponObj.SetIndex(cnt, _info.Count - 1);
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

    // 이동 스크립트 weapon, bullet 안으로 옮기기
    // 스폰만 ability에서 하고 이동은 weapon에서
    /*
    private Vector3 CalculateOrbitPosition(float angle)
    {
        if (_ownerTransform == null) return transform.position;

        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * _orbitRadius;
        float z = Mathf.Sin(radian) * _orbitRadius;
        return _ownerTransform.position + new Vector3(x, 0, z);
    }
    private void UpdateAroundWeapon(int index, int allCount)
    {
        _currentRotationAngle += _rotationSpeed * Time.deltaTime;
        if (360 <= _currentRotationAngle)
        {
            _currentRotationAngle -= 360f;
        }

        // _ownerTransform
        float baseAngle = (float)(360 / allCount) * index;
        float currentAngle = baseAngle + _currentRotationAngle;

        Vector3 newPosition = CalculateOrbitPosition(currentAngle);
        this.transform.position = newPosition;

        Vector3 direction = (newPosition - _ownerTransform.position).normalized;
        if (direction != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    */
}