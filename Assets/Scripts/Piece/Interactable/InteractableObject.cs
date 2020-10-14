using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractableDown, IInteractableUp
{
	public UnityEvent OnDown;
	public UnityEvent OnUp;

	public void OnInteractionDown()
	{
		OnDown.Invoke();
	}

	public void OnInteractionUp()
	{
		OnUp.Invoke();
	}
}
