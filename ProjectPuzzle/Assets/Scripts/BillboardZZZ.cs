using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardZZZ : MonoBehaviour
{
    void FixedUpdate()
    {
        //transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
    }
}
