using System.Collections;
using Data;
using UnityEngine;

public class Weapon : BaseObject
{
    private CharacterController _characterController;
    private float _creatTime = 3.0f;
    private Coroutine _spawner = null;
    private bool _isSpawning = false;
    private int _id = 0;
    private DummyData _dummyData;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _characterController = GetComponentInParent<CharacterController>();
        _dummyData = new DummyData();
        return true;
    }

    private void OnDestroy()
    {
        StopSpawning();
    }
    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
        Debug.Log($"Weapon Id : {_id}");
        _creatTime = _dummyData.WeaponDataDict[_id].CoolDown;
        SpawnBullet();
        //_dummyData.WeaponDataList[_id].Type;
    }

    public void SpawnBullet()
    {
        if (_spawner == null)
        {
            _spawner = StartCoroutine(BulletSpawnerCor());
        }
    }

    public void StopSpawning()
    {
        if (_spawner != null)
        {
            StopCoroutine(_spawner);
            _spawner = null;
        }
        _isSpawning = false;
    }
    IEnumerator BulletSpawnerCor()
    {
        _isSpawning = true;
        
        while (_isSpawning)
        {
            BulletSpawner();
            yield return new WaitForSeconds(_creatTime);
        }
    }

    private void BulletSpawner()
    {
        // 시간마다
        // 개수
        // 플레이어를 중심으로 5개부터 시작해서 최대 20개까지
        int maxCount = _dummyData.WeaponDataDict[_id].Count;
        float angleStep = 360f / maxCount;
        for (int cnt = 0; cnt < maxCount; cnt++)
        {
            float currentAngle = cnt * angleStep;

            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            );

            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_characterController.transform.position, 0, _dummyData.WeaponDataDict[_id].BulletId);

            if (bulletObj != null)
            {
                bulletObj.SetInfo(_dummyData.WeaponDataDict[_id].BulletId);
                bulletObj.SetDirection(direction);
            }
        }
    }
    
    public void SetCreateTime(float time)
    {
        _creatTime = Mathf.Max(0.3f, time); // 최소 0.3초
        
        if (_isSpawning)
        {
            StopSpawning();
            SpawnBullet(); // 새로운 시간으로 재시작
        }
    }
}