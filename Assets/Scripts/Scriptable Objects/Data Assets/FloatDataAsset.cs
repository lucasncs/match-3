using UnityEngine;

[CreateAssetMenu(fileName = "FloatDataAsset", menuName = "Data/Data Asset/Float")]
public class FloatDataAsset : DataAsset<float>
{
	public FloatDataAsset(string _id, float _value) : base(_id, _value) { }

	public FloatDataAsset(string _id, string _key, float _value) : base(_id, _key, _value) {	}

	public override void Load()
	{
		value = SaveSystem.GetFloat(key, defaultValue);
	}

	public override void Save()
	{
		SaveSystem.SetFloat(key, value);
	}
}
