public class HighScoreTextUpdater : ScoreTextUpdater
{
	private void Awake()
	{
		UpdateText(ScoreManager.Instance.HighScore);
		ScoreManager.Instance.OnHighScoreChange += UpdateText;
	}
}
