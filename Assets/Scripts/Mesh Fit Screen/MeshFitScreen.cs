using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshFitScreen : MonoBehaviour
{
	private void Start()
	{
		Vector3[] vertices = new Vector3[4];
		Vector2[] uv = new Vector2[4];
		int[] triangles = new int[6];

		Camera cam = Camera.main;
		Vector2 bottomLeft = cam.ScreenToWorldPoint(Vector2.zero);
		Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

		vertices[0] = new Vector3(bottomLeft.x, topRight.y);
		vertices[1] = topRight;
		vertices[2] = bottomLeft;
		vertices[3] = new Vector3(topRight.x, bottomLeft.y);

		uv[0] = new Vector2(0, 1);
		uv[1] = new Vector2(1, 1);
		uv[2] = new Vector2(0, 0);
		uv[3] = new Vector2(1, 0);

		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 2;
		triangles[4] = 1;
		triangles[5] = 3;

		GetComponent<MeshFilter>().mesh = new Mesh
		{
			vertices = vertices,
			uv = uv,
			triangles = triangles
		};
	}
}
