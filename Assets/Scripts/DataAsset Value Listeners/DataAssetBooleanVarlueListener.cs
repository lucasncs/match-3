using UnityEngine;
using UnityEngine.Events;

public class DataAssetBooleanVarlueListener : MonoBehaviour
{
	[SerializeField] private BooleanDataAsset _dataObject;
	[SerializeField] private bool _invertValue;

	public UnityEventBoolean OnValueChange;

	private void OnEnable()
	{
		_dataObject.OnValueChange += ReceiveValue;
		ReceiveValue(_dataObject.Value);
	}
	private void OnDisable()
	{
		_dataObject.OnValueChange -= ReceiveValue;
	}

	private void ReceiveValue(bool value)
	{
		OnValueChange.Invoke(value ^ _invertValue);
	}
}
