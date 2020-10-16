using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
public class CanvasObject : MonoBehaviour
{
	[SerializeField, HideInInspector] private Canvas _canvas;

	[SerializeField] private UnityEvent OnEnabled;
	[SerializeField] private UnityEvent OnDisabled;

	public bool activeInHierarchy => _canvas.enabled && enabled && gameObject.activeInHierarchy;


	public void SetActive(bool _value)
	{
		/// if the _value is diferent than the current one
		if (_canvas.enabled != _value)
		{
			if (_value)
				OnEnabled.Invoke();
			else
				OnDisabled.Invoke();
		}

		_canvas.enabled = _value;

		if (!enabled || !gameObject.activeInHierarchy)
		{
			enabled = true;
			gameObject.SetActive(true);
		}
	}

	private void OnValidate()
	{
		_canvas = GetComponent<Canvas>();
	}


#if UNITY_EDITOR
	[ContextMenu("On | Off")]
	private void Activate()
	{
		UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "CanvasObject Activation");
		UnityEditor.EditorUtility.SetDirty(this);
		SetActive(!activeInHierarchy);
	}
#endif
}
