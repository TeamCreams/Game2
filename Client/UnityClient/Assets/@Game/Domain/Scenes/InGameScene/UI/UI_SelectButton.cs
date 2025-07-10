using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SelectButton : UI_Base
{
    enum Texts
    {
        Name,
    }
    int _id = 0;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        this.gameObject.BindEvent(OnClick_Button, Define.EUIEvent.Click);
        return true;
    }

    void OnClick_Button(PointerEventData eventData)
    {
        SpawnAbillity();
    }
    public void SetInfo(int id)
    {
        _id = id;
        GetText((int)Texts.Name).text = id.ToString();
    }    
    private void SpawnAbillity()
    {
        Contexts.BattleRush.SpawnAbility(_id);
    }
}
