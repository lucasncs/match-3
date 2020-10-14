using UnityEngine;

[RequireComponent(typeof(Piece))]
public class MovablePiece : MonoBehaviour
{
	[SerializeField] private Piece _piece;

	private Coroutine _moveCoroutine;

	public void Move(int toX, int toY)
	{
		_piece.ChangeCoordinates(toX, toY);
		transform.localPosition = _piece.Grid.GetCoordinatesWorldPosition(toX, toY);
	}

	public void Move(int toX, int toY, float time)
	{
		if (_moveCoroutine != null)
			StopCoroutine(_moveCoroutine);

		_piece.ChangeCoordinates(toX, toY);

		_moveCoroutine = StartCoroutine(CoroutineAnimation.Animate(
			transform.localPosition,
			_piece.Grid.GetCoordinatesWorldPosition(toX, toY),
			time,
			(p) => transform.localPosition = p));
	}

	private void OnValidate()
	{
		if (_piece == null)
			_piece = GetComponent<Piece>();
	}
}
