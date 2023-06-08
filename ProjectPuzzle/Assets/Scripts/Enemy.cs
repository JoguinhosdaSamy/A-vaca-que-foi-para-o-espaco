using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameController _controller;
    private Player _target;
    public Point currentPoint;
    public Transform nextPoint;
    public GameObject[] models;
    private bool _isTargetNull;
    public float speed = 8.0f;
    public int counter;
    public Image imgWait;

    private void Start()
    {
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        _target = GameObject.Find("Player").GetComponent<Player>();
        _isTargetNull = _target == null;
        nextPoint = currentPoint.transform;
        imgWait = GetComponentInChildren<Image>();
        imgWait.fillAmount = counter / 2.0f;
    }

    private void FixedUpdate()
    {
            imgWait.fillAmount = counter / 2.0f;
        if (_controller.movementStatus != GameController.Movement.Alien) return;
        if (_isTargetNull) return;
        if (counter != 0)
        {
            models[0].SetActive(false);
            models[1].SetActive(true);
            counter--;
            _target.target.ShowLights();
            _controller.SetMovementStatus(GameController.Movement.Vaca);
            _target.CheckPossibilities();
            return;
        }

        models[1].SetActive(false);
        models[0].SetActive(true);

        Vector3 dir = nextPoint.position - transform.position;
        if (dir.magnitude > 0.1f)
        {
            transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);
            Vector3 rot = dir.normalized;
            rot.y = transform.position.y;
            //transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
        else
        {
            _isTargetNull = true;
            _target.target.ShowLights();
            _controller.SetMovementStatus(GameController.Movement.Vaca);
            _target.CheckPossibilities();

        }
    }

    private void CheckDie()
    {
        {
            _controller.GameOver();
        }
    }

    public void FindShortestPath()
    {
        if (counter == 0)
        {
            if (_target.target == currentPoint)
            {
                _target.target.ShowLights();
                _controller.SetMovementStatus(GameController.Movement.Vaca);
                _target.CheckPossibilities();
                return;
            }
            
            var pq = new PriorityQueue<Point>();
            pq.Enqueue(currentPoint, 0);

            var distances = new Dictionary<Point, float>();
            distances[currentPoint] = 0;

            while (pq.Count > 0)
            {

                var current = pq.Dequeue();

                if (current == _target.target)
                {
                    var path = new List<Point>();
                    while (current != currentPoint)
                    {
                        path.Add(current);
                        current = current.prev; 
                    }

                    path.Reverse();
                    nextPoint = path[0].transform;
                    currentPoint = path[0];
                    _isTargetNull = false;
                    return;
                }

                for (var i = 0; i < current.points.Length; i++)
                {
                    var connected = current.points[i];
                    var type = current.tipo[i];

                    if (type == Point.Tipo.Vaca)
                    {
                        continue;
                    }

                    var distance = distances[current] +
                                   Vector3.Distance(current.transform.position, connected.transform.position);
                    if (!distances.ContainsKey(connected) || distance < distances[connected])
                    {
                        distances[connected] = distance;
                        connected.prev = current;
                        pq.Enqueue(connected, distance);
                    }
                }
            }

            _isTargetNull = false;
        }
    }
}

#if UNITY_LINUX

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    private SerializedProperty _speed;
    private SerializedProperty _counter;
    private SerializedProperty _currentPoint;

    private void OnEnable()
    {
        _speed = serializedObject.FindProperty("speed");
        _counter = serializedObject.FindProperty("counter");
        _currentPoint = serializedObject.FindProperty("currentPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_speed, 0.1f, 10f, new GUIContent("Speed"));
        EditorGUILayout.IntSlider(_counter, 0, 10, new GUIContent("Counter"));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIContent targetLabel = new GUIContent("WayPoint Inicial");
        EditorGUILayout.PropertyField(_currentPoint,targetLabel);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif