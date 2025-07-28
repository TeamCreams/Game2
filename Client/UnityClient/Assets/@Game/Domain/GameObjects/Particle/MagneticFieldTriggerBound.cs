using Data;
using UnityEngine;

public class MagneticFieldTriggerBound : BaseObject
{
    private int _id = 0;
    private float _damage;
    public float Damage
    {
        get => _damage;
    }

    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        return true;
    }

    public override bool OnSpawn()
    {
        if (false == base.OnSpawn())
        {
            return false;
        }
        return true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    public override void SetInfo(int dataTemplate)
    {
        base.SetInfo(dataTemplate);
        _id = DataTemplateID;
    }
    public void SetBulletInfo(float damage)
    {
        _damage = damage;
    }
    public void SetParent(Transform parent)
    {
        this.transform.parent = parent;
        this.transform.localPosition = Vector3.zero;
    }

}
