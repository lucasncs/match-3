using UnityEngine;

public class MenuStateManager : Singleton<MenuStateManager>
{
	[SerializeField, Space] private UIState[] _states;
	[SerializeField, Space] private int _startingState;

	public MenuState CurrentState => _states[CurrentStateIndex].state;
	public int CurrentStateIndex { get; private set; }

	private void Start()
	{
		ChangeState(_startingState);
	}

	public void ChangeState(int stateIndex)
	{
		if (stateIndex >= 0 && stateIndex < _states.Length)
		{
			CurrentStateIndex = stateIndex;
			ApplyState(_states[stateIndex]);
		}
		else
			Debug.LogError($"Trying to change to an inexistant State!\nState will stay the same. input {{{stateIndex}}}", gameObject);
	}
	public void ChangeState(MenuState state)
	{
		for (int i = 0; i < _states.Length; i++)
		{
			if (_states[i].state == state)
				ChangeState(i);
		}
	}

	private void ApplyState(UIState newState)
	{
		for (int i = 0; i < newState.disabled.Length; i++)
			newState.disabled[i].SetActive(false);

		for (int i = 0; i < newState.enabled.Length; i++)
			newState.enabled[i].SetActive(true);
	}

	[System.Serializable]
	public struct UIState
	{
		public MenuState state;
		public CanvasObject[] enabled;
		public CanvasObject[] disabled;
	}


#if UNITY_EDITOR
	[ContextMenu("Next")]
	private void Next()
	{
		ChangeState(++CurrentStateIndex);
	}
	[ContextMenu("Previous")]
	private void Prev()
	{
		ChangeState(--CurrentStateIndex);
	}
#endif
}
