using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
     public WayPoints[] points;
   
    void FixedUpdate()
    {
        for(int i = 0; i < points.Length; i++)
        {
           
            Debug.DrawLine(transform.position, points[i].transform.position);
        }
    
    }
}
