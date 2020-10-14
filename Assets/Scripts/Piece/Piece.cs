using UnityEngine;

public class Piece : MonoBehaviour
{
	[field: SerializeField] public MovablePiece Movable { get; private set; }
	[field: SerializeField] public DestructablePiece Destructable { get; private set; }

	public int X { get; private set; }
	public int Y { get; private set; }

	public PieceData Data { get; private set; }
	public Grid Grid { get; private set; }

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

	public Piece Init(int x, int y, PieceData pieceData, Grid grid)
	{
		X = x;
		Y = y;
		Data = pieceData;
		Grid = grid;
		name = $"Piece [{x}, {y}] - [{pieceData.name}]";

		if (Data.Type == PieceType.Normal)
			GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;

		return this;
	}

	private void OnValidate()
	{
		if (Movable == null)
			Movable = GetComponent<MovablePiece>();

		if (Destructable == null)
			Destructable = GetComponent<DestructablePiece>();
	}
}
