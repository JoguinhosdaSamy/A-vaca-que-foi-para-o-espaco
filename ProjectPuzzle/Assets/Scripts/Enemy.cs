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
            transform.Translate(dir.normalized * (3.0f * Time.deltaTime), Space.World);
        }
        else
        {
            isTargetNull = true;
            controller.SetMovementStatus(GameController.Movement.Vaca);
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

            // Initialize the priority queue
            var pq = new PriorityQueue<Point>();
            pq.Enqueue(currentPoint, 0);

            // Initialize the distances
            var distances = new Dictionary<Point, float>();
            distances[currentPoint] = 0;

            while (pq.Count > 0)
            {
                // Get the next point to visit
                var current = pq.Dequeue();

                // Check if we've reached the target
                if (current == target.target)
                {
                    // Reconstruct the path and set the nextPoint
                    var path = new List<Point>();
                    while (current != currentPoint)
                    {
                        path.Add(current);
                        current = current.prev; // atualiza o "prev" para o próximo ponto no caminho
                    }

                    path.Reverse();
                    nextPoint = path[0].transform;
                    currentPoint = path[0];
                    isTargetNull = false;
                    return;
                }

                // Visit each connected point
                for (var i = 0; i < current.points.Length; i++)
                {
                    var connected = current.points[i];
                    var type = current.tipo[i];

                    // Check if the connection allows the enemy to move through it
                    if (type == Point.Tipo.Vaca)
                    {
                        continue;
                    }

                    // Calculate the distance and update the distances and the priority queue
                    var distance = distances[current] +
                                   Vector3.Distance(current.transform.position, connected.transform.position);
                    if (!distances.ContainsKey(connected) || distance < distances[connected])
                    {
                        distances[connected] = distance;
                        connected.prev = current; // atualiza o "prev" para o ponto atual
                        pq.Enqueue(connected, distance);
                    }
                }
            }

            isTargetNull = false;
        }
    }
}