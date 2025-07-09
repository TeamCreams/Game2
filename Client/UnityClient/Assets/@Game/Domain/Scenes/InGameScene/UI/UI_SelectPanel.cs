using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SelectPanel : UI_Base
{
    bool[] _list = new bool[10];
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        SpawnButtons();
        return true;
    }

    private void SpawnButtons()
    {
        List<int> randomNumbers = new List<int>();
        while (randomNumbers.Count < 4)
        {
            int randomNum = Random.Range(1, 5);
            if (!randomNumbers.Contains(randomNum))
            {
                randomNumbers.Add(randomNum);
            }
        }

        for (int i = 0; i < _list.Length; i++)
        {
            _list[i] = false;
        }

        foreach (int num in randomNumbers)
        {
            _list[num] = true;
        }

        for (int i = 0; i < _list.Length; i++)
        {
            if (_list[i])
            {
                var ui = Managers.UI.MakeSubItem<UI_SelectButton>(parent: this.transform, pooling: true);;
                ui.SetInfo(i);
            }
        }
    }
}
