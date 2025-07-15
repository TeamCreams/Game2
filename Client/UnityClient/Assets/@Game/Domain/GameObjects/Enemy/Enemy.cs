using Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


public class Enemy : BaseObject
{
    private float _hp = 0f;
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        return true;
    }
    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
    }
    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        Contexts.BattleRush.EnemyObjectIdList.Add(this.ObjectId);
        this.OnTriggerEnterAsObservable()
            .Subscribe(collider => Attacked(collider))
            .AddTo(_disposables);
        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Contexts.BattleRush.EnemyObjectIdList.Remove(this.ObjectId);
    }

    #region Update
    private void Update_Idle()
    {
    }

    private void Update_Move()
    {

    }

    private void Update_Die()
    {
    }
    #endregion
    private void Attacked(Collider collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            // bullet 삭제
            // 데미지 처리
        }
    }

}
