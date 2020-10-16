using UnityEngine;

[CreateAssetMenu(fileName = "BooleanDataAsset", menuName = "Data/Data Asset/Boolean"), DefaultExecutionOrder(100)]
public class BooleanDataAsset : DataAsset<bool>
{
	public BooleanDataAsset(string _id, bool _value) : base(_id, _value) { }

	public BooleanDataAsset(string _id, string _key, bool _value) : base(_id, _key, _value) { }

	public override void Load()
	{
		value = SaveSystem.GetBool(key, defaultValue);
	}

	public override void Save()
	{
		SaveSystem.SetBool(key, value);
	}
}
