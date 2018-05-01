using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [Range(0, 10)]
    public float OrientationWeight = 0.5f;
    [Range(0, 10)]
    public float TorsionWeight = 0.5f;
    public Vector3 TorsionPenality = new Vector3(1, 0, 0);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
