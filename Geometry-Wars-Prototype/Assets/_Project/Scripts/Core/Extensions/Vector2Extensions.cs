using UnityEngine;

public static class Vector2Extensions {
    public static float ToDegrees(this Vector2 vector) {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static float ToRadians(this Vector2 vector) {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static Vector2 DegreesToVector2(this float angle) {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static Vector2 RadiansToVector2(this float angle) {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}