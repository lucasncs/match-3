using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Animation played using Coroutines
/// </summary>
public static class CoroutineAnimation
{
	/// <summary>
	/// Animate a value linearly.
	/// </summary>
	/// <param name="from">The initial value</param>
	/// <param name="to">The final value</param>
	/// <param name="duration">The amount of time, in seconds, to get from the initial value to the final value</param>
	/// <param name="setValue">The value modification expression that will be executed</param>
	/// <param name="curve">The curve that defines how the value will be modified</param>
	/// <returns>The IEnumerator structure of the animation to be used as a Coroutine</returns>
	public static IEnumerator Animate(Vector3 from, Vector3 to, float duration, Action<Vector3> setValue)
	{
		yield return AnimateOnce(from, to, duration, setValue, AnimationCurve.Linear(0, 0, 1, 1));
	}

	/// <summary>
	/// Animate a value using a curve.
	/// </summary>
	/// <param name="from">The initial value</param>
	/// <param name="to">The final value</param>
	/// <param name="duration">The amount of time, in seconds, to get from the initial value to the final value</param>
	/// <param name="setValue">The value modification expression that will be executed</param>
	/// <param name="curve">The curve that defines how the value will be modified</param>
	/// <returns>The IEnumerator structure of the animation to be used as a Coroutine</returns>
	public static IEnumerator Animate(Vector3 from, Vector3 to, float duration, Action<Vector3> setValue, AnimationCurve curve)
	{
		if (curve.postWrapMode == WrapMode.Loop)
			yield return AnimateLoop(from, to, duration, setValue, curve);
		else if (curve.postWrapMode == WrapMode.PingPong)
			yield return AnimatePingPong(from, to, duration, setValue, curve);
		else
			yield return AnimateOnce(from, to, duration, setValue, curve);

	}

	/// <summary>
	/// Simple straight forward animation, with no looping.
	/// </summary>
	/// <param name="from">The initial value</param>
	/// <param name="to">The final value</param>
	/// <param name="duration">The amount of time, in seconds, to get from the initial value to the final value</param>
	/// <param name="setValue">The value modification expression that will be executed</param>
	/// <param name="curve">The curve that defines how the value will be modified</param>
	/// <returns>The IEnumerator structure of the animation to be used as a Coroutine</returns>
	public static IEnumerator AnimateOnce(Vector3 from, Vector3 to, float duration, Action<Vector3> setValue, AnimationCurve curve)
	{
		float time = 0;

		while (time < 1)
		{
			time += Time.fixedDeltaTime / duration;
			setValue(Vector3.LerpUnclamped(from, to, curve.Evaluate(time)));

			yield return new WaitForFixedUpdate();
		}
	}

	/// <summary>
	/// Infinite loop animation that will begin again from the initial value once it reaches the final value.
	/// </summary>
	/// <param name="from">The initial value</param>
	/// <param name="to">The final value</param>
	/// <param name="duration">The amount of time, in seconds, to get from the initial value to the final value</param>
	/// <param name="setValue">The value modification expression that will be executed</param>
	/// <param name="curve">The curve that defines how the value will be modified</param>
	/// <returns>The IEnumerator structure of the animation to be used as a Coroutine</returns>
	public static IEnumerator AnimateLoop(Vector3 from, Vector3 to, float duration, Action<Vector3> setValue, AnimationCurve curve)
	{
		while (true)
		{
			yield return AnimateOnce(from, to, duration, setValue, curve);
		}
	}

	/// <summary>
	/// Infinite ping-pong loop animation that will begin again from the final value once it reaches it, and then invert with the initial.
	/// </summary>
	/// <param name="from">The initial value</param>
	/// <param name="to">The final value</param>
	/// <param name="duration">The amount of time, in seconds, to get from the initial value to the final value</param>
	/// <param name="setValue">The value modification expression that will be executed</param>
	/// <param name="curve">The curve that defines how the value will be modified</param>
	/// <returns>The IEnumerator structure of the animation to be used as a Coroutine</returns>
	public static IEnumerator AnimatePingPong(Vector3 from, Vector3 to, float duration, Action<Vector3> setValue, AnimationCurve curve)
	{
		while (true)
		{
			yield return AnimateOnce(from, to, duration, setValue, curve);

			Vector3 temp = from;
			from = to;
			to = temp;
		}
	}

	/// <summary>
	/// Attachment to a IEnumerator that enables an action be executed when the IEnumerator reaches its end.
	/// </summary>
	/// <param name="e">The original IEnumerator</param>
	/// <param name="onCompleteAction">Action to execute</param>
	/// <returns></returns>
	public static IEnumerator ExecuteOnComplete(this IEnumerator e, Action onCompleteAction)
	{
		yield return e;

		onCompleteAction();
	}
}
