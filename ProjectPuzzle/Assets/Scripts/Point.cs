using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Point : MonoBehaviour
{
    public enum Tipo {Neutro, Alien, Vaca};
    public enum Prop {None, EndPoint, SleepPower};
    public Point[] points;
    public Tipo[] tipo;
    private List<GameObject> lines = new List<GameObject>();
    public GameObject linhasPrefab;
    private readonly Color _redLine = Color.red;
    private readonly Color _yellowLine = Color.yellow;
    private GameController controller;
    public Prop Property;
    public Point prev; // novo atributo




    private void OnDrawGizmos()
    {
        for (var i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.white;
            switch (tipo[i])
            {
                case Tipo.Alien:
                    Gizmos.color = _redLine;
                    break;
                case Tipo.Vaca:
                    Gizmos.color = _yellowLine;
                    break;
                    
            }
            Gizmos.DrawLine(transform.position, points[i].transform.position);
        }
    }
    
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        
        for (var i = 0; i < points.Length; i++)
        {
            var transform1 = transform;
            var position = transform1.position;
            
            GameObject linha = Instantiate(linhasPrefab, position, Quaternion.identity, transform1);
            LineRenderer lineRenderer = linha.GetComponent<LineRenderer>();
            switch (tipo[i])
            {
                case Tipo.Alien:
                    lineRenderer.SetColors(_redLine, _redLine);
                    break;
                case Tipo.Vaca:
                    lineRenderer.SetColors(_yellowLine, _yellowLine);
                    break;
                    
            }

            lines.Add(linha);
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, points[i].transform.position);
        }
    }


    void OnMouseDown()
    {
        if(controller.MovementStatus == GameController.Movement.Vaca){
            Point[] listaPoints = Player.player.target.GetComponent<Point>().points;
            Tipo[] listaTarget = Player.player.target.GetComponent<Point>().tipo;
            for (var i = 0; i < listaPoints.Length; i++){
                if (listaPoints[i].transform == transform){
                    if (listaTarget[i] == Tipo.Alien)
                    {
                        return;
                    }
                    controller.SetMovementStatus(GameController.Movement.Moving);
                    Player.player.target = this;
                    Player.player._istargetNull = false;
                }
            }
        }
    }
    
    public void UpdateConnectedPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].prev == this)
            {
                points[i].tipo[i] = tipo[i];
                points[i].UpdateConnectedPoints();
            }
        }
    }
}
