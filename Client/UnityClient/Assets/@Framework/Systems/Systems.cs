using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public partial class Systems
{
    private static Systems s_instance;
    private static Systems Instance { get { Init_Framework(); return s_instance; } }

    #region Contents
    private UserSystem _user = new UserSystem();

    public static UserSystem User { get { return Instance?._user; } }
    #endregion


    public static void Init_Framework()
    {
        if (s_instance == null)
        {
            s_instance = new Systems();


            // 초기화
            s_instance._user.Init();
            Systems.Init_Game();
        }
    }

    /// <summary>
    /// 이 함수 있는 내용은 @Game안에 정의되어야하는 내용입니다.
    /// @Game에서 정의하고 이 함수의 내용을 주석해주세요.
    /// </summary>
    //private static void Init_Game()
    //{
    //    // @Game에서 정의된 내용이 있다면 여기에 작성해주세요.
    //    // 예시: s_instance._mycontent.Init();
    //}
}
