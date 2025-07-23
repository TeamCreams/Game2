using UnityEngine;

public class BulletParticle : BaseObject
{
    private int _id;

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
    public void SetParents(Transform parent)
    {
        this.gameObject.transform.parent = parent;
    }
}
