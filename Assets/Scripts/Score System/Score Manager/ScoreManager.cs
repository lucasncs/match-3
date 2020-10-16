using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>, IRestartable
{
	[SerializeField] private int _pointsPerPiece = 100;
	[SerializeField] private IntegerDataAsset _highScore;
	public int CurrentScore { get; private set; }
	public int HighScore => _highScore.Value;

	public bool isNewHigh { get; private set; }

	public event Action<int> OnScoreChange;
	public event Action<int> OnHighScoreChange;

	private void Start()
	{
		OnScoreChange?.Invoke(CurrentScore);
		OnHighScoreChange?.Invoke(HighScore);
	}

	public void SubmitPoints(int pieces)
	{
		CurrentScore += _pointsPerPiece * pieces;
		OnScoreChange?.Invoke(CurrentScore);

		if (CurrentScore > HighScore)
		{
			isNewHigh = true;
			_highScore.Value = CurrentScore;
			OnHighScoreChange?.Invoke(CurrentScore);
		}
	}

	public void Restart()
	{
		CurrentScore = 0;
		OnScoreChange?.Invoke(CurrentScore);

		isNewHigh = false;
	}
}
