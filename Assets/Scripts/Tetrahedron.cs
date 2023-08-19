using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Tetrahedron : MonoBehaviour, ITetrahedron {
    private MeshFilter meshFilter;

    public Vector3 p0 = new Vector3(0, 0, 0);
    public Vector3 p1 = new Vector3(1, 0, 0);
    public Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
    public Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
    Mesh mesh;

    public Vector3 P0 => p0 + transform.position;
    public Vector3 P1 => p1 + transform.position;
    public Vector3 P2 => p2 + transform.position;
    public Vector3 P3 => p3 + transform.position;
    
    public GameObject GameObject => gameObject;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start() {
        Rebuild();
    }

    private void Rebuild() {
        mesh = meshFilter.sharedMesh;
        if (mesh == null) {
            meshFilter.mesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }

        mesh.Clear();
        mesh.vertices = new Vector3[] {
            p0, p1, p2,
            p0, p2, p3,
            p2, p1, p3,
            p0, p3, p1
        };
        
        mesh.triangles = new int[] {
            0, 1, 2,
            3, 4, 5,
            6, 7, 8,
            9, 10, 11
        };

        var uv0 = new Vector2(0, 0);
        var uv1 = new Vector2(1, 0);
        var uv2 = new Vector2(0.5f, 1);

        mesh.uv = new Vector2[] {
            uv0, uv1, uv2,
            uv1, uv0, uv1,
            uv2, uv1, uv0,
            uv1, uv2, uv0
        };

        var colors = new Color[mesh.vertices.Length];
        colors[0] = colors[1] = colors[2] = Color.blue;
        colors[3] = colors[4] = colors[5] = Color.red;
        colors[6] = colors[7] = colors[8] = Color.yellow;
        colors[9] = colors[10] = colors[11] = Color.magenta;

        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
