using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1 : MonoBehaviour
{
    public Transform target;
    public static Player_1 player;

    void Start(){
        player = this;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        if (dir.magnitude > 0.1){
            transform.Translate(dir.normalized * 3.0f * Time.deltaTime, Space.World);
        }
    }

}
