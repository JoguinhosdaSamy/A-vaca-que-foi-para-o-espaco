using UnityEngine;

public class Point : MonoBehaviour
{
    public enum Tipo {Neutro, Alien, Vaca};
    public Transform[] points;
    public Tipo[] tipo;
    public GameObject linhasPrefab;
    private readonly Color _redLine = Color.red;
    private readonly Color _yellowLine = Color.yellow;

    

    void Start()
    {
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
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, points[i].position);
        }
    }


    void OnMouseDown()
    {
        Transform[] lista = Player.player.target.GetComponent<Point>().points;
        for (var i = 0; i < lista.Length; i++){
            if (lista[i] == transform){
                Player.player.target = gameObject.transform;
            }
        }
    }
}