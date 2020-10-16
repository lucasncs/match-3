using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
	[Tooltip("How much time the round will last (in seconds).")]
	[SerializeField] private int _roundDuration = 120;

	[Tooltip("Base points goal. The overall goal will grow with each round, this is just the initial value.")]
	[SerializeField] private int _basePoitsGoal = 1500;

	public int RoundDuration => _roundDuration;
	public int BaseScoreGoal => _basePoitsGoal;
}
