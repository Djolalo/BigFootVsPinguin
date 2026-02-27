using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSnowParticle : MonoBehaviour
{
    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        print(initialRotation);
        print("local rotation : " + transform.localRotation);
        transform.rotation = initialRotation;
    }
}
