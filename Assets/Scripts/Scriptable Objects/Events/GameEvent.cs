using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A zero argument callback that is used for decouple systems and at the same time making hooks persistent.
/// </summary>
[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	/// <summary>
	/// The list of listeners that this event will notify if it is raised.
	/// </summary>
	private readonly List<GameEventListener> _eventListeners = new List<GameEventListener>();

	/// <summary>
	/// Invokes the event.
	/// </summary>
	public void Raise()
	{
		for (int i = _eventListeners.Count - 1; i >= 0; i--)
			_eventListeners[i].OnEventRaised();
	}

	/// <summary>
	/// Subscribe a new event listener.
	/// </summary>
	public void RegisterListener(GameEventListener listener)
	{
		if (!_eventListeners.Contains(listener))
			_eventListeners.Add(listener);
	}

	/// <summary>
	/// Unsubscribe a event listener.
	/// </summary>
	public void UnregisterListener(GameEventListener listener)
	{
		if (_eventListeners.Contains(listener))
			_eventListeners.Remove(listener);
	}
}