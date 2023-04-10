using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameController controller;
    public Point pointActual;
    private Player target;
    public Transform directionGo;
    [HideInInspector]
    public bool _istargetNull;
    public int counter;
    
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        target = GameObject.Find("Player").GetComponent<Player>();
        _istargetNull = target == null;
        directionGo = pointActual.transform;
    }
    
    public void FindShortestPath()
    {
        if(counter == 0){
            if (target.target == pointActual)
            {
                controller.SetMovementStatus(GameController.Movement.Vaca);
                return;
            }
            Point[] listaPoints = pointActual.points;
            Point.Tipo[] listaTarget = pointActual.tipo;
            float minDistance = float.MaxValue;
            for (var i = 0; i < listaPoints.Length; i++)
            {
                if (listaTarget[i] == Point.Tipo.Vaca)
                {
                    continue;
                }
                Vector3 newPosition = this.transform.position + new Vector3(listaPoints[i].transform.position.x,0.0f, listaPoints[i].transform.position.z);
                float distance = (this.target.transform.position - newPosition).magnitude;

                if (distance < minDistance)
                {
                    directionGo = listaPoints[i].transform;
                    minDistance = distance;
                    pointActual = listaPoints[i];
               
                }
            }
            
            _istargetNull = false;
        }
    }
    
    void FixedUpdate()
    {
        if (controller.MovementStatus != GameController.Movement.Alien) return;
            
        if (_istargetNull) return;

        if (counter != 0)
        {
            counter--;
            controller.SetMovementStatus(GameController.Movement.Vaca);
            return;
        }

        Vector3 dir = directionGo.position - transform.position;
        if (dir.magnitude > 0.1)
        {
            transform.Translate(dir.normalized * (3.0f * Time.deltaTime), Space.World);
        } else {
            _istargetNull = true;
            controller.SetMovementStatus(GameController.Movement.Vaca);
        }

    }
    
    void CheckDie()
    {
        if (pointActual == target.target)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

[CustomEditor(typeof(Enemy), true)]
public class EnemyEditor : Editor
{
    SerializedProperty counterProp;
    SerializedProperty pointProp;
    
    void OnEnable()
    {
        // Setup the SerializedProperties.
        counterProp = serializedObject.FindProperty ("counter");
        pointProp = serializedObject.FindProperty ("pointActual");

    }
    public override void OnInspectorGUI()
    {

        EditorGUILayout.IntSlider (counterProp, 0, 5, new GUIContent ("Counter do enemy"));
        EditorGUILayout.PropertyField (pointProp, new GUIContent ("Ponto Inicial"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
