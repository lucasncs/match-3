using System;
using UnityEngine;

[Serializable]
public abstract class DataAsset<T> : DataAsset where T : System.IComparable
{
	public DataAsset(string _id, T _value)
	{
		if (string.IsNullOrWhiteSpace(_id))
			throw new ArgumentException("ID not set or invalid", nameof(_id));

		id = _id;
		key = _id;
		value = _value;
	}
	public DataAsset(string _id, string _key, T _value) : this(_id, _value)
	{
		key = _key;
	}

	[SerializeField] protected T defaultValue;
	protected T value;

	public event Action<T> OnValueChange;

	/// <summary>
	/// Data stored
	/// </summary>
	public virtual T Value
	{
		get => value;
		set
		{
			this.value = value;
			Save();
			OnValueChange?.Invoke(this.value);
		}
	}

	protected virtual void OnEnable()
	{
		if (!SaveSystem.HasKey(key) && key != null)
		{
			//Debug.Log($"{id} || Getting Default = {defaultValue} ({typeof(T)})", this);
			Value = defaultValue;
		}
		else if (key != null)
		{
			Load();
			OnValueChange?.Invoke(value);
		}
	}
}

[Serializable]
public abstract class DataAsset : ScriptableObject
{
	[SerializeField] protected string id = "";
	[SerializeField] protected string key = "";

	/// <summary>
	/// Identification Name
	/// </summary>
	public string Id => id;
	/// <summary>
	/// Key string for saving this item's information
	/// </summary>
	public string Key => key;


	/// <summary>
	/// Save data
	/// </summary>
	public abstract void Save();
	/// <summary>
	/// Load data
	/// </summary>
	public abstract void Load();


#if UNITY_EDITOR
	private void OnDisable()
	{
		if (!SaveSystem.HasKey(key) && key != null)
			Save();
	}
#endif
}