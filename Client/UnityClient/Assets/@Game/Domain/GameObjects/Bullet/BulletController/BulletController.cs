using System.Collections;
using UnityEngine;

public class BulletController : BaseObject
{
    private CharacterController _characterController;
    private int MaxCount = 5;
    private float CreatTime = 3.0f;
    private Coroutine _spawner = null;
    private bool _isSpawning = false;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        _characterController = GetComponent<CharacterController>();
        SpawnBullet();
        return true;
    }

    private void OnDestroy()
    {
        StopSpawning();
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
            yield return new WaitForSeconds(CreatTime);
        }
    }

    private void BulletSpawner()
    {
        // 시간마다
        // 개수
        // 플레이어를 중심으로 5개부터 시작해서 최대 20개까지
        float angleStep = 360f / MaxCount;

        for (int cnt = 0; cnt < MaxCount; cnt++)
        {
            float currentAngle = cnt * angleStep;

            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            );

            Bullet bulletObj = Managers.Object.Spawn<Bullet>(_characterController.transform.position, 0, 0);

            if (bulletObj != null)
            {
                bulletObj.SetDirection(direction);
            }
        }
    }

    public void SetMaxCount(int count)
    {
        // 5개에서 20개 사이로 제한
        MaxCount = Mathf.Clamp(count, 5, 20);
    }
    
    public void SetCreateTime(float time)
    {
        CreatTime = Mathf.Max(0.3f, time); // 최소 0.3초
        
        if (_isSpawning)
        {
            StopSpawning();
            SpawnBullet(); // 새로운 시간으로 재시작
        }
    }
}