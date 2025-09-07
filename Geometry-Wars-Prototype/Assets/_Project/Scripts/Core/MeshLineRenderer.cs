using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mesh-based line renderer using MeshFilter + MeshRenderer.
/// Add this to a GameObject and call AddLine() / ClearLines().
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshLineRenderer : MonoBehaviour
{
    private struct Line {
        public Vector3 StartPoint;
        public Color StartColor;
        public Vector3 EndPoint;
        public Color EndColor;
        public float Width;
    }

    [SerializeField] string sortingLayerName;

    private readonly List<Line> _lines = new();
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private bool _dirty = true;

    void Awake() {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        _meshRenderer.sortingLayerName = sortingLayerName;
        _mesh = new Mesh { name = "LineMesh" };
        _meshFilter.sharedMesh = _mesh;
    }

    public void AddLine(Vector3 start, Vector3 end, Color startColor, Color endColor, float width = 0.05f) {
        _lines.Add(new Line {
            StartPoint = start,
            EndPoint = end,
            StartColor = startColor,
            EndColor = endColor,
            Width = width
        });
        _dirty = true;
    }

    public void AddLine(Vector3 start, Vector3 end, Color color, float width = 0.05f) =>
        AddLine(start, end, color, color, width);

    public void ClearLines() {
        _lines.Clear();
        _mesh.Clear();
        _dirty = true;
    }

    void LateUpdate() {
        if (_dirty) RebuildMesh();
    }

    private void RebuildMesh() {
        if (_lines.Count == 0) {
            _mesh.Clear();
            _dirty = false;
            return;
        }

        int quadCount = _lines.Count;
        int vertCount = quadCount * 4;
        int indexCount = quadCount * 6;

        var vertices = new Vector3[vertCount];
        var colors = new Color[vertCount];
        var indices = new int[indexCount];

        Camera cam = Camera.main;
        if (!cam) return;

        int v = 0, i = 0;
        foreach (Line line in _lines) {
            Vector3 dir = line.EndPoint - line.StartPoint;
            Vector3 camToStart = line.StartPoint - cam.transform.position;
            Vector3 cross = Vector3.Cross(camToStart, dir).normalized * line.Width;

            vertices[v + 0] = line.StartPoint + cross;
            vertices[v + 1] = line.StartPoint - cross;
            vertices[v + 2] = line.EndPoint - cross;
            vertices[v + 3] = line.EndPoint + cross;

            colors[v + 0] = line.StartColor;
            colors[v + 1] = line.StartColor;
            colors[v + 2] = line.EndColor;
            colors[v + 3] = line.EndColor;

            indices[i + 0] = v + 0;
            indices[i + 1] = v + 1;
            indices[i + 2] = v + 2;
            indices[i + 3] = v + 2;
            indices[i + 4] = v + 3;
            indices[i + 5] = v + 0;

            v += 4;
            i += 6;
        }

        _mesh.Clear();
        _mesh.SetVertices(vertices);
        _mesh.SetColors(colors);
        _mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        _dirty = false;
    }
}










// /// <summary>
// /// Mesh-based line renderer. Builds a mesh of quads for each line.
// /// </summary>
// public class LineRenderer {
//     private struct Line {
//         public Vector3 StartPoint;
//         public Color StartColor;
//         public Vector3 EndPoint;
//         public Color EndColor;
//         public float Width;
//     }

//     private readonly List<Line> _lines = new();
//     private readonly Material _material;
//     private readonly Transform _originTransform;

//     private Mesh _mesh;

//     public LineRenderer(Material material, Transform origin = null) {
//         _material = material;
//         _originTransform = origin;
//         _mesh = new Mesh { name = "LineMesh" };
//     }

//     public int Count => _lines.Count;

//     public void AddLine(Vector3 startPoint, Vector3 endPoint, Color startColor, Color endColor, float width = 0.05f) {
//         _lines.Add(new Line {
//             StartPoint = startPoint,
//             StartColor = startColor,
//             EndPoint = endPoint,
//             EndColor = endColor,
//             Width = width
//         });
//     }

//     public void AddLine(Vector3 startPoint, Vector3 endPoint, Color color, float width = 0.05f) {
//         AddLine(startPoint, endPoint, color, color, width);
//     }

//     public void ClearLines() => _lines.Clear();

//     /// <summary>
//     /// Build a mesh from all lines and render it.
//     /// </summary>
//     public void RenderLines(Camera cam = null) {
//         if (_material == null) {
//             Debug.LogError("Material is missing.");
//             return;
//         }
//         if (_lines.Count == 0)
//             return;

//         cam ??= Camera.main;
//         if (cam == null) return;

//         Vector3 origin = _originTransform ? _originTransform.position : Vector3.zero;

//         int quadCount = _lines.Count;
//         int vertCount = quadCount * 4;
//         int indexCount = quadCount * 6;

//         var vertices = new Vector3[vertCount];
//         var colors = new Color[vertCount];
//         var indices = new int[indexCount];

//         int v = 0, i = 0;
//         foreach (Line line in _lines) {
//             Vector3 lineDir = line.EndPoint - line.StartPoint;
//             Vector3 camToStart = line.StartPoint - cam.transform.position;
//             Vector3 cross = Vector3.Cross(camToStart, lineDir).normalized * line.Width;

//             // Quad corners
//             vertices[v + 0] = origin + line.StartPoint + cross;
//             vertices[v + 1] = origin + line.StartPoint - cross;
//             vertices[v + 2] = origin + line.EndPoint - cross;
//             vertices[v + 3] = origin + line.EndPoint + cross;

//             colors[v + 0] = line.StartColor;
//             colors[v + 1] = line.StartColor;
//             colors[v + 2] = line.EndColor;
//             colors[v + 3] = line.EndColor;

//             // Two triangles
//             indices[i + 0] = v + 0;
//             indices[i + 1] = v + 1;
//             indices[i + 2] = v + 2;
//             indices[i + 3] = v + 2;
//             indices[i + 4] = v + 3;
//             indices[i + 5] = v + 0;

//             v += 4;
//             i += 6;
//         }

//         _mesh.Clear();
//         _mesh.SetVertices(vertices);
//         _mesh.SetColors(colors);
//         _mesh.SetIndices(indices, MeshTopology.Triangles, 0);

//         _material.SetPass(0);
//         Graphics.DrawMesh(_mesh, Matrix4x4.identity, _material, 0);
//     }
// }









// using System.Collections.Generic;
// using UnityEngine;

// /// <summary>
// /// Class for drawing lines in Playmode.
// /// To render lines. Call the RenderLines() method inside:
// /// -   Camera.onPostRender                      (Built-in Render Pipeline)
// /// -   RenderPipelineManager.endCameraRendering (URP/HDRP)
// /// </summary>

// public class LineRenderer {
//     private struct Line {
//         public Vector3 StartPoint;
//         public Color StartColor;

//         public Vector3 EndPoint;
//         public Color EndColor;

//         public float Width;
//     }

//     private Material _material;
//     private Transform _originTransform;

//     private List<Line> _lines = new();

//     /// <param name="material">Material used when rendering lines.</param>
//     /// <param name="origin">Origin transform. Used to offset lines. Leave null to have world center (0,0,0) as origin.</param>
//     public LineRenderer(Material material, Transform origin = null) {
//         _material = material;
//         _originTransform = origin;
//     }

//     /// <summary>
//     /// Amount of lines.
//     /// </summary>
//     public int Count => _lines.Count;


//     public void AddLine(Vector3 startPoint, Vector3 endPoint, Color startColor, Color endColor, float width = 0.05f) {
//         _lines.Add(new Line() {
//             StartPoint = startPoint,
//             StartColor = startColor,
//             EndPoint = endPoint,
//             EndColor = endColor,
//             Width = width
//         });
//     }

//     public void AddLine(Vector3 startPoint, Vector3 endPoint, Color color, float width = 0.05f) {
//         _lines.Add(new Line() {
//             StartPoint = startPoint,
//             StartColor = color,
//             EndPoint = endPoint,
//             EndColor = color,
//             Width = width
//         });
//     }

//     public void ClearLines() {
//         _lines.Clear();
//     }


//     /// <summary>
//     /// Render lines to screen.
//     /// Should be called inside:
//     ///     Camera.onPostRender (Built-in Render Pipeline) /
//     ///     RenderPipelineManager.endCameraRendering (URP/HDRP)
//     /// </summary>
//     public void RenderLines() {
//         if (_material == null) {
//             Debug.LogError("Material is missing.");
//             return;
//         }
//         Camera activeCamera = Camera.main;
//         Vector3 origin = _originTransform != null ? _originTransform.position : Vector3.zero;

//         GL.Begin(GL.QUADS);
//         _material.SetPass(0);
//         foreach(Line line in _lines) {
//             Vector3 lineDiff = line.StartPoint - line.EndPoint;
//             Vector3 cameraToStartDiff = line.StartPoint - activeCamera.transform.position;
//             Vector3 cameraToEndDiff = line.EndPoint - activeCamera.transform.position;
//             Vector3 crossStart = Vector3.Cross(cameraToStartDiff, lineDiff).normalized;
//             Vector3 crossEnd = Vector3.Cross(cameraToEndDiff, lineDiff).normalized;

//             // Start point
//             GL.Color(line.StartColor);
//             GL.Vertex(origin + line.StartPoint + (crossStart * line.Width));
//             GL.Vertex(origin + line.StartPoint - (crossStart * line.Width));

//             // End point
//             GL.Color(line.EndColor);
//             GL.Vertex(origin + line.EndPoint - (crossEnd * line.Width));
//             GL.Vertex(origin + line.EndPoint + (crossEnd * line.Width));
//         }
//         GL.End();
//     }
// }