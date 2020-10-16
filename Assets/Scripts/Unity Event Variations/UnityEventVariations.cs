using System;

namespace UnityEngine.Events
{
	[Serializable]
	/// <summary>
	/// UnityEvent Boolean variation.
	/// </summary>
	public class UnityEventBoolean : UnityEvent<bool> { }

	[Serializable]
	/// <summary>
	/// UnityEvent float variation.
	/// </summary>
	public class UnityEventFloat : UnityEvent<float> { }

	[Serializable]
	/// <summary>
	/// UnityEvent Integer variation.
	/// </summary>
	public class UnityEventInteger : UnityEvent<int> { }

	[Serializable]
	/// <summary>
	/// UnityEvent String variation.
	/// </summary>
	public class UnityEventString : UnityEvent<string> { }

	[Serializable]
	/// <summary>
	/// UnityEvent Long variation.
	/// </summary>
	public class UnityEventLong : UnityEvent<long> { }
	
	[Serializable]
	/// <summary>
	/// UnityEvent GameObject variation.
	/// </summary>
	public class UnityEventGameObject : UnityEvent<GameObject> { }

	[Serializable]
	/// <summary>
	/// UnityEvent GameObject variation.
	/// </summary>
	public class UnityEventRaycastHit : UnityEvent<RaycastHit> { }
}
