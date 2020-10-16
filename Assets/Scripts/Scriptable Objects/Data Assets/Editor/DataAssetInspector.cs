using UnityEditor;

[CustomEditor(typeof(BooleanDataAsset))]
public class BooleanDataAssetInspector : DataAssetInspector<BooleanDataAsset, bool>
{
	public override bool ValuePropertyEditor() => EditorGUILayout.Toggle("Value", value);
}

[CustomEditor(typeof(FloatDataAsset))]
public class FloatDataAssetInspector : DataAssetInspector<FloatDataAsset, float>
{
	public override float ValuePropertyEditor() => EditorGUILayout.FloatField("Value", value);
}

[CustomEditor(typeof(IntegerDataAsset))]
public class IntegerDataAssetInspector : DataAssetInspector<IntegerDataAsset, int>
{
	public override int ValuePropertyEditor() => EditorGUILayout.IntField("Value", value);
}

[CustomEditor(typeof(LongIntegerDataAsset))]
public class LongDataAssetInspector : DataAssetInspector<LongIntegerDataAsset, long>
{
	public override long ValuePropertyEditor() => EditorGUILayout.IntField("Value", (int)value);
}

[CustomEditor(typeof(StringDataAsset))]
public class StringDataAssetInspector : DataAssetInspector<StringDataAsset, string>
{
	public override string ValuePropertyEditor() => EditorGUILayout.TextField("Value", value);
}




public abstract class DataAssetInspector<Data, Type> : Editor where Data : DataAsset<Type> where Type : System.IComparable
{
	SerializedProperty id, key, defaultValue;
	new Data target;

	protected Type value
	{
		get => target.Value;
		set => target.Value = value;
	}

	private void OnEnable()
	{
		target = (Data)base.target;

		id = serializedObject.FindProperty("id");
		key = serializedObject.FindProperty("key");
		defaultValue = serializedObject.FindProperty("defaultValue");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		if (!string.IsNullOrEmpty(key.stringValue))
			target.Load();

		EditorGUILayout.LabelField(id.stringValue == null ? "" : $"{id.stringValue} - Data Object ({typeof(Type)})", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(id);
		EditorGUILayout.PropertyField(key);
		EditorGUILayout.PropertyField(defaultValue);
		EditorGUILayout.Space();

		if (!string.IsNullOrEmpty(key.stringValue))
			value = ValuePropertyEditor();

		serializedObject.ApplyModifiedProperties();
	}

	public abstract Type ValuePropertyEditor();
}
