using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

internal class Pool
{
	private GameObject _prefab;
	private IObjectPool<GameObject> _pool;

	private Transform _root;
	public Transform Root
	{
		get
		{
			if (_root == null)
			{
				GameObject go = new GameObject() { name = $"@{_prefab.name}Pool" };
				_root = go.transform;
			}

			return _root;
		}
	}

	public Pool(GameObject prefab, int defaultCapacity = 10)
	{
		_prefab = prefab;
		_pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, defaultCapacity: defaultCapacity);
	}

	public void Push(GameObject go)
	{
		if (go.activeSelf)
			_pool.Release(go);
	}

	public GameObject Pop()
	{
		return _pool.Get();
	}

	#region Funcs
	private GameObject OnCreate()
	{
		GameObject go = GameObject.Instantiate(_prefab);
		go.transform.SetParent(Root);
		go.name = _prefab.name;
		return go;
	}

	private void OnGet(GameObject go)
	{
		go.SetActive(true);
	}

	private void OnRelease(GameObject go)
	{
		go.SetActive(false);
	}

	private void OnDestroy(GameObject go)
	{
		GameObject.Destroy(go);
	}
	#endregion
}

public class PoolManager
{
	private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

	public GameObject Pop(GameObject prefab)
	{
		if (_pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
	}

	public bool Push(GameObject go)
	{
		if (_pools.ContainsKey(go.name) == false)
            return false;

		go.transform.SetParent(_pools[go.name].Root, false);

		_pools[go.name].Push(go);
		return true;
	}

	public int Count()
	{
		return _pools.Count;
    }

	public void Clear()
	{
		_pools.Clear();
	}

    public void CreatePool(GameObject original, int defaultCapacity = 10)
	{
		Pool pool = new Pool(original, defaultCapacity);
        _pools.Add(original.name, pool);
    }
}