using System;
using Arosoul.Essentials;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonContext { Pressed, Released }
public enum DeviceType { Keyboard, Controller }

public class GameInput : SingletonPersistent<GameInput> {

    // Exposed Vector Actions
    public static Vector2 Move { get; private set; }
    public static Vector2 Look { get; private set; }

    // Exposed Button Actions
    public static bool IsFirePressed; // TODO: Compress these fields into a class/struct to make it easier to make button actions
    // public static bool WasFirePressedThisFrame;
    // public static bool WasFireReleasedThisFrame;
    public static event Action<ButtonContext> OnFire; 



    // Event for device switching
    public static event Action<DeviceType> OnActiveDeviceChanged;



    private InputActions _inputActions;

    protected override void OnSingletonEnable() {
        _inputActions = new InputActions();
        _inputActions.Enable();

        /* Subscribe to callbacks */
 
        // Vector2
        _inputActions.Player.Move.performed += ctx => ExecuteInputAction(() => Move = ctx.ReadValue<Vector2>(), ctx);
        _inputActions.Player.Move.canceled += ctx => ExecuteInputAction(() => Move = Vector2.zero, ctx);

        _inputActions.Player.Look.performed += ctx => ExecuteInputAction(() => Look = ctx.ReadValue<Vector2>(), ctx);
        _inputActions.Player.Look.canceled += ctx => ExecuteInputAction(() => Look = Vector2.zero, ctx);

        // Button
        _inputActions.Player.Fire.performed += ctx => ExecuteInputAction(() => {
            IsFirePressed = true;
            OnFire?.Invoke(ButtonContext.Pressed);
        }, ctx);
        _inputActions.Player.Fire.canceled += ctx => ExecuteInputAction(() => { 
            IsFirePressed = false;
            OnFire?.Invoke(ButtonContext.Released); 
        }, ctx);
    }

    protected override void OnSingletonDisable() {
        // Clean-up
        _inputActions.Disable();
        _inputActions.Dispose();
    }


    private void ExecuteInputAction(Action action, InputAction.CallbackContext ctx) {
        UpdateActiveDevice(ctx);

        try {
            action?.Invoke();
        }
        catch (Exception ex) {
            throw ex;
        }
    }


    // Manage input device
    public static DeviceType ActiveDevice { get; private set; } = DeviceType.Keyboard;
    public static bool MouseVisible { get; private set; } = true;

    /// <summary>
    /// Set if mouse is visible.
    /// Mouse will only show when ActiveDevice is set to Keyboard.
    /// Mouse will always be hidden if a controller is used.
    /// </summary>
    public static void SetMouseVisible(bool visible) { 
        MouseVisible = visible;
        Inst.UpdateMouseVisibility(); 
    }

    private void UpdateActiveDevice(InputAction.CallbackContext ctx) {
        var device = ctx.control.device;
        DeviceType deviceUsed;

        if (device is Gamepad) deviceUsed = DeviceType.Controller;
        else if (device is Keyboard || device is Mouse) deviceUsed = DeviceType.Keyboard;
        else {
            Debug.LogError($"Game input does not implement device used: {device.name}");
            return;
        }

        if (deviceUsed != ActiveDevice) {
            // New input device
            ActiveDevice = deviceUsed;
            OnActiveDeviceChanged?.Invoke(deviceUsed);
            Debug.Log($"Switched active device to: {ActiveDevice}");
        }

        UpdateMouseVisibility();
    }

    private void UpdateMouseVisibility() {
        if (MouseVisible && ActiveDevice == DeviceType.Keyboard) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}