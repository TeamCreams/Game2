using UnityEngine;
using static Define;

public class LoadScene : BaseScene
{
    [SerializeField]
    private EScene _scene = EScene.BettingScene;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Resource.LoadAsync<Object>(nameof(UI_LoadingPopup), async (_) =>
        {
            await UI_LoadingPopup.Show(async () =>
            {
                //LoadAllAsync는 메인스레드에서 실행되어야 합니다.
                await Awaitable.MainThreadAsync();
                Managers.Resource.LoadAllAsync<Object>("PreLoad", async (key, count, totalCount) =>
                {
                    Debug.Log($"{key} {count}/{totalCount}");

                    if (count == totalCount)
                    {
                        Managers.Data.Init();

                        //LoadScene은 메인스레드에서 실행되어야 합니다.
                        await Awaitable.MainThreadAsync();
                        Managers.Scene.LoadScene(_scene);
                    }
                });
            }, EThread.Main);
        });

        return true;
    }


    public override void Clear()
    {
    }

}
