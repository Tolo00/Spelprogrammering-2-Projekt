using UnityEngine;

public interface IGun {
    void SetEnabled(bool enabled);
    void SetDirection(Vector2 direction);
    void SetOrigin(Vector2 origin);
}