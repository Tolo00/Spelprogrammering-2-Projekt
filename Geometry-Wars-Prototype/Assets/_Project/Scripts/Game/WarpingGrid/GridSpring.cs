using UnityEngine;

public class GridSpring {
    public GridPoint PointA;
    public GridPoint PointB;
    public float TargetLength;
    public float Stiffness;
    public float Damping;

    public GridSpring(GridPoint pointA, GridPoint pointB, float stiffness, float damping) {
        PointA = pointA;
        PointB = pointB;
        TargetLength = Vector3.Distance(pointA.Position, pointB.Position);
        Stiffness = stiffness;
        Damping = damping;
    }

    public void Tick() {
        var x = PointA.Position - PointB.Position;
        float magnitude = x.magnitude;

        if (magnitude <= TargetLength) 
            return; // Pull only

        x = x / magnitude * (magnitude - TargetLength);
        Vector2 diffVel = PointB.Velocity - PointA.Velocity;
        Vector2 force = Stiffness * x - diffVel * Damping;

        PointA.AddForce(-force);
        PointB.AddForce(force);
    }
}