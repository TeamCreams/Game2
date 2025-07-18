using Data;
using UnityEngine;
public partial class Ability
{
    // Follow
    private void SpawnFollowWeapons()
    {
        for (int cnt = 0; cnt < _info.Count; cnt++)
        {
            Debug.Log("SpawnFollowWeapons" + cnt);

            Vector3 spawnPosition = _ownerTransform != null ? _ownerTransform.position : transform.position;
            Weapon weaponObj = Managers.Object.Spawn<Weapon>(spawnPosition, 0, _info.WeaponId, this.transform);
            if (weaponObj != null)
            {
                weaponObj.SetOwner(_ownerObjectId);
                weaponObj.SetAbilityDataEType(_info.Type);
                _weaponList.Add(weaponObj);
            }
        }

        // 히스토리 초기화
        InitializePositionHistory();
    }
    private void InitializePositionHistory()
    {
        if (_ownerTransform == null) return;

        int historySize = Mathf.RoundToInt(_historyDuration / _positionUpdateInterval);

        for (int i = 0; i < historySize; i++)
        {
            _playerPositionHistory.Enqueue(_ownerTransform.position);
        }
    }
    private void UpdateFollowWeapons() 
    {
        UpdatePositionHistory();
        
        // 1초 전 위치 가져오기
        Vector3 targetPosition = GetHistoryPosition();
        
        foreach (var weapon in _weaponList)
        {
            if (weapon != null)
            {
                weapon.transform.position = Vector3.Lerp(weapon.transform.position, targetPosition, Time.deltaTime * 5f);
            }
        }
    }
    
    private void UpdatePositionHistory()
    {
        _positionUpdateTimer += Time.deltaTime;
    
        if (_positionUpdateInterval <= _positionUpdateTimer)
        {
            // 오래된 위치 제거
            if ((int)(_historyDuration / _positionUpdateInterval) <= _playerPositionHistory.Count)
            {
                _playerPositionHistory.Dequeue();
            }
            
            // 새로운 위치 추가
            _playerPositionHistory.Enqueue(_ownerTransform.position);
            
            _positionUpdateTimer = 0f;
        }
    }
    
    // 1초 전 위치 가져오기
    private Vector3 GetHistoryPosition()
    {
        if (_playerPositionHistory.Count == 0)
        {
            return _ownerTransform.position;
        }
        
        return _playerPositionHistory.Peek();
    }
}