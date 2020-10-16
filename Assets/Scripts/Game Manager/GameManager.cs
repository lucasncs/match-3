using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private IntegerDataAsset _currentLevel;

	[Header("Managers & Controllers")]
	[SerializeField] private LevelManager _levelManager;
	[SerializeField] private MenuStateManager _menuManager;

	public bool IsVictory { get; private set; } = true;


	public void PlayerLose()
	{
		_menuManager.ChangeState(MenuState.GameOver);
		IsVictory = false;
	}

	public void PlayerVictory()
	{
		_menuManager.ChangeState(MenuState.Victory);
		IsVictory = true;
	}

	private static void RunRestartables()
	{
		var restartables = FindInterfaces.Find<IRestartable>();
		for (int i = 0; i < restartables.Count; i++)
		{
			restartables[i].Restart();
		}
	}


	#region UI Controls
	public void PressedStartGame()
	{
		_menuManager.ChangeState(MenuState.Gameplay);

		RunRestartables();

		if (IsVictory)
			_levelManager.StartGame();
		else
			_levelManager.RestartGame();

		IsVictory = false;
	}

	public void PressedNextRound()
	{
		_currentLevel.Value++;

		_menuManager.ChangeState(MenuState.Gameplay);
		IsVictory = false;

		RunRestartables();

		_levelManager.RestartGame();
	}

	public void PressedMenu()
	{
		_menuManager.ChangeState(MenuState.Main);
	}
	#endregion
}
