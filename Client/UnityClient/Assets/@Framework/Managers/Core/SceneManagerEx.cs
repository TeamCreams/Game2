using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
	public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
	public Action<Define.EScene> OnSceneChanged = null;

	public void LoadScene(Define.EScene type)
	{
		Managers.Clear();
        OnSceneChanged?.Invoke(type);
        SceneManager.LoadScene(GetSceneName(type));
    }

	private string GetSceneName(Define.EScene type)
	{
		string name = System.Enum.GetName(typeof(Define.EScene), type);
		return name;
	}

	public void Clear()
	{
		CurrentScene.Clear();
	}
}
