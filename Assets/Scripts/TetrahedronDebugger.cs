using UnityEngine;

[RequireComponent(typeof(ITetrahedron))]
public class TetrahedronDebugger : MonoBehaviour {
    private ITetrahedron _tetrahedron;

    private ITetrahedron Tetrahedron {
        get {
            if (_tetrahedron == null) {
                _tetrahedron = GetComponent<ITetrahedron>();
            }

            return _tetrahedron;
        }
    }

    private Vector3 P0 => Tetrahedron.P0;
    private Vector3 P1 => Tetrahedron.P1;
    private Vector3 P2 => Tetrahedron.P2;
    private Vector3 P3 => Tetrahedron.P3;

    [Range(0, 1)] [SerializeField] private float _gizmosSize = .05f;

    [SerializeField] private bool _showP0;
    [SerializeField] private bool _showP1;
    [SerializeField] private bool _showP2;
    [SerializeField] private bool _showP3;

    [SerializeField] private bool _showCenter;

    private void OnDrawGizmosSelected() {
        if (_showP0) {
            Gizmos.DrawSphere(P0, _gizmosSize);
        }

        if (_showP1) {
            Gizmos.DrawSphere(P1, _gizmosSize);
        }

        if (_showP2) {
            Gizmos.DrawSphere(P2, _gizmosSize);
        }

        if (_showP3) {
            Gizmos.DrawSphere(P3, _gizmosSize);
        }

        Gizmos.color = Color.blue;

        if (_showP0 && _showP1) {
            DrawSegment(P0, P1);            
        }

        if (_showP0 && _showP2) {
            DrawSegment(P0, P2);            
        }

        if (_showP0 && _showP3) {
            DrawSegment(P0, P3);            
        }

        if (_showP1 && _showP2) {
            DrawSegment(P1, P2);            
        }

        if (_showP1 && _showP3) {
            DrawSegment(P1, P3);            
        }

        if (_showP2 && _showP3) {
            DrawSegment(P2, P3);
        }

        var center = (P0 + P1 + P2 + P3) / 4;
        Gizmos.color = Color.green;

        if (_showCenter) {
            Gizmos.DrawSphere(center, _gizmosSize);

            Gizmos.DrawLine(P0, center);
            Gizmos.DrawLine(P1, center);
            Gizmos.DrawLine(P2, center);
            Gizmos.DrawLine(P3, center);
        }
    }

    private void DrawSegment(Vector3 a, Vector3 b) {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere((a + b) / 2, _gizmosSize / 2);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(a, b);
    }
}
