using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	[Tooltip("If {false} the object will be destroyed when changing scenes.")]
	[SerializeField] protected bool _dontDestroyOnLoad = false;

	private static bool _applicationIsQuitting = false;

	/// <summary>
	/// Returns a singleton class instance
	/// If current instance is not assigned it will try to find an object of the instance type,
	/// in case instance already exists on a scene. If not, new instance will be created
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
			{
				Debug.LogError($"{typeof(T)} [Singleton] is already destroyed. Returning null.");
				return null;
			}

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
				DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}




	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	/// it will create a buggy ghost object that will stay on the Editor scene
	/// even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	protected virtual void OnDestroy()
	{
		_instance = null;
		_applicationIsQuitting = true;
	}

	protected virtual void OnApplicationQuit()
	{
		_instance = null;
		_applicationIsQuitting = true;
	}
}