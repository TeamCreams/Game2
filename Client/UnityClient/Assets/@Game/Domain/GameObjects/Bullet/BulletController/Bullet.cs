using UnityEngine;

public class Bullet : BaseObject
{
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private float _speed = 10f;
    private float _lifeTime = 2f; // 총알 생존 시간 (2초)
    private float _currentTime = 0f;
    private int _id = 0;

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }
        OnTriggerEnter_Event -= Attack;
        OnTriggerEnter_Event += Attack;

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Bullet에 Rigidbody 컴포넌트가 없습니다!");
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {

    }
    
    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        MoveBullet();
        
        CheckLifeTime();
    }

    private void MoveBullet()
    {
        if (_rigidbody != null && _direction != Vector3.zero)
        {
            Vector3 newPosition = transform.position + _direction * _speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }
    }

    private void CheckLifeTime()
    {
        _currentTime += Time.fixedDeltaTime;
        
        // 2초가 지나면 총알 제거
        if (_lifeTime <= _currentTime)
        {
            PushStone(); 
        }
    }

    public override void SetInfo(int id)
    {
        _id = id;
        _currentTime = 0f; 
    }

    private void PushStone()
    {
        Managers.Resource.Destroy(this.gameObject);
    }

    private void Attack(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            Managers.Resource.Destroy(this.gameObject);
        }
    }
}