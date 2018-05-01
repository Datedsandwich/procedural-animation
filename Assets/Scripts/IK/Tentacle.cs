using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : InverseKinematics
{
    [Range(0, 10)]
    public float OrientationWeight = 0.5f;
    [Range(0, 10)]
    public float TorsionWeight = 0.5f;
    public Vector3 TorsionPenality = new Vector3(1, 0, 0);

	void Start() {
		Angles = new float[Joints.Length];
	}

    void Update()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        targetPosition = Target.position - direction * TargetDistance;
        if (ErrorFunction(targetPosition, Angles) > StopThreshold)
        {
            ApprochTarget(targetPosition);
        }

        Debug.DrawLine(Joints[Joints.Length - 1].transform.position, targetPosition, Color.green);
        Debug.DrawLine(Target.transform.position, targetPosition, new Color(0, 0.5f, 0));
    }

    protected override float ErrorFunction(Vector3 target, float[] angles)
    {
        PositionRotation result = ForwardKinematics(angles);

        // Minimise torsion (rotation on x axis)
        float torsion = 0;
        for (int i = 0; i < angles.Length; i++)
        {
            torsion += Mathf.Abs(angles[i]) * TorsionPenality.x;
            torsion += Mathf.Abs(angles[i]) * TorsionPenality.y;
            torsion += Mathf.Abs(angles[i]) * TorsionPenality.z;
        }

		float distance = Vector3.Distance(target, result);
		float effectorOrientation = Mathf.Abs(Quaternion.Angle(result, Target.rotation) / 180f) * OrientationWeight;
		torsion = (torsion / angles.Length) * TorsionWeight;

        return distance + effectorOrientation + torsion;
    }
}
