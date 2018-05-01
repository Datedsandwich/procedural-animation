using UnityEngine;

public struct PositionRotation
{
    Vector3 position;
    Quaternion rotation;

    public PositionRotation(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public static implicit operator Vector3(PositionRotation positionRotation)
    {
        return positionRotation.position;
    }

    public static implicit operator Quaternion(PositionRotation positionRotation)
    {
        return positionRotation.rotation;
    }
}
