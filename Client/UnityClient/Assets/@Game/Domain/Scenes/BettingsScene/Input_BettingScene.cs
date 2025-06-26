using UnityEngine;

public class Input_BettingScene : IInputSystem
{
    public void Init()
    {
    }

    public void OnKeyAction()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Event_BettingScene.BettingStart?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Event_BettingScene.BettingFinish?.Invoke();
        }
    }
}
