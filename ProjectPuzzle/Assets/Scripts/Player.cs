using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform target;
    public static Player player;
    private bool _istargetNull;

    void Start()
    {
        _istargetNull = target == null;
        player = this;
    }

    void FixedUpdate()
    {
        if (_istargetNull) return;

        Vector3 dir = target.position - transform.position;
        if (dir.magnitude > 0.1){
            transform.Translate(dir.normalized * (3.0f * Time.deltaTime), Space.World);
        }
    }

}