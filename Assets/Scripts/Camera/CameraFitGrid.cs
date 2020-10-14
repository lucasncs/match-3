using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFitGrid : MonoBehaviour
{
	[SerializeField] private Grid _grid;
	[SerializeField] private float _sizeOffset = 1;

	private void Start()
	{
		Bounds gridBounds = _grid.GetBounds();
		gridBounds.Expand(_sizeOffset);

		float screenRatio = (float)Screen.width / Screen.height;
		float targetRatio = gridBounds.size.x / gridBounds.size.y;

		if (screenRatio >= targetRatio)
		{
			GetComponent<Camera>().orthographicSize = gridBounds.size.y * .5f;
		}
		else
		{
			float differenceInSize = targetRatio / screenRatio;
			GetComponent<Camera>().orthographicSize = gridBounds.size.y * .5f * differenceInSize;
		}
	}

	private void Reset()
	{
		if (_grid == null)
			_grid = FindObjectOfType<Grid>();
	}
}
