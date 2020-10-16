using System.Collections;
using UnityEngine;

public class DestructablePiece : MonoBehaviour
{
	[SerializeField] protected Piece _piece;

	public bool IsBeingDestroyed { get; private set; }

	public void Destroy()
	{
		IsBeingDestroyed = true;
		StartCoroutine(DestroyCoroutine());
	}

	private IEnumerator DestroyCoroutine()
	{
		yield return CoroutineAnimation.Animate(
			transform.localScale,
			Vector3.zero,
			.3f,
			(s) => transform.localScale = s);

		Destroy(gameObject);
	}

	private void OnValidate()
	{
		if (_piece == null)
			_piece = GetComponent<Piece>();
	}
}
