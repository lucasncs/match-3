using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(MenuStateManager))]
public class MenuStateManagerEditor : Editor
{
	private const string ENUM_NAME = "MenuState";

	List<string> _statesList = new List<string>();
	bool _fold;
	bool _showAll;
	bool _fileExists;

	private void OnEnable()
	{
		_statesList = LoadEnum();
		_fold = _statesList.Count < 1;
		_showAll = _statesList.Count > 0;
		_fileExists = File.Exists(GetPath());
	}

	public override void OnInspectorGUI()
	{
		if (!_fileExists)
		{
			if (GUILayout.Button("Create Enum"))
				BuildEnum();

			return;
		}

		serializedObject.Update();

		if (_fold = EditorGUILayout.Foldout(_fold, "State Definitions"))
		{
			int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size", _statesList.Count));
			while (newCount < _statesList.Count)
				_statesList.RemoveAt(_statesList.Count - 1);
			while (newCount > _statesList.Count)
				_statesList.Add(null);

			for (int i = 0; i < _statesList.Count; i++)
			{
				_statesList[i] = EditorGUILayout.TextField($"State {i}", _statesList[i]);
			}

			if (_statesList.Count > 0 && GUILayout.Button("Update Info"))
				BuildEnum();
		}

		if (_showAll)
		{
			EditorGUILayout.Space(20);
			base.OnInspectorGUI();
		}
	}

	private void BuildEnum()
	{
		StringBuilder str = new StringBuilder();
		str.AppendLine($"public enum {ENUM_NAME}\n{{");

		if (_statesList.Count > 0)
		{
			for (int i = 0; i < _statesList.Count - 1; i++)
			{
				if (!string.IsNullOrEmpty(_statesList[i]))
					str.AppendLine($"{_statesList[i]},");
			}
			if (!string.IsNullOrEmpty(_statesList[_statesList.Count - 1]))
				str.AppendLine($"{_statesList[_statesList.Count - 1]}");
		}

		str.AppendLine(@"}");


		string path = GetPath();
		if (!string.IsNullOrEmpty(path))
		{
			File.WriteAllText(path, str.ToString());

			AssetDatabase.Refresh();
		}
	}

	private List<string> LoadEnum()
	{
		StreamReader fileReader = null;
		string path = GetPath();

		if (File.Exists(path))
		{
			fileReader = new StreamReader(path);

			string text = string.Concat(fileReader.ReadToEnd().Split('{')[1].Split('}')[0].Where(c => !char.IsWhiteSpace(c)));
			string[] lines = text.Split(',');

			List<string> enums = new List<string>();
			for (int i = 0; i < lines.Length; i++)
			{
				if (!string.IsNullOrEmpty(lines[i]))
					enums.Add(lines[i]);
			}

			fileReader.Close();

			return enums;
		}

		return new List<string>();
	}

	private string GetPath()
	{
		//string[] assets = AssetDatabase.FindAssets($"t: Script {typeof(MenuStateManager)}");
		//for (int i = 0; i < assets.Length; i++)
		//{
		//	string path = AssetDatabase.GUIDToAssetPath(assets[i]);
		//	if (path.Contains($"{typeof(MenuStateManager)}.cs"))
		//	{
		//		return path.Replace($"{typeof(MenuStateManager)}", ENUM_NAME);
		//	}
		//}

		string[] res = System.IO.Directory.GetFiles(Application.dataPath, $"{typeof(MenuStateManager)}.cs", SearchOption.AllDirectories);
		return res[0].Replace($"{typeof(MenuStateManager)}", ENUM_NAME).Replace("\\", "/");
	}
}
