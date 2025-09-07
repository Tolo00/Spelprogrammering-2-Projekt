using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for drawing lines in Playmode.
/// To render lines. Call the RenderLines() method inside:
/// -   Camera.onPostRender                      (Built-in Render Pipeline)
/// -   RenderPipelineManager.endCameraRendering (URP/HDRP)
/// </summary>

public class LineRenderer {
    private struct Line {
        public Vector3 StartPoint;
        public Color StartColor;

        public Vector3 EndPoint;
        public Color EndColor;

        public float Width;
    }

    private Material _material;
    private Transform _origin;

    private List<Line> _lines = new();

    /// <param name="material">Material used when rendering lines.</param>
    /// <param name="origin">Origin transform. Used to offset lines. Leave null to have world center (0,0,0) as origin.</param>
    public LineRenderer(Material material, Transform origin = null) {
        _material = material;
        _origin = origin;
    }

    /// <summary>
    /// Amount of lines.
    /// </summary>
    public int Count => _lines.Count;


    public void AddLine(Vector3 startPoint, Vector3 endPoint, Color startColor, Color endColor, float width = 0.05f) {
        _lines.Add(new Line() {
            StartPoint = startPoint,
            StartColor = startColor,
            EndPoint = endPoint,
            EndColor = endColor,
            Width = width
        });
    }

    public void AddLine(Vector3 startPoint, Vector3 endPoint, Color color, float width = 0.05f) {
        _lines.Add(new Line() {
            StartPoint = startPoint,
            StartColor = color,
            EndPoint = endPoint,
            EndColor = color,
            Width = width
        });
    }

    public void ClearLines() {
        _lines.Clear();
    }


    /// <summary>
    /// Render lines to screen.
    /// Should be called inside:
    ///     Camera.onPostRender (Built-in Render Pipeline) /
    ///     RenderPipelineManager.endCameraRendering (URP/HDRP)
    /// </summary>
    public void RenderLines() {
        // GL.Begin(GL.LINES);
        // _material.SetPass(0);
        // foreach(Line line in _lines) {
        //     GL.Color(line.StartColor);
        //     GL.Vertex(_origin.position + line.StartPoint);

        //     GL.Color(line.EndColor);
        //     GL.Vertex(_origin.position + line.EndPoint);
        // }
        // GL.End();
    
        Camera activeCamera = Camera.main;

        GL.Begin(GL.QUADS);
        _material.SetPass(0);
        foreach(Line line in _lines) {
            Vector3 lineDiff = line.StartPoint - line.EndPoint;
            Vector3 cameraToStartDiff = line.StartPoint - activeCamera.transform.position;
            Vector3 cameraToEndDiff = line.EndPoint - activeCamera.transform.position;
            Vector3 crossStart = Vector3.Cross(cameraToStartDiff, lineDiff).normalized;
            Vector3 crossEnd = Vector3.Cross(cameraToEndDiff, lineDiff).normalized;

            // Start point
            GL.Color(line.StartColor);
            GL.Vertex(_origin.position + line.StartPoint + (crossStart * line.Width));
            GL.Vertex(_origin.position + line.StartPoint - (crossStart * line.Width));

            // End point
            GL.Color(line.EndColor);
            GL.Vertex(_origin.position + line.EndPoint - (crossEnd * line.Width));
            GL.Vertex(_origin.position + line.EndPoint + (crossEnd * line.Width));
        }

        GL.Color(Color.red);
        GL.Vertex3(0, 0.5f, 0);
        GL.Vertex3(0.5f, 1, 0);
        GL.Vertex3(1, 0.5f, 0);
        GL.Vertex3(0.5f, 0, 0);

        GL.Color(Color.cyan);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0.25f, 0);
        GL.Vertex3(0.25f, 0.25f, 0);
        GL.Vertex3(0.25f, 0, 0);
        GL.End();
    }
}