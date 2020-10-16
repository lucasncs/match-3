using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Performs animations on diferent Transform values.
/// </summary>
public class TransformAnimation : MonoBehaviour
{
	private enum AnimationType
	{
		LocalPosition, LocalRotation, LocalScale, GlobalPosition, GlobalRotation, PositionX, PositionY, PositionZ
	};

	[Tooltip("What should be animated")]
	[SerializeField] private AnimationType _type;

	[Tooltip("The curve that defines how the value will be modified")]
	[SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

	[Tooltip("The starting value")]
	[SerializeField] private Vector3 _initialValue;

	[Tooltip("The ending value")]
	[SerializeField] private Vector3 _finalValue;

	[Tooltip("The amount of seconds to go from the initial value to the final value")]
	[SerializeField] private float _duration = 1f;

	[Tooltip("Delay animation's start (in seconds)")]
	[SerializeField] private float _delay = 0;

	[Tooltip("Start animation as soon as the Component/GameObject is enabled")]
	[SerializeField] private bool _playOnEnable;

	[Space]
	public UnityEvent OnAnimationComplete;

	private Action<Vector3> _setValue;
	private Coroutine _animCoroutine;

	private void Awake()
	{
		// Defines the value modification expression that will be executed in the animation based on the chosen type
		switch (_type)
		{
			case AnimationType.LocalPosition:
				_setValue = (p) => transform.localPosition = p;
				break;
			case AnimationType.LocalRotation:
				_setValue = (r) => transform.localEulerAngles = r;
				break;
			case AnimationType.LocalScale:
				_setValue = (s) => transform.localScale = s;
				break;
			case AnimationType.GlobalPosition:
				_setValue = (p) => transform.position = p;
				break;
			case AnimationType.GlobalRotation:
				_setValue = (r) => transform.eulerAngles = r;
				break;
			case AnimationType.PositionX:
				_setValue = (p) => transform.localPosition = new Vector3(p.x, transform.localPosition.y, transform.localPosition.z);
				break;
			case AnimationType.PositionY:
				_setValue = (p) => transform.localPosition = new Vector3(transform.localPosition.x, p.y, transform.localPosition.z);
				break;
			case AnimationType.PositionZ:
				_setValue = (p) => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, p.z);
				break;
		}
	}

	protected void OnEnable()
	{
		if (_playOnEnable)
			Play();
	}
	private void OnDisable()
	{
		Stop();
	}

	[ContextMenu("Play")]
	public void Play()
	{
		Stop();
		_animCoroutine = StartCoroutine(Animate());
	}

	[ContextMenu("Stop")]
	public void Stop()
	{
		if (_animCoroutine != null)
			StopCoroutine(_animCoroutine);
	}

	private IEnumerator Animate()
	{
		if (_delay > 0)
			yield return new WaitForSeconds(_delay);

		_setValue(_initialValue);

		// Play animation accordingly to the WrapMode defined in the animation curve
		if (_curve.postWrapMode == WrapMode.ClampForever)
			yield return CoroutineAnimation.AnimateOnce(_initialValue, _finalValue, _duration, _setValue, _curve);
		else if (_curve.postWrapMode == WrapMode.Loop)
			yield return CoroutineAnimation.AnimateLoop(_initialValue, _finalValue, _duration, _setValue, _curve);
		else if (_curve.postWrapMode == WrapMode.PingPong)
			yield return CoroutineAnimation.AnimatePingPong(_initialValue, _finalValue, _duration, _setValue, _curve);

		OnAnimationComplete.Invoke();
	}
}
