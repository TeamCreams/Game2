using System.Collections.Generic;
using Data;
using UnityEngine;
public partial class Weapon
{
    #region Arround
    // Around 변수들
    private float _rotationSpeed = 50f;
    private float _currentRotationAngle = 0f;
    private float _orbitRadius = 2;

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
    #endregion

    #region Follow
    // Follow 변수들
    private Queue<Vector3> _playerPositionHistory = new Queue<Vector3>(); 
    private float _historyDuration = 1.0f; 
    private float _positionUpdateInterval = 0.5f;
    private float _positionUpdateTimer = 0f;
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
        
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 5f);

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
    #endregion
}