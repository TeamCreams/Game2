using UnityEngine;

public class Flameshrower : BulletParticle
{
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
        this.transform.localPosition = Vector3.zero;
    }
    public override void SetParents(Transform parent)
    {
        base.SetParents(parent);
        this.gameObject.transform.parent = parent;
    }
}
