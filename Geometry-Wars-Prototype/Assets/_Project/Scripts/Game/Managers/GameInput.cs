using System;
using Arosoul.Essentials;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : SingletonPersistent<GameInput> {

    // Exposed Vector Actions
    public static Vector2 Move { get; private set; }

    // Exposed high-level events (ex. button press)
        // public event Action OnJump; (EXAMPLE) 




    private InputActions _inputActions;

    protected override void OnSingletonEnable() {
        _inputActions = new InputActions();
        _inputActions.Enable();

        // Subscribe to callbacks
        _inputActions.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => Move = Vector2.zero;
    }

    protected override void OnSingletonDisable() {
        // Clean-up
        _inputActions.Disable();
        _inputActions.Dispose();
    }
}