using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component to attach on objects and set then to listen to a specific Game Event.
/// </summary>
public class GameEventListener : MonoBehaviour
{
	[Tooltip("Event to register with")]
	[SerializeField] private GameEvent _event;

	[Tooltip("Response to invoke when Event is raised")]
	public UnityEvent Response;

	private void OnEnable()
	{
		_event.RegisterListener(this);
	}

	private void OnDisable()
	{
		_event.UnregisterListener(this);
	}

	/// <summary>
	/// Method used by the Game Event to invoke what is setted on Response UnityEvent.
	/// </summary>
	public void OnEventRaised()
	{
		Response.Invoke();
	}
}