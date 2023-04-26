using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public GameObject[] points;
    public Light light;

    private GameObject player;

    public float distanceThreshold = 2.1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        light.enabled = false;
    }

    void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(points[i].transform.position, player.transform.position);

            if (distance <= distanceThreshold)
            {
                light.enabled = true;
                break;
            }
            else
            {
                light.enabled = false;
            }
        }
    }
}
