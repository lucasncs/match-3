using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Camera _cam;

	private RaycastHit2D _lastDown;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (CastRay(eventData, out RaycastHit2D hit))
		{
			_lastDown = hit;
			//if (_down.transform.TryGetComponent(out IInteractableDown interactable))
			//	interactable.OnInteractionDown();

			IInteractableDown interactable = _lastDown.transform.GetComponentInChildren<IInteractableDown>();
			if (interactable != null)
				interactable.OnInteractionDown();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IInteractableUp interactable;
		if (CastRay(eventData, out RaycastHit2D hit))
		{
			//if (hit.transform.TryGetComponent(out IInteractableUp interactable))
			//	interactable.OnInteractionUp();

			//if (_down && _down.transform != hit.transform && _down.transform.TryGetComponent(out interactable))
			//	interactable.OnInteractionUp();

			if ((interactable = hit.transform.GetComponentInChildren<IInteractableUp>()) != null)
				interactable.OnInteractionUp();

		}

		if (_lastDown && _lastDown.transform != hit.transform
			&& (interactable = _lastDown.transform.GetComponentInChildren<IInteractableUp>()) != null)
			interactable.OnInteractionUp();
	}


	//private bool CastRay(PointerEventData eventData, out RaycastHit hit)
	//{
	//	var ray = _cam.ScreenPointToRay(eventData.position);
	//	return Physics.Raycast(ray, out hit);
	//}
	private bool CastRay(PointerEventData eventData, out RaycastHit2D hit)
	{
		var ray = _cam.ScreenPointToRay(eventData.position);
		return hit = Physics2D.GetRayIntersection(ray);
	}

	private void Awake()
	{
		if (_cam == null)
		{
			_cam = Camera.main;
			Debug.LogWarning("Player Input's Camera is not set. Getting Camera with \"MainCamera\" Tag", gameObject);
		}
	}

	private void OnValidate()
	{
		if (_cam == null)
		{
			Debug.LogWarning("Player Input's Camera is not set.", gameObject);
		}
	}
}
