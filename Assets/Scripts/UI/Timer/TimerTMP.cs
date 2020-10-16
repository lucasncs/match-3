using System.Collections;
using UnityEngine;

public class TimerTMP : MonoBehaviour
{
	[Tooltip("Time in seconds")]
	[SerializeField] private float time;
	[SerializeField] private TMPro.TextMeshProUGUI textOutput;
	[Tooltip("Use {0} to insert the value. If leaved EMPTY the output will be simply the time")]
	[SerializeField] private string outputFormat = "{0}:{1:00}";

	public bool Finished { get; private set; }

	private bool simplifiedFormat;
	private bool canUpdate;
	private Coroutine updater;

	public event System.Action<TimerData> OnRefreshTime;
	public UnityEngine.Events.UnityEvent OnFinished;


	private void Start()
	{
		simplifiedFormat = string.IsNullOrEmpty(outputFormat);
	}

	[ContextMenu("Start Timer")]
	public void StartTimer()
	{
		StartTimer(time);
	}
	public void StartTimer(float _time, float _refreshTime = 1f)
	{
		Stop();

		canUpdate = true;
		updater = StartCoroutine(Updater(_time, _refreshTime));
	}

	private IEnumerator Updater(float _timer, float _refreshTime)
	{
		float countdown = _timer;

		OnRefreshTime?.Invoke(new TimerData(_timer, countdown));

		UpdateText(_timer);

		while (canUpdate)
		{
			yield return new WaitForSeconds(_refreshTime);

			countdown -= _refreshTime;

			if (countdown <= 0f)
				canUpdate = false;

			OnRefreshTime?.Invoke(new TimerData(_timer, countdown));
			UpdateText(Mathf.Clamp(countdown, 0f, float.PositiveInfinity));
		}

		yield return new WaitForSeconds(.3f);

		Finish();
	}

	private void UpdateText(float _time)
	{
		textOutput.text = simplifiedFormat ? _time.ToString() : string.Format(outputFormat, (int)(_time / 60), (int)(_time % 60));
	}

	private void Finish()
	{
		Finished = true;
		canUpdate = false;

		OnFinished.Invoke();
	}

	public void Stop()
	{
		if (updater != null)
		{
			StopCoroutine(updater);
		}
	}
}

public struct TimerData
{
	public float initialTime;
	public float countdownTime;

	public TimerData(float initialTime, float countdownTime)
	{
		this.initialTime = initialTime;
		this.countdownTime = countdownTime;
	}
}
