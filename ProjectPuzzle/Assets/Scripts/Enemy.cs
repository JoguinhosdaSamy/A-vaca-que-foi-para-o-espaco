using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private GameController _controller;
    private Player _target;
    public Point currentPoint;
    public Transform nextPoint;
    private bool _isTargetNull;
    public float speed = 4.0f;
    public int counter;

    private void Start()
    {
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        _target = GameObject.Find("Player").GetComponent<Player>();
        _isTargetNull = _target == null;
        nextPoint = currentPoint.transform;
    }

    private void FixedUpdate()
    {
        if (_controller.movementStatus != GameController.Movement.Alien) return;
        if (_isTargetNull) return;
        if (counter != 0)
        {
            counter--;
            _target.target.ShowLights();
            _controller.SetMovementStatus(GameController.Movement.Vaca);
            _target.CheckPossibilities();
            return;
        }

        Vector3 dir = nextPoint.position - transform.position;
        if (dir.magnitude > 0.1f)
        {
            transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);
            Vector3 rot = dir.normalized;
            rot.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(dir.normalized);
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
        if (currentPoint == _target.target)
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
