using System.Collections.Generic;
using Arosoul.Essentials;
using UnityEngine;

public class WarpingGrid : Singleton<WarpingGrid> {
    [Header("Grid Settings")]
    [SerializeField, Min(3)] Vector2Int _size;
    [SerializeField, Min(0.5f)] Vector2 _spacing;
    [SerializeField, Min(0.01f)] float _stiffness = 0.25f;
    [SerializeField, Min(0.01f)] float _damping = 0.5f;

    [Header("Visuals")]
    [SerializeField, Min(0f)] float _lineWidth = 0.05f;
    [SerializeField] Color _color;
    [SerializeField] Material _material;

    private GridPoint[,] _points;
    private List<GridSpring> _springs = new();

    private bool _initialized = false;

    [SerializeField] MeshLineRenderer _lineRenderer;

    void Awake() {
    
        Initialize();
    }

    void OnDestroy() {

    }

    private void Initialize() {
        Debug.Assert(_material != null, "Material is not assigned.");
        Debug.Assert(_lineRenderer != null, "LineRenderer is not assigned.");

        BuildGrid();

        _initialized = true;
    }

    private void BuildGrid() {
        _points = new GridPoint[_size.x, _size.y];

        _springs.Clear();
        
        // Create points
        for (int y = 0; y < _size.y; y++) {
            for (int x = 0; x < _size.x; x++) {
                if (x == 0 || y == 0 || x == _size.x-1 || y == _size.y-1)
                    _points[x, y] = new GridPoint(new Vector2(x * _spacing.x, y * _spacing.y), 0); // Fixed wall points
                else 
                    _points[x, y] = new GridPoint(new Vector2(x * _spacing.x, y * _spacing.y), 1); // Moveable mesh points
            }
        }

        // Link points with springs
        for (int y = 0; y < _size.y; y++) {
            for (int x = 0; x < _size.x; x++) {  
                if (x > 0) _springs.Add(new GridSpring(_points[x-1, y], _points[x,y], _stiffness, _damping));
                if (y > 0) _springs.Add(new GridSpring(_points[x, y-1], _points[x,y], _stiffness, _damping));
            }   
        }
    }

    void FixedUpdate() {
        if (!_initialized) return;

        // Update simulation
        foreach (GridSpring spring in _springs) {
            spring.Tick();
        }
        foreach (GridPoint point in _points) {
            point.Tick();
        }

        // Draw grid
        _lineRenderer.ClearLines();
        // Vector2 originPos = transform.position;
        for (int y = 0; y < _size.y; y++) {
            for (int x = 0; x < _size.x; x++) {
                if (x > 0) _lineRenderer.AddLine(_points[x-1, y].Position, _points[x,y].Position, _color, width: _lineWidth);
                if (y > 0) _lineRenderer.AddLine(_points[x, y-1].Position, _points[x,y].Position, _color, width: _lineWidth);
            }
        }
    }

    void OnDrawGizmosSelected() {
        // Draw borders
        Vector2 bottomLeft = transform.position;
        Vector2 topRight = transform.position + new Vector3((_size.x-1) * _spacing.x, (_size.y-1) * _spacing.y);

        Gizmos.DrawLine(bottomLeft, new Vector2(bottomLeft.x, topRight.y));
        Gizmos.DrawLine(bottomLeft, new Vector2(topRight.x, bottomLeft.y));
        Gizmos.DrawLine(topRight, new Vector2(topRight.x, bottomLeft.y));
        Gizmos.DrawLine(topRight, new Vector2(bottomLeft.x, topRight.y));
    }

    #region Manipulate Grid
    public static void ApplyImpulseForce(float force, Vector2 position, float radius) {
        if (!Inst.ValidateSingleton()) return;

        foreach (GridPoint point in Inst._points) {
            Vector2 relativePointPos = (Vector2)Inst.transform.position + point.Position;
            float distSqr = (position - relativePointPos).sqrMagnitude;
            if (distSqr < radius * radius) {
                point.AddForce(100 * force * (position - relativePointPos) / (1000 + distSqr));
                point.IncreaseDamping(0.6f);
            }
        }
    }

    public static void ApplyExplosiveForce(float force, Vector2 position, float radius) {
        if (!Inst.ValidateSingleton()) return;

        foreach (GridPoint point in Inst._points) {
            Vector2 relativePointPos = (Vector2)Inst.transform.position + point.Position;
            float distSqr = (position - relativePointPos).sqrMagnitude;
            if (distSqr < radius * radius) {
                point.AddForce(100 * force * (relativePointPos - position) / (10000 + distSqr));
                point.IncreaseDamping(0.6f);
            }
        }
    }
    #endregion

    private bool ValidateSingleton() {
        if (Inst == null) Debug.LogWarning("Warping Grid does not exist in scene.");
        return Inst != null;
    }

}