﻿using UnityEngine;

//[CreateAssetMenu(fileName = "AssetCollection", menuName = "/Asset Collection")]
public abstract class AssetCollection<T> : ScriptableObject where T : Object
{
	[SerializeField] protected T[] _collection;

	/// <summary>
	/// Zero based Length.
	/// </summary>
	public int Length => _collection.Length;

	/// <summary>
	/// Get an collection item at index.
	/// </summary>
	/// <param name="index">The index to get item</param>
	public virtual T GetAt(int index)
	{
		if (index >= _collection.Length || index < 0)
		{
			Debug.LogWarning("Trying to get an object out of the limit", this);
		}

		return _collection[Mathf.Clamp(index, 0, _collection.Length - 1)];
	}

	/// <summary>
	/// Get item at a random index.
	/// </summary>
	public virtual T GetRandom()
	{
		return GetRandom(out _);
	}
	/// <summary>
	/// Get item at a random index.
	/// </summary>
	/// <param name="index">Get the random used index out</param>
	public virtual T GetRandom(out int index)
	{
		index = Random.Range(0, _collection.Length);
		return GetAt(index);
	}


#if UNITY_EDITOR
	protected virtual void OnDisable()
	{
		// Safety measures
		if (_collection.Length == 0)
		{
			Debug.LogError($"Empty collection \"{name}\"", this);
		}
		else
		{
			for (int i = 0; i < _collection.Length; i++)
			{
				if (_collection[i] == null)
					Debug.LogError($"Object missing or not set on \"{name}\"[{i}]", this);
			}
		}
	}
#endif
}