using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] [Range(0,100f)] private float rotationSpeedX;
    [SerializeField] [Range(0,100f)] private float rotationSpeedY;
    [SerializeField] [Range(0,100f)] private float rotationSpeedZ;
    
    void Update()
    {
        transform.Rotate(rotationSpeedX * Time.deltaTime, 
                        rotationSpeedY * Time.deltaTime, 
                        rotationSpeedZ * Time.deltaTime, 
                        Space.Self);
    }
}
