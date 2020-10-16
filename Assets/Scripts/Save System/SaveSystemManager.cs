using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
	private void Awake()
	{
		SaveSystem.Load();
		DontDestroyOnLoad(gameObject);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
			SaveSystem.Save();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
			SaveSystem.Save();
	}

	private void OnApplicationQuit()
	{
		SaveSystem.Save();
	}
}
