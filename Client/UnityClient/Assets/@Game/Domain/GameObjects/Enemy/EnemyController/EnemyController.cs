using UnityEngine;

public class EnemyController : BaseObject
{
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {

    }
}
