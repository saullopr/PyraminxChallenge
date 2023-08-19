using UnityEngine;

public interface ITetrahedron {
    public Vector3 P0 { get; }
    public Vector3 P1 { get; }
    public Vector3 P2 { get; }
    public Vector3 P3 { get; }

    public GameObject GameObject { get; }
}
