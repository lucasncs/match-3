public class GoalScoreTextUpdater : ScoreTextUpdater, IRestartable
{
	private void Awake()
	{
		UpdateText(LevelManager.Instance.GetScoreGoal());
	}
	public void Restart()
	{
		Awake();
	}
}
