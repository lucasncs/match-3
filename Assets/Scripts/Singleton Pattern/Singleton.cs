using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	[Tooltip("If {false} the object will be destroyed when changing scenes.")]
	[SerializeField] protected bool _dontDestroyOnLoad = false;

	/// <summary>
	/// Returns a singleton class instance.
	/// If current instance is not assigned it will try to find an object of the instance type,
	/// in case instance already exists on a scene. If not, new instance will be created.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if (_instance == null)
				{
					_instance = new GameObject(typeof(T).Name).AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			if (_dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		_instance = null;
	}
}