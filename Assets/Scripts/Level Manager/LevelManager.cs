using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
	[Header("Level Configs")]
	[SerializeField] private LevelData _levelData;
	[SerializeField] private IntegerDataAsset _currentRound;
	[SerializeField] private float _pointsGoalMultiplier = 1.2f;
	[SerializeField] private TimerTMP _timer;
	[SerializeField] private GridController _gridController;
	[SerializeField] private ScoreManager _scoreManager;

	protected override void Awake()
	{
		base.Awake();
		PrepareGame();
	}

	public void PrepareGame()
	{
		_gridController.CreateGrid();
	}

	public void StartGame()
	{
		_timer.StartTimer(_levelData.RoundDuration);
	}

	public void RestartGame()
	{
		_gridController.RecreateGrid();
		StartGame();
	}

	public void EndGame()
	{
		if (_scoreManager.CurrentScore >= GetScoreGoal())
		{
			GameManager.Instance.PlayerVictory();
		}
		else
		{
			GameManager.Instance.PlayerLose();
		}
	}

	public int GetScoreGoal()
	{
		return (int)(_levelData.BaseScoreGoal * (_pointsGoalMultiplier + _currentRound.Value - 1));
	}
}
