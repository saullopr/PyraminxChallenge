using System.Collections.Generic;
using UnityEngine;

public class CompositeTetrahedron : MonoBehaviour, ITetrahedron {
    [SerializeField] private List<GameObject> _upFacing;
    [SerializeField] private List<GameObject> _downFacing;

    public List<GameObject> UpFacing => _upFacing;
    public List<GameObject> DownFacing => _downFacing;

    public Vector3 P0 => UpFacing[0].GetComponent<ITetrahedron>().P0;
    public Vector3 P1 => UpFacing[3].GetComponent<ITetrahedron>().P1;
    public Vector3 P2 => UpFacing[2].GetComponent<ITetrahedron>().P2;
    public Vector3 P3 => UpFacing[1].GetComponent<ITetrahedron>().P3;

    public GameObject GameObject => gameObject;

    public void SetChildren(List<GameObject> upFacing, List<GameObject> downFacing) {
        _upFacing = upFacing;
        _downFacing = downFacing;

        foreach (var tetrahedron in upFacing) {
            tetrahedron.transform.parent = GameObject.transform;
        }

        foreach (var tetrahedron in downFacing) {
            tetrahedron.transform.parent = GameObject.transform;
        }
    }
}
