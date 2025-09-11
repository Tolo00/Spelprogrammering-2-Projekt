using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour {
    [SerializeField] Transform _gunHolder;

    [Header("Guns")]
    [SerializeField] GameObject _defaultGunPrefab;

    private IGun _gunInterface;
    private GameObject _gunObject;

    private bool _fireEnabled;
    private Vector2 _fireDirection;

    private Player _playerRef;

    void Awake() {
        _playerRef = GetComponent<Player>();

        EquipDefaultGun();
    }


    void Update() {
        if (!_playerRef.InputEnabled) {
            _gunInterface.SetEnabled(false);
            return;
        }

        if (GameInput.ActiveDevice == DeviceType.Keyboard) GetKeyboardInput();
        else if (GameInput.ActiveDevice == DeviceType.Controller) GetControllerInput();
        else return;
        
        _gunInterface.SetEnabled(_fireEnabled);
        _gunInterface.SetOrigin(transform.position);
        _gunInterface.SetDirection(_fireDirection);
    }

    private void GetKeyboardInput() {
        _fireEnabled = GameInput.IsFirePressed;
        
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        _fireDirection = (mouseWorldPos - (Vector2)transform.position).normalized;
    }

    private void GetControllerInput() {
        _fireEnabled = GameInput.Look != Vector2.zero;
        _fireDirection = GameInput.Look.normalized;
    }

    



    public void EquipDefaultGun() {
        EquipGun(_defaultGunPrefab);
    }
    
    /// <summary>
    /// Equip a new gun. Removes the current one.
    /// Root GameObject requires one component that implements IGun.
    /// </summary>
    /// <param name="_gunPrefab">Gun prefab to equip.</param>
    public void EquipGun(GameObject _gunPrefab) {
        if (_gunObject != null) {
            Destroy(_gunObject);
            _gunObject = null;
            _gunInterface = null;
        }

        _gunObject = Instantiate(_gunPrefab, _gunHolder);
        _gunInterface = _gunObject.GetComponent<IGun>();

        if (_gunInterface == null) {
            Debug.LogError("Gun prefab does not implement IGun in any root components. Equipping default gun.");
            EquipDefaultGun();
            return;
        }
    }


}
