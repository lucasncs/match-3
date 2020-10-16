using UnityEngine;

public class DisplayIntegerDataAsset : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text _textField;
	[SerializeField] private IntegerDataAsset _dataItem;
	[SerializeField] private int _valueModifier = 0;

	private void Awake()
	{
		UpdateText(_dataItem.Value);
	}

	private void OnEnable()
	{
		_dataItem.OnValueChange += UpdateText;
	}
	private void OnDisable()
	{
		_dataItem.OnValueChange -= UpdateText;
	}

	private void UpdateText(int value)
	{
		_textField.text = (value + _valueModifier).ToString(); ;
	}
}
