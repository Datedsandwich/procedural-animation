using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;
    public float MinAngle;
    public float MaxAngle;

    [Range(0, 1f)]
    public float SlowdownThreshold = 0.5f;
    [Range(0, 360f)]
    public float Speed = 1f;

    void Awake()
    {
        StartOffset = transform.localPosition;
    }

    public float ClampAngle(float angle, float delta = 0)
    {
        return Mathf.Clamp(angle + delta, MinAngle, MaxAngle);
    }

    public float GetAngle()
    {
        float angle = 0;
        if (Axis.x == 1) angle = transform.localEulerAngles.x;
        else
        if (Axis.y == 1) angle = transform.localEulerAngles.y;
        else
        if (Axis.z == 1) angle = transform.localEulerAngles.z;

        return ClampAngle(angle);
    }

    public float SetAngle(float angle)
    {
        angle = ClampAngle(angle);
		
        if (Axis.x == 1)
        {
            transform.localEulerAngles = new Vector3(angle, 0, 0);
        }
        else if (Axis.y == 1)
        {
            transform.localEulerAngles = new Vector3(0, angle, 0);
        }
        else if (Axis.z == 1)
        {
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        return angle;
    }
}
