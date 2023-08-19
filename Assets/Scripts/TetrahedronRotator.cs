using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrahedronRotator : MonoBehaviour {
    [SerializeField] private CompositeTetrahedron _tetrahedron;

    [SerializeField] private GameObject _plane;
    
    private Vector3 P0 => _tetrahedron.P0;
    private Vector3 P1 => _tetrahedron.P1;
    private Vector3 P2 => _tetrahedron.P2;
    private Vector3 P3 => _tetrahedron.P3;

    [SerializeField] private float _gizmosSize = 0.1f;
    [FormerlySerializedAs("_foo")] [SerializeField] private bool _updatePlanes;

    [SerializeField] private int _levels;
    [SerializeField] private Plane[] _planes = Array.Empty<Plane>();

    private void Start() {
        _tetrahedron = FindObjectOfType<CompositeTetrahedron>();
        
        CreatePlanes();
    }

    private void Update() {
        if (_updatePlanes) {
            _updatePlanes = false;
            
            CreatePlanes();
        }

        // foreach (var plane in _planes) {
        //     if (plane.rotate) {
        //         plane.rotate = false;
        //         Rotate(plane);
        //     }
        // }
    }

    public void Rotate(string input) {
        var strings = input.Split(';');

        Rotate(int.Parse(strings[0]), int.Parse(strings[1]), int.Parse(strings[2]));
    }
    
    public void Rotate(int plane, int level, int direction) {
        Rotate(_planes[plane], level, direction);
    }
    
    private void Rotate(Plane plane, int level, int direction) {
        var planeRotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
        
        var colliders = Physics.OverlapBox(plane.Levels[level], new Vector3(100, 0.05f, 100), planeRotation);

        var go = new GameObject("Rotation plane") {
            transform = {
                position = plane.Levels[level],
                rotation = planeRotation
            }
        };

        foreach (var col in colliders) {
            col.transform.parent = go.transform;
        }
        
        go.transform.DORotate(new Vector3(0.0f, 120.0f * direction, 0.0f), 1.0f, RotateMode.LocalAxisAdd);
    }

    private void CreatePlanes() {
        _planes = new Plane[] {
            new Plane(P0, P1, P2, P3, _levels),
            new Plane(P0, P1, P3, P2, _levels),
            new Plane(P0, P2, P3, P1, _levels),
            new Plane(P1, P2, P3, P0, _levels),
        };
    }

    private void OnDrawGizmos() {
        foreach (var plane in _planes) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(plane.Center, _gizmosSize);
            Gizmos.DrawLine(plane.Center, plane.Center + plane.normal * -500);

            Gizmos.color = Color.green;
            foreach (var level in plane.Levels) {
                Gizmos.DrawSphere(level, _gizmosSize);
            }
        }
    }
}

[Serializable]
public class Plane {
    private const float F = 0.8660254f;

    [SerializeField] public bool rotate;
    
    public Vector3 Center { get; private set; }
    public Vector3 normal;
    public List<Vector3> Levels { get; private set; } = new List<Vector3>();

    public Plane(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int levels) {
        Center = (a + b + c) / 3;

        // normal = Vector3.Cross(b - a, c - a).normalized;
        normal = Vector3.Normalize(d - Center) * -1;

        for (var i = 0; i < levels; i++) {
            var level = Center + normal * ((i + .5f) * -F);
            
            Levels.Add(level);
        }
    }
}
