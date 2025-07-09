using UnityEngine;

public class UI_InGameScene : UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowPopupUI<UI_SelectPopup>();
        Managers.UI.ShowBaseUI<UI_Joystick>();
        return true;
    }

}
