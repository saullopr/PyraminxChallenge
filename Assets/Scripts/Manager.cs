using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour {
    public int _height = 3;
    public GameObject _tetrahedronPrefab;

    [SerializeField] private float _rotationAngle = 36.9f;

    [Header("Generation")]
    [SerializeField]
    private int _targetScale;

    [SerializeField] private bool _regenerate = true;

    private readonly float F = Mathf.Sqrt(0.75f);
    private readonly float G = 0.28868f;

    private void Awake() {
        Generate(_targetScale);
    }

    private void Update() {
        if (_regenerate) {
            _regenerate = false;
            
            Generate(_targetScale);
        }
    }

    private void Generate(int targetScale) {
        var prev = BuildTetrahedron(_tetrahedronPrefab, 1);

        for (var scale = 2; scale <= targetScale; scale += scale) {
            var tetrahedron = BuildTetrahedron(prev.gameObject, scale);

            prev.gameObject.SetActive(false);
            prev = tetrahedron;
        }
    }
    
    private CompositeTetrahedron BuildTetrahedron(GameObject baseGo, int scale) {
        var upFacing = GenerateUpFacingTetrahedrons(baseGo, scale).ToList();
        var downFacing = GenerateDownFacingTetrahedrons(upFacing[1].GetComponent<ITetrahedron>()).ToList();

        var compositeTetrahedron = new GameObject($"Composite Tetrahedron - scale {scale}").AddComponent<CompositeTetrahedron>();
        compositeTetrahedron.gameObject.AddComponent<TetrahedronDebugger>();
        compositeTetrahedron.SetChildren(upFacing, downFacing);

        return compositeTetrahedron;
    }

    private IEnumerable<GameObject> GenerateUpFacingTetrahedrons(GameObject baseGo, int scale) {
        var positions = GetUpFacePositions(_height, scale);

        var i = 0;
        foreach (var (pos, rotation) in positions) {
            var tetrahedron = Instantiate(baseGo, pos, rotation);
            
            tetrahedron.name = $"Tetra {i++}";
            yield return tetrahedron;
        }
    }

    private IEnumerable<GameObject> GenerateDownFacingTetrahedrons(ITetrahedron top) {
        var positions = new Vector3[] {
            (top.P0 + top.P1) / 2,
            (top.P0 + top.P2) / 2,
            (top.P1 + top.P2) / 2,
        };

        var i = 0;
        foreach (var pos in positions) {
            var parent = new GameObject {
                transform = {
                    position = pos,
                    rotation = Quaternion.Euler(0, i++ * 120, 0) 
                }
            };

            var copy = Instantiate(top.GameObject, parent.transform, true);

            parent.transform.rotation = Quaternion.Euler(_rotationAngle, parent.transform.rotation.eulerAngles.y, 180);
            copy.transform.parent = null;
            
            Destroy(parent);

            yield return copy;
        }
    }

    private IEnumerable<(Vector3, Quaternion)> GetUpFacePositions(int max, int scale) {
        for (var x = 0; x < max; x++) {
            for (var z = 0; z < max - x; z++) {
                for (var y = 0; y < max - x - z; y++) {
                    var pos = new Vector3(x + (z * .5f) + (y * .5f), y * F, z * F + y * G);
                    
                    yield return (pos * scale, Quaternion.identity);
                }
            }
        }
    }
}
