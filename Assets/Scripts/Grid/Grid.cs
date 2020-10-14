using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	[Header("Grid Dimentions")]
	[SerializeField] private int _xSize = 1;
	[SerializeField] private int _ySize = 1;
	[SerializeField] private Vector2 _cellOffset;

	[Header("Movimentation Speeds")]
	[SerializeField] private float _fillTimeStep;

	[Header("Prefabs")]
	[SerializeField] private Piece _piecePrefab;

	[Header("Pieces Type Data Collections")]
	[SerializeField] private PieceDataCollection _dataCollection;

	private Piece[,] _pieces;

	private Piece _pressedPiece;
	private Piece _releasedPiece;


	private void Awake()
	{
		CreateGrid();
	}

	private void CreateGrid()
	{
		Piece CheckIfCreatedMatches(Piece p)
		{
			if (GetMatch(p, p.X, p.Y) != null)
			{
				p.Init(p.X, p.Y, _dataCollection.GetRandom(), this);
				CheckIfCreatedMatches(p);
			}
			return p;
		}

		_pieces = new Piece[_xSize, _ySize];
		for (int x = 0; x < _xSize; x++)
		{
			for (int y = 0; y < _ySize; y++)
			{
				_pieces[x, y] = CheckIfCreatedMatches(SpawnPiece(x, y));
			}
		}
	}

	private IEnumerator Fill()
	{
		bool movedPiece;

		void Step()
		{
			movedPiece = false;

			for (int x = 0; x < _xSize; x++)
			{
				for (int y = _ySize - 2; y >= 0; y--) // Skip the bottom row
				{
					var piece = _pieces[x, y];

					if (piece != null && piece.IsMovable)
					{
						var pieceBelow = _pieces[x, y + 1];

						if (pieceBelow == null)
						{
							piece.Movable.Move(x, y + 1, _fillTimeStep);

							_pieces[x, y + 1] = piece;
							_pieces[x, y] = null;

							movedPiece = true;
						}
					}
				}
			}

			for (int x = 0; x < _xSize; x++)
			{
				var pieceBelow = _pieces[x, 0];

				if (pieceBelow == null)
				{
					var newPiece = SpawnPiece(x, -1);
					newPiece.Movable.Move(x, 0, _fillTimeStep);
					_pieces[x, 0] = newPiece;

					movedPiece = true;
				}
			}
		}

		do
		{
			yield return new WaitForSeconds(_fillTimeStep);

			do
			{
				Step();

				yield return new WaitForSeconds(_fillTimeStep);

			} while (movedPiece);

		} while (DestroyAllValidMatches());
	}

	private Piece SpawnPiece(int x, int y)
	{
		return Instantiate(_piecePrefab, GetCoordinatesWorldPosition(x, y), Quaternion.identity, transform)
							.Init(x, y, _dataCollection.GetRandom(), this);
	}

	public Vector2 GetCoordinatesWorldPosition(int x, int y)
	{
		return new Vector2(transform.position.x - _xSize * .5f + x + .5f,
			transform.position.y + _ySize * .5f - y - .5f) * _cellOffset;
	}

	public void PressedPiece(Piece piece)
	{
		_pressedPiece = piece;
	}

	public void ReleasedPiece(Piece piece)
	{
		_releasedPiece = piece;

		if (IsAdjacent(_pressedPiece, _releasedPiece))
			SwapPieces(_pressedPiece, _releasedPiece);
	}

	private bool IsAdjacent(Piece piece1, Piece piece2)
	{
		return (piece1.X == piece2.X && Mathf.Abs(piece1.Y - piece2.Y) == 1)
			|| (piece1.Y == piece2.Y && Mathf.Abs(piece1.X - piece2.X) == 1);
	}

	private void SwapPieces(Piece piece1, Piece piece2)
	{
		if (piece1.IsMovable && piece2.IsMovable && !piece1.IsMatch(piece2))
		{
			_pieces[piece1.X, piece1.Y] = piece2;
			_pieces[piece2.X, piece2.Y] = piece1;

			List<Piece> matches1 = GetMatch(piece1, piece2.X, piece2.Y);
			List<Piece> matches2 = GetMatch(piece2, piece1.X, piece1.Y);

			if (matches1 != null || matches2 != null)
			{
				int p1X = piece1.X;
				int p1Y = piece1.Y;

				piece1.Movable.Move(piece2.X, piece2.Y, _fillTimeStep);
				piece2.Movable.Move(p1X, p1Y, _fillTimeStep);

				if (matches1 != null)
					foreach (var p in matches1)
						DestroyPiece(p.X, p.Y);

				if (matches2 != null)
					foreach (var p in matches2)
						DestroyPiece(p.X, p.Y);

				StartCoroutine(Fill());
			}
			else
			{
				_pieces[piece1.X, piece1.Y] = piece1;
				_pieces[piece2.X, piece2.Y] = piece2;
			}
		}
	}

	private List<Piece> GetMatch(Piece piece, int newX, int newY)
	{
		if (piece.Data.Type == PieceType.Normal)
		{
			List<Piece> horizontalPieces = new List<Piece>();
			List<Piece> verticalPieces = new List<Piece>();
			List<Piece> matchingPieces = new List<Piece>();

			void Check(ref List<Piece> list, int nX, int nY, bool isHorizontal)
			{
				list.Add(piece);

				int length = isHorizontal ? _xSize : _ySize;
				for (int dir = 0; dir <= 1; dir++)
				{
					for (int xOffset = 1; xOffset < length; xOffset++)
					{
						int x = dir == 0 ? nX - xOffset : nX + xOffset; // Left : Right | Up : Down

						if (x < 0 || x >= (isHorizontal ? _xSize : _ySize))
							break;

						Piece lookupPiece = isHorizontal ? _pieces[x, nY] : _pieces[nY, x];

						if (lookupPiece != null && piece.IsMatch(lookupPiece))
							list.Add(lookupPiece);
						else
							break;
					}
				}

				if (list.Count >= 3)
					matchingPieces.AddRange(list);

				// Traverse the oposite orientation if we found a match (for L and T shapes)
				if (list.Count >= 3)
				{
					length = isHorizontal ? _ySize : _xSize;
					for (int i = 0; i < list.Count; i++)
					{
						for (int dir = 0; dir <= 1; dir++)
						{
							for (int yOffset = 1; yOffset < length; yOffset++)
							{
								int y = dir == 0 ? nY - yOffset : nY + yOffset; // Left : Right | Up : Down

								if (y < 0 || y >= (isHorizontal ? _xSize : _ySize))
									break;

								Piece lookupPiece = isHorizontal ? _pieces[list[i].X, y] : _pieces[y, list[i].Y];

								if (lookupPiece != null && piece.IsMatch(lookupPiece))
								{
									if (isHorizontal)
										verticalPieces.Add(lookupPiece);
									else
										horizontalPieces.Add(lookupPiece);
								}
								else
									break;
							}
						}

						if (isHorizontal)
						{
							if (verticalPieces.Count < 2)
								verticalPieces.Clear();
							else
							{
								matchingPieces.AddRange(verticalPieces);

								break;
							}
						}
						else
						{
							if (horizontalPieces.Count < 2)
								horizontalPieces.Clear();
							else
							{
								matchingPieces.AddRange(horizontalPieces);

								break;
							}
						}
					}
				}
			}

			// Check horizontal
			Check(ref horizontalPieces, newX, newY, true);

			if (matchingPieces.Count >= 3)
				return matchingPieces;


			// Didn't find anything going horizontally first,
			// so now check vertically
			horizontalPieces.Clear();
			verticalPieces.Clear();

			Check(ref verticalPieces, newY, newX, false);

			if (matchingPieces.Count >= 3)
				return matchingPieces;
		}

		return null;
	}

	private bool DestroyAllValidMatches()
	{
		bool needsRefill = false;

		for (int x = 0; x < _xSize; x++)
		{
			for (int y = 0; y < _ySize; y++)
			{
				if (_pieces[x, y] != null && _pieces[x, y].IsDestructable)
				{
					List<Piece> match = GetMatch(_pieces[x, y], x, y);

					if (match != null)
					{
						for (int i = 0; i < match.Count; i++)
						{
							if (DestroyPiece(match[i].X, match[i].Y))
								needsRefill = true;
						}
					}
				}
			}
		}

		return needsRefill;
	}

	private bool DestroyPiece(int x, int y)
	{
		Piece piece = _pieces[x, y];
		if (piece.IsDestructable && !piece.Destructable.IsBeingDestroyed)
		{
			piece.Destructable.Destroy();

			_pieces[x, y] = null;

			return true;
		}

		return false;
	}

	private List<Piece> GetAdjacent(Piece piece)
	{
		List<Piece> adjacent = new List<Piece>();

		int x = piece.X;
		int y = piece.Y;

		if (y > 0 && _pieces[x, y - 1] != null)
			adjacent.Add(_pieces[x, y - 1]);

		if (x < _xSize - 1 && _pieces[x + 1, y] != null)
			adjacent.Add(_pieces[x + 1, y]);

		if (y < _ySize - 1 && _pieces[x, y + 1] != null)
			adjacent.Add(_pieces[x, y + 1]);

		if (x > 0 && _pieces[x - 1, y] != null)
			adjacent.Add(_pieces[x - 1, y]);

		return adjacent;
	}

	public Bounds GetBounds()
	{
		Bounds bounds = new Bounds();
		var renderers = GetComponentsInChildren<SpriteRenderer>(true);

		foreach (var item in renderers)
		{
			bounds.Encapsulate(item.bounds);
		}

		return bounds;
	}
}
