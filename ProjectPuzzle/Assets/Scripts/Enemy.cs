using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private GameController controller;
    private Player target;
    public Point currentPoint;
    public Transform nextPoint;
    private bool isTargetNull;
    public float speed = 4.0f;
    public int counter;

    private void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        target = GameObject.Find("Player").GetComponent<Player>();
        isTargetNull = target == null;
        nextPoint = currentPoint.transform;
    }

    private void FixedUpdate()
    {
        if (controller.MovementStatus != GameController.Movement.Alien) return;
        if (isTargetNull) return;
        if (counter != 0)
        {
            counter--;
            controller.SetMovementStatus(GameController.Movement.Vaca);
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
            isTargetNull = true;
            //verificar se a Vaca está em um ponto, e o único caminho possível não é um ponto ocupado pelo Alien 
            controller.SetMovementStatus(GameController.Movement.Vaca);
            // caso contrário, mais um turno para o alien

        }
    }

    private void CheckDie()
    {
        if (currentPoint == target.target)
        {
            controller.GameOver();
        }
    }

    public void FindShortestPath()
    {
        if (counter == 0)
        {
            if (target.target == currentPoint)
            {
                controller.SetMovementStatus(GameController.Movement.Vaca);
                return;
            }
            
            var pq = new PriorityQueue<Point>();
            pq.Enqueue(currentPoint, 0);

            var distances = new Dictionary<Point, float>();
            distances[currentPoint] = 0;

            while (pq.Count > 0)
            {

                var current = pq.Dequeue();

                if (current == target.target)
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
                    isTargetNull = false;
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

            isTargetNull = false;
        }
    }
}
