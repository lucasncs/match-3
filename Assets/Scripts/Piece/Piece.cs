using UnityEngine;

public class Piece : MonoBehaviour
{
#if CSHARP_7_3_OR_NEWER && UNITY_2020
	[field: SerializeField] public MovablePiece Movable { get; private set; }
	[field: SerializeField] public DestructablePiece Destructable { get; private set; }
#else
	[SerializeField] private MovablePiece _movable;
	[SerializeField] private DestructablePiece _destructable;
	public MovablePiece Movable => _movable;
	public DestructablePiece Destructable => _destructable;
#endif

	public int X { get; private set; }
	public int Y { get; private set; }

	public PieceData Data { get; private set; }
	public GridController Grid { get; private set; }

	public bool IsMovable => Movable;
	public bool IsDestructable => Destructable;


	public void Pressed()
	{
		Grid.PressedPiece(this);
	}

	public void Released()
	{
		Grid.ReleasedPiece(this);
	}

	public void ChangeCoordinates(int x, int y)
	{
		if (!IsMovable) return;

		X = x;
		Y = y;

		name = $"Piece [{x}, {y}] - [{Data.name}]";
	}

	public bool IsMatch(Piece piece)
	{
		return Data == piece.Data;
	}

	public Piece Init(int x, int y, PieceData pieceData, GridController grid)
	{
		X = x;
		Y = y;
		Data = pieceData;
		Grid = grid;

		name = $"Piece [{x}, {y}] - [{Data.name}]";

		GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;

		return this;
	}

	private void OnValidate()
	{
#if CSHARP_7_3_OR_NEWER && UNITY_2020
		if (Movable == null)
			Movable = GetComponent<MovablePiece>();

		if (Destructable == null)
			Destructable = GetComponent<DestructablePiece>();
#else
		if (_movable == null)
			_movable = GetComponent<MovablePiece>();

		if (_destructable == null)
			_destructable = GetComponent<DestructablePiece>();
#endif
	}
}
