using UnityEngine;

public class ScoreTextUpdater : MonoBehaviour
{
	[SerializeField] private TMPro.TextMeshProUGUI textField;
	[SerializeField] private string textFormating = "0000";
	[Space]
	[SerializeField] private UnityEngine.Events.UnityEvent OnChange;

	private void Awake()
	{
		UpdateText(ScoreManager.Instance.CurrentScore);
		ScoreManager.Instance.OnScoreChange += UpdateText;
	}

	protected void UpdateText(int _score)
	{
		textField.text = _score.ToString(textFormating);

		if (_score > 0)
			OnChange.Invoke();
	}
}
