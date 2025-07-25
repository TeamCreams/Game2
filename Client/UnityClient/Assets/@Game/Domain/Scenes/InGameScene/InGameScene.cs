using System;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;

public class InGameScene : BaseScene
{
    UI_Joystick _joyStick = null;
    Player _player = null;

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

        Contexts.BattleRush.OnClickSpawnAbilityButton
            .Subscribe(abilityId =>
            {
                if (_player == null) return;
                
                _player.OnSpawnAbility?.OnNext(abilityId);
            })
            .AddTo(_disposables);

        var brain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        // if (brain != null)
        // {
        //     Camera brainCamera = brain.OutputCamera;
        //     Contexts.BattleRush.ScreenWidth = brainCamera.pixelWidth;
        //     Contexts.BattleRush.ScreenHeight = brainCamera.pixelHeight;
        // }

        return true;
    }

    // 리소스 로드후 실행하는 곳
    public void LateInit()
    {
        Managers.UI.ShowSceneUI<UI_InGameScene>();

        _player = Managers.Object.Spawn<Player>(Vector3.zero, 0, 0);
        var followCamera = Managers.Object.Spawn<FreeLookCamera>(Vector3.zero, 0, 0);
    
        Contexts.BattleRush.PlayerObjectId = _player.ObjectId;
        followCamera.SetTarget();

        Managers.Object.Spawn<EnemySpawner>(Vector3.zero, 0, 0);
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
