using UnityEngine;

[CreateAssetMenu(menuName = "Data/Piece Data")]
public class PieceData : ScriptableObject
{
	[SerializeField] private PieceType _type;
	[SerializeField] private Sprite _sprite;

	public PieceType Type => _type;
	public Sprite Sprite => _sprite;
}
