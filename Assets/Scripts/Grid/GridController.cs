using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridController : MonoBehaviour
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
	[SerializeField] private PieceDataCollection _piecesDataCollection;

	[Header("Events")]
	public UnityEvent OnSelectPiece;
	public UnityEvent OnSwapPieces;
	public UnityEventInteger OnMatchPieces;

	private Piece[][] _pieces;

	private Piece _pressedPiece;


	public Vector2 GetCoordinatesWorldPosition(int x, int y)
	{
		return new Vector2(transform.position.x - _xSize * .5f + x,
			transform.position.y + _ySize * .5f - y) * _cellOffset;
	}

	public void PressedPiece(Piece piece)
	{
		_pressedPiece = piece;
		OnSelectPiece.Invoke();
	}

	public void ReleasedPiece(Piece piece)
	{
		if (IsAdjacent(_pressedPiece, piece))
			SwapPieces(_pressedPiece, piece);
	}

	public void CreateGrid()
	{
		_pieces = new Piece[_xSize][];
		for (int i = 0; i < _xSize; i++)
			_pieces[i] = new Piece[_ySize];

		for (int x = 0; x < _xSize; x++)
		{
			for (int y = 0; y < _ySize; y++)
			{
				_pieces[x][y] = CheckIfCreatedMatches(SpawnPiece(x, y));
			}
		}

		if (!CheckPossibleMatches())
			ShuffleGrid();
	}

	public void RecreateGrid()
	{
		ShuffleGrid();
		StartCoroutine(Fill());
	}

	public void ShuffleGrid()
	{
		for (int x = 0; x < _xSize; x++)
		{
			for (int y = 0; y < _ySize; y++)
			{
				_pieces[x][y] = CheckIfCreatedMatches(_pieces[x][y]
					.Init(x,y, _piecesDataCollection.GetRandom(), this));
			}
		}

		if (!CheckPossibleMatches())
			ShuffleGrid();
	}

	private Piece SpawnPiece(int x, int y)
	{
		return Instantiate(_piecePrefab, GetCoordinatesWorldPosition(x, y), Quaternion.identity, transform)
							.Init(x, y, _piecesDataCollection.GetRandom(), this);
	}

	private IEnumerator Fill()
	{
		bool movedPiece;

		do
		{
			yield return new WaitForSeconds(_fillTimeStep);

			do
			{
				movedPiece = false;

				for (int x = 0; x < _xSize; x++)
				{
					for (int y = _ySize - 2; y >= 0; y--) // Skip the bottom row
					{
						var piece = _pieces[x][y];

						if (piece != null && piece.IsMovable)
						{
							var pieceBelow = _pieces[x][y + 1];

							if (pieceBelow == null)
							{
								piece.Movable.Move(x, y + 1, _fillTimeStep);

								_pieces[x][y + 1] = piece;
								_pieces[x][y] = null;

								movedPiece = true;
							}
						}
					}
				}

				for (int x = 0; x < _xSize; x++)
				{
					var pieceBelow = _pieces[x][0];

					if (pieceBelow == null)
					{
						var newPiece = SpawnPiece(x, -1);
						newPiece.Movable.Move(x, 0, _fillTimeStep);
						_pieces[x][0] = newPiece;

						movedPiece = true;
					}
				}

				yield return new WaitForSeconds(_fillTimeStep);

			} while (movedPiece);

		} while (DestroyAllValidMatches());


		if (!CheckPossibleMatches())
			ShuffleGrid();
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
			_pieces[piece1.X][piece1.Y] = piece2;
			_pieces[piece2.X][piece2.Y] = piece1;

			List<Piece> matches1 = GetMatch(piece1, piece2.X, piece2.Y);
			List<Piece> matches2 = GetMatch(piece2, piece1.X, piece1.Y);

			if (matches1 != null || matches2 != null)
			{
				int p1X = piece1.X;
				int p1Y = piece1.Y;
				int matches = 0;

				piece1.Movable.Move(piece2.X, piece2.Y, _fillTimeStep);
				piece2.Movable.Move(p1X, p1Y, _fillTimeStep);

				if (matches1 != null)
				{
					matches += matches1.Count;
					foreach (var p in matches1)
						DestroyPiece(p.X, p.Y);
				}

				if (matches2 != null)
				{
					matches += matches2.Count;
					foreach (var p in matches2)
						DestroyPiece(p.X, p.Y);
				}

				StartCoroutine(Fill());
				OnMatchPieces.Invoke(matches);
				OnSwapPieces.Invoke();
			}
			else
			{
				_pieces[piece1.X][piece1.Y] = piece1;
				_pieces[piece2.X][piece2.Y] = piece2;
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

			void Check(ref List<Piece> list, ref List<Piece> oppositeList, int nX, int nY, bool isHorizontal)
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

						Piece lookupPiece = isHorizontal ? _pieces[x][nY] : _pieces[nY][x];

						if (lookupPiece != null && piece.IsMatch(lookupPiece))
							list.Add(lookupPiece);
						else
							break;
					}
				}

				if (list.Count >= 3)
					matchingPieces.AddRange(list);

				// Traverse the opposite orientation if found a match (for L and T shapes)
				if (list.Count >= 3)
				{
					length = isHorizontal ? _ySize : _xSize;
					for (int i = 0; i < list.Count; i++)
					{
						for (int dir = 0; dir <= 1; dir++)
						{
							for (int yOffset = 1; yOffset < length; yOffset++)
							{
								int y = dir == 0 ? nY - yOffset : nY + yOffset; // Up : Down | Left : Right

								if (y < 0 || y >= (isHorizontal ? _xSize : _ySize))
									break;

								Piece lookupPiece = isHorizontal ? _pieces[list[i].X][y] : _pieces[y][list[i].Y];

								if (lookupPiece != null && piece.IsMatch(lookupPiece))
								{
									oppositeList.Add(lookupPiece);
								}
								else
									break;
							}
						}

						if (oppositeList.Count < 2)
							oppositeList.Clear();
						else
						{
							matchingPieces.AddRange(oppositeList);

							break;
						}

					}
				}
			}


			// Check horizontal
			Check(ref horizontalPieces, ref verticalPieces, newX, newY, true);

			if (matchingPieces.Count >= 3)
				return matchingPieces;

			// Didn't find anything going horizontally first,
			// so now check vertically
			horizontalPieces.Clear();
			verticalPieces.Clear();

			Check(ref verticalPieces, ref horizontalPieces, newY, newX, false);

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
				if (_pieces[x][y] != null && _pieces[x][y].IsDestructable)
				{
					List<Piece> match = GetMatch(_pieces[x][y], x, y);

					if (match != null)
					{
						for (int i = 0; i < match.Count; i++)
						{
							if (DestroyPiece(match[i].X, match[i].Y))
								needsRefill = true;
						}

						OnMatchPieces.Invoke(match.Count);
					}
				}
			}
		}

		return needsRefill;
	}

	private bool DestroyPiece(int x, int y)
	{
		Piece piece = _pieces[x][y];
		if (piece.IsDestructable && !piece.Destructable.IsBeingDestroyed)
		{
			piece.Destructable.Destroy();

			_pieces[x][y] = null;

			return true;
		}

		return false;
	}

	private bool CheckPossibleMatches()
	{
		for (int x = 0; x < _xSize; x++)
		{
			for (int y = 0; y < _ySize; y++)
			{
				Piece piece = _pieces[x][y];
				List<Piece> transversal;

				if (piece != null
					&& piece.IsMovable
					&& (transversal = GetTransversal(piece)) != null)
				{
					foreach (var transvPiece in transversal)
					{
						if (piece.IsMatch(transvPiece))
						{
							List<Piece> adjacent = GetAdjacent(transvPiece);

							foreach (var adjPiece in adjacent)
							{
								if (piece.IsMatch(adjPiece) && !IsAdjacent(adjPiece, piece))
									return true;
							}

							List<Piece> transversal2 = GetTransversal(transvPiece);

							foreach (var transvPiece2 in transversal2)
							{
								if (piece.IsMatch(transvPiece2) && transvPiece2 != piece && (transvPiece2.X == piece.X || transvPiece2.Y == piece.Y))
									return true;
							}
						}
					}
				}

			}
		}

		return false;
	}

	private Piece CheckIfCreatedMatches(Piece p)
	{
		if (GetMatch(p, p.X, p.Y) != null)
		{
			p.Init(p.X, p.Y, _piecesDataCollection.GetRandom(), this);
			CheckIfCreatedMatches(p);
		}
		return p;
	}

	private List<Piece> GetAdjacent(Piece piece)
	{
		List<Piece> adjacent = new List<Piece>();

		int x = piece.X;
		int y = piece.Y;

		if (y > 0 && _pieces[x][y - 1] != null)
			adjacent.Add(_pieces[x][y - 1]);

		if (x < _xSize - 1 && _pieces[x + 1][y] != null)
			adjacent.Add(_pieces[x + 1][y]);

		if (y < _ySize - 1 && _pieces[x][y + 1] != null)
			adjacent.Add(_pieces[x][y + 1]);

		if (x > 0 && _pieces[x - 1][y] != null)
			adjacent.Add(_pieces[x - 1][y]);

		return adjacent;
	}

	private List<Piece> GetTransversal(Piece piece)
	{
		List<Piece> transversal = new List<Piece>();

		int x = piece.X;
		int y = piece.Y;

		if (x > 0 && y > 0 && _pieces[x - 1][y - 1] != null)
			transversal.Add(_pieces[x - 1][y - 1]);

		if (x < _xSize - 1 && y > 0 && _pieces[x + 1][y - 1] != null)
			transversal.Add(_pieces[x + 1][y - 1]);

		if (x < _xSize - 1 && y < _ySize - 1 && _pieces[x + 1][y + 1] != null)
			transversal.Add(_pieces[x + 1][y + 1]);

		if (x > 0 && y < _ySize - 1 && _pieces[x - 1][y + 1] != null)
			transversal.Add(_pieces[x - 1][y + 1]);

		return transversal;
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
