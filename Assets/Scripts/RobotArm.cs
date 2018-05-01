using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : InverseKinematics
{
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
}
