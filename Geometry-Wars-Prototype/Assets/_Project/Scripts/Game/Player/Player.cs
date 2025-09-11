using UnityEngine;

public class Player : MonoBehaviour {
    public bool InputEnabled { get; private set; } = true;
    public void SetInputEnabled(bool enabled) => InputEnabled = enabled;

    
}