using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    public RobotJoint[] Joints;

    protected float[] Angles = null;
    public Transform Target;
    [Tooltip("Distance end effector should maintain to target")]
    public float TargetDistance;
    protected Vector3 targetPosition;

    [Range(0, 1f)]
    [Tooltip("Used to simulate gradient (degrees).")]
    public float DeltaGradient = 0.1f;
    [Range(0, 100f)]
    [Tooltip("How much we move depending on the gradient.")]
    public float LearningRate = 25f;

    [Range(0, 0.25f)]
    [Tooltip("If closer than this, it stops.")]
    public float StopThreshold = 0.1f;
    [Range(0, 10f)]
    public float SlowdownThreshold = 0.25f;

    void Start()
    {
        Angles = new float[Joints.Length];
    }

    void Update()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        targetPosition = Target.position - direction * TargetDistance;
        if (DistanceFromTarget(targetPosition, Angles) > StopThreshold)
        {
            ApprochTarget(targetPosition);
        }

        Debug.DrawLine(Joints[Joints.Length - 1].transform.position, targetPosition, Color.green);
        Debug.DrawLine(Target.transform.position, targetPosition, new Color(0, 0.5f, 0));
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    protected void ApprochTarget(Vector3 target)
    {
        // Starts from the end, down to the base
        for (int i = Joints.Length - 1; i >= 0; i--)
        {
            if (DistanceFromTarget(target, Angles) <= StopThreshold)
            {
                break;
            }

            float error = DistanceFromTarget(target, Angles);
            float slowdown = Mathf.Clamp01((error - StopThreshold) / (SlowdownThreshold - StopThreshold));

            // Gradient descent
            float gradient = CalculateGradient(target, Angles, i, DeltaGradient);
            Angles[i] -= LearningRate * gradient * slowdown;
            // Clamp
            Angles[i] = Joints[i].ClampAngle(Angles[i]);
        }

        for (int i = 0; i < Joints.Length - 1; i++)
        {
            Joints[i].SetAngle(Angles[i]);
        }
    }

    protected float CalculateGradient(Vector3 target, float[] angles, int i, float delta)
    {
        float[] anglesCopy = (float[])angles.Clone();
        float f_x = DistanceFromTarget(target, anglesCopy);

        anglesCopy[i] += delta;
        float f_x_plus_h = DistanceFromTarget(target, anglesCopy);

        float gradient = (f_x_plus_h - f_x) / delta;

        return gradient;
    }

    protected Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = Joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < Joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }

        return prevPoint;
    }
}
