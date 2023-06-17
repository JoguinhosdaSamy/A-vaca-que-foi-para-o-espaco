using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    //tiramos isso que está na linha 12! tirar em todas as variáveis...
    private GameController _controller;
    public Point target;
    public static Player player;
    public AudioClip clickSound;
    private AudioSource audioSource;
    public Enemy enemy;
    public GameObject[] modelos;
    [Range (3,10)]
    public float speed = 4.0f;
    [FormerlySerializedAs("_istargetNull")][HideInInspector]
    public bool istargetNull;

    private int jumps;
    

    void Start()
    {
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        istargetNull = target == null;
        player = this;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
     {
         if (target == enemy.currentPoint)
         {
             if(_controller.movementStatus == GameController.Movement.Vaca){
                 _controller.GameOver();
             }
        
         }
         
         if (istargetNull) return;

         Vector3 dir = target.transform.position - transform.position;
         if (dir.magnitude > 0.1)
         {
            transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);
            Vector3 rot = dir.normalized;
            rot.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(dir.normalized);
            modelos[0].SetActive(false);
            modelos[1].SetActive(true);
        } else {
             istargetNull = true;
            modelos[0].SetActive(true);
            modelos[1].SetActive(false);
            CheckCondition();
            _controller.SetMovementStatus(GameController.Movement.Alien);
            jumps++;
            CheckDie();
         }
     }
    private void CheckCondition()
    {
       
        var targetTag = target.property;

        switch (targetTag)
        {
            case Point.Prop.EndPoint:
                _controller.Victory(jumps);
                break;
            case Point.Prop.SleepPower:
                _controller.PowerSleep();
                break;
        }
        
    }
    
   private void CheckDie()
    {
        //if (target == enemy.currentPoint)
        Vector3 dir = GameController.controller.enemy.transform.position - transform.position;
        if (dir.magnitude < 0.1f) 
        {
            _controller.GameOver();
            
        }
    }

   public void CheckPossibilities()
   {
       List<Point> arrayPoints = new List<Point>();

       for (var i = 0; i < target.points.Length; i++)
       {
           if (target.tipo[i] == Point.Tipo.Alien)
           {
               continue;
           }

           arrayPoints.Add(target.points[i]);

       }

        if (arrayPoints.Count == 1)
        {

            if (arrayPoints[0] == enemy.currentPoint)
            {
                _controller.SetMovementStatus(GameController.Movement.Alien);
            }
            Debug.Log("som logou");
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (clickSound = null)
            {
              audioSource.PlayOneShot(clickSound);
            }
        }
        
   }
}
/*#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private SerializedProperty _target;
    private SerializedProperty _speed;


    private void OnEnable()
    {
        _target = serializedObject.FindProperty("target");
        _speed = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_speed, 0.1f, 10f, new GUIContent("Speed do Player"));
        EditorGUILayout.Space();
        GUIContent targetLabel = new GUIContent("WayPoint Inicial");
        EditorGUILayout.PropertyField(_target, targetLabel);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif*/