using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float beatTempo;
    public Vector3 startPosition;
    public bool hasStarted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatTempo = beatTempo / 60f;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    { 
        if (hasStarted)
        {
            transform.position -= new Vector3(0, beatTempo * Time.deltaTime, 0f);
        if ((transform.position - startPosition).magnitude > 30f)
        {
                    transform.position = startPosition;
                   // beatTempo = Random.Range(1, 5);
        }
        }
    }
}
