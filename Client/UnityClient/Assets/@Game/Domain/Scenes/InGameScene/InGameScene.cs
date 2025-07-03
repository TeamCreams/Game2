using UnityEngine;

public class InGameScene : BaseScene
{
    UI_Joystick _joyStick = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        StartLoadAssets("PreLoad");
        return true;
    }
    public override void Clear()
    {
    }
    
    private void StartLoadAssets(string label)
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                //Managers.Data.Init();
                //Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("Bullet"), 100);
            }
        });
    }
}
