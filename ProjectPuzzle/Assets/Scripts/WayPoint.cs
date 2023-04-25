using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public float radius = 5f;
    public Light light;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        light.enabled = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= radius)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
    }
}
