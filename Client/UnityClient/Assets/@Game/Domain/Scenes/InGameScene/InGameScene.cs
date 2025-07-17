using UniRx;
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

        Event_UI_Joystick.JoystickAmountEvent
            .Subscribe(dir =>
            {
                //Debug.Log($"Joystick Changed : {dir.ToString()}");
                Contexts.BattleRush.PlayerDir = dir;
            })
            .AddTo(_disposables);

        return true;
    }

    // 리소스 로드후 실행하는 곳
    public void LateInit()
    {
        Managers.UI.ShowSceneUI<UI_InGameScene>();

        var player = Managers.Object.Spawn<Player>(Vector3.zero, 0, 0);

        var followCamera = Managers.Object.Spawn<FreeLookCamera>(Vector3.zero, 0, 0);

        Contexts.BattleRush.PlayerObjectId = player.ObjectId;

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
                this.LateInit();
            }
        });
    }
}
