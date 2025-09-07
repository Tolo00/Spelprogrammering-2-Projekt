using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LineDrawingTest : MonoBehaviour {
    
    [SerializeField] Material _material;

    private LineRenderer _lr;

    Vector3 startPos;
    Vector3 mouseWorldPos;

    void Awake() {
        Debug.Assert(_material != null, "Material is null.");

        _lr = new LineRenderer(_material, transform);
        RenderPipelineManager.endCameraRendering += Render;
    }

    void OnDestroy() {
        RenderPipelineManager.endCameraRendering -= Render;
    }

    void Update() {
        Vector3 screenPos = Mouse.current.position.value;
        screenPos.z = 10f;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            startPos = mouseWorldPos;
        }

        if (!Mouse.current.leftButton.isPressed) {
            _lr.ClearLines();
            return;            
        }
        
        _lr.ClearLines();
        _lr.AddLine(startPos, mouseWorldPos, Color.red, Color.blue, 0.05f);
    }

    void Render(ScriptableRenderContext context, Camera camera) {
        if (_lr == null) return;
        _lr.RenderLines();
    }
}
