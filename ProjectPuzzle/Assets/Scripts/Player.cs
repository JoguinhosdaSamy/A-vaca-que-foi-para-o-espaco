using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
       public WayPoints target;
    
    /*void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Moved)
            {
                transform.position += (Vector3)t.deltaPosition/600;
            }
        }
    }*/
    void FixedUpdate()
    {
        Vector3 dir = target.transform.position - transform.position;
         if(dir.magnitude < 0.1f)
            {
                target = target.points[0];
            }   
        transform.position = transform.position + dir + dir.normalized * Time.fixedDeltaTime * 6.0f;
    }
}

