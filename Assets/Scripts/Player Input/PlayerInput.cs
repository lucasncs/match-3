using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Camera _cam;

	private RaycastHit2D _lastDown;
	private bool _cancelInput;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!_cancelInput && CastRay(eventData.position, out RaycastHit2D hit))
		{
			_lastDown = hit;

			IInteractableDown interactable = _lastDown.transform.GetComponentInChildren<IInteractableDown>();
			if (interactable != null)
				interactable.OnInteractionDown();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		RaycastHit2D hit = new RaycastHit2D();
		IInteractableUp interactable;
		if (!_cancelInput && CastRay(eventData.position, out hit))
		{
			if ((interactable = hit.transform.GetComponentInChildren<IInteractableUp>()) != null)
				interactable.OnInteractionUp();
		}

		if (_lastDown && (!hit || _lastDown.transform != hit.transform)
			&& (interactable = _lastDown.transform.GetComponentInChildren<IInteractableUp>()) != null)
			interactable.OnInteractionUp();
	}

	public void SetCancelInput(bool cancelInput)
	{
		_cancelInput = cancelInput;
	}

	private bool CastRay(Vector2 origin, out RaycastHit2D hit)
	{
		return hit = Physics2D.GetRayIntersection(_cam.ScreenPointToRay(origin));
	}

	private void Awake()
	{
		if (_cam == null)
		{
			_cam = Camera.main;
			Debug.LogWarning("Player Input's Camera is not set. Getting Camera with \"MainCamera\" Tag", gameObject);
		}
	}
}
