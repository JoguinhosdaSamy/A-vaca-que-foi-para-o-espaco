using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
       public WayPoints target;
    
    // void Update()
    // {
    //     if(Input.touchCount > 0)
    //     {
    //         Touch t = Input.GetTouch(0);
    //         
    //         Vector3 pos = GetComponent<Camera>().ScreenToWorldPoint(t.position);
    //         pos.z = 0;
    //         transform.position = pos;
    //     }
    // }
    
    void FixedUpdate()
    {
        Vector3 dir = target.transform.position - transform.position;
         if(dir.magnitude < 0.1f)
            {
                target = target.points[0];
            }   
        transform.position = transform.position + dir.normalized * Time.fixedDeltaTime * 6.0f;
    }
}

