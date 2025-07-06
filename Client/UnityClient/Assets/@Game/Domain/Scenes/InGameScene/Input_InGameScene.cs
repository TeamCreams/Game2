using UnityEngine;

public class Input_InGameScene : IInputSystem
{
    public void Init()
    {
    }

    public void OnKeyAction()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Weapon weaponObj = Managers.Object.Spawn<Weapon>(this.transform.position, 0, 0, this.transform);
        }

    }
}
