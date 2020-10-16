using UnityEngine;

[CreateAssetMenu(fileName = "LongIntegerDataAsset", menuName = "Data/Data Asset/Long Integer")]
public class LongIntegerDataAsset : DataAsset<long>
{
	public LongIntegerDataAsset(string _id, uint _value) : base(_id, _value) { }

	public LongIntegerDataAsset(string _id, string _key, uint _value) : base(_id, _key, _value) { }

	public override void Load()
	{
		value = SaveSystem.GetLong(key, defaultValue);
	}

	public override void Save()
	{
		SaveSystem.SetLong(key, value);
	}
}