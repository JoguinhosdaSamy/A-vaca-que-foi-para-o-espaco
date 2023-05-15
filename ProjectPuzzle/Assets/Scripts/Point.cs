using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Point : MonoBehaviour
{
    public enum Tipo {Neutro, Alien, Vaca};
    public enum Prop {None, EndPoint, SleepPower};
    public Point[] points;
    public Tipo[] tipo;
    private List<GameObject> _lines = new List<GameObject>();
    public GameObject linhasPrefab;
    private readonly Color _redLine = Color.red;
    private readonly Color _yellowLine = Color.yellow;
    private GameController _controller;
    [FormerlySerializedAs("Property")] public Prop property;
    public Point prev; // novo atributo

    public Material materialAceso;
    public Material materialApagado;
    [HideInInspector]public MeshRenderer _meshRenderer;

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
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
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

            _lines.Add(linha);
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, points[i].transform.position);
        }
        
    }


    void OnMouseDown()
    {
        if(_controller.movementStatus == GameController.Movement.Vaca){
            Point[] listaPoints = Player.player.target.GetComponent<Point>().points;
            Tipo[] listaTarget = Player.player.target.GetComponent<Point>().tipo;
            for (var i = 0; i < listaPoints.Length; i++){
                
                Debug.Log(transform);
                if (listaPoints[i].transform == transform){
                    if (listaTarget[i] == Tipo.Alien)
                    {
                        return;
                    }

                    HideLights();
                    _controller.SetMovementStatus(GameController.Movement.Moving);
                    Player.player.target = this;
                    Player.player.istargetNull = false;
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

    public void ShowLights()
    {
        for (var i = 0; i < points.Length; i++)
        {
            
            if (tipo[i] == Tipo.Alien)
            {
                return;
            }

            points[i]._meshRenderer.material = points[i].materialAceso;
        }
    }

    private void HideLights()
    {
        Point[] points = FindObjectsOfType<Point>();

        foreach (Point point in points)
        {
            point._meshRenderer.material = point.materialApagado;
        }
    }
}
