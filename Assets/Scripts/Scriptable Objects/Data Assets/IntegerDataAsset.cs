using UnityEngine;

[CreateAssetMenu(fileName = "IntegerDataAsset", menuName = "Data/Data Asset/Integer")]
public class IntegerDataAsset : DataAsset<int>
{
	public IntegerDataAsset(string _id, int _value) : base(_id, _value) { }

	public IntegerDataAsset(string _id, string _key, int _value) : base(_id, _key, _value) { }

	public override void Load()
	{
		value = SaveSystem.GetInt(key, defaultValue);
	}

	public override void Save()
	{
		SaveSystem.SetInt(key, value);
	}
}
