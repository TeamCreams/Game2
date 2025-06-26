using UnityEngine;
using static Define;

public class BettingScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        // 기본적인 객체 생성
        InputSystem = new Input_BettingScene();
        InputSystem.Init();


        // 이벤트 바인딩
        Event_BettingScene.BettingStart += OnBettingStart;
        Event_BettingScene.BettingFinish += OnBettingFinish;

        return true;
    }

    public void OnBettingStart()
    {
        Debug.Log("Betting has started.");
    }

    public void OnBettingFinish()
    {
        Debug.Log("Betting has finished.");
    }

    public override void Clear()
    {
        Event_BettingScene.BettingStart -= OnBettingStart;
        Event_BettingScene.BettingFinish -= OnBettingFinish;
    }

}
