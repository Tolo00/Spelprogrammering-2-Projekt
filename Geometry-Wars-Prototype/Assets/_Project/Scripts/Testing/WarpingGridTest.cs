using UnityEngine;
using UnityEngine.InputSystem;

public class WarpingGridTest : MonoBehaviour {
    [Header("Explosive Force")]
    [SerializeField] float explosiveForce;
    [SerializeField] float explosiveRadius;

    [Header("Implosive Force")]
    [SerializeField] float implosiveForce;
    [SerializeField] float implosiveRadius;
    
    void Update() {
        if (Mouse.current.rightButton.wasPressedThisFrame) {
            WarpingGrid.ApplyImpulseForce(implosiveForce, Camera.main.ScreenToWorldPoint(Mouse.current.position.value), implosiveRadius);
        }
    }

    void FixedUpdate() {
        if (Mouse.current.leftButton.isPressed) {
            WarpingGrid.ApplyExplosiveForce(explosiveForce, Camera.main.ScreenToWorldPoint(Mouse.current.position.value), explosiveRadius);
        }
    }
}