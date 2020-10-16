using UnityEngine;

[CreateAssetMenu(fileName = "StringDataAsset", menuName = "Data/Data Asset/String")]
public class StringDataAsset : DataAsset<string>
{
	public StringDataAsset(string _id, string _value) : base(_id, _value) { }

	public StringDataAsset(string _id, string _key, string _value) : base(_id, _key, _value) { }

	public override void Load()
	{
		value = SaveSystem.GetString(key, defaultValue);
	}

	public override void Save()
	{
		SaveSystem.SetString(key, value);
	}
}
