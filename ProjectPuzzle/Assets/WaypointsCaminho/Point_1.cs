using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_1 : MonoBehaviour
{
    //public enum Tipo {ALIEN, VACA, NEUTRO};
    //public Tipo tipo;
    public Transform[] points;
    public GameObject linhasPrefab;

    

    void Start(){
        
        for (int i = 0; i < points.Length; i++){
            GameObject linha = Instantiate(linhasPrefab, transform.position, Quaternion.identity, transform);
            LineRenderer lineRenderer = linha.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, points[i].position);
        }
    }


    void OnMouseDown() {
        Transform[] lista = Player_1.player.target.GetComponent<Point>().points;
        for (int i = 0; i < lista.Length; i++){
            if (lista[i] == transform){
                Player_1.player.target = gameObject.transform;
            }
        }
        
    }


}
