using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float speed;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateX)
        { 
            transform.Rotate(speed * Time.deltaTime, 0, 0);
        }
        if (rotateY)
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
        if (rotateZ)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}
