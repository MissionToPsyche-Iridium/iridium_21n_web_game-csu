using UnityEngine;

public class Notes: MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    void Start()
    {
        timeInstantiated = Manager.getAudioSourceTime();
    }

    void Update()
    {
        double timeSinceStart = Manager.getAudioSourceTime() - timeInstantiated;
        float timeFactor = (float)(timeSinceStart / (1 * 2)); //greater than 1 slows the notes down. Do not change

        if (timeFactor > 1)
        {
            Destroy(gameObject);
        }
        else{
            transform.localPosition = Vector3.Lerp(Vector3.up * Manager.Instance.spawnYCoordinate,Vector3.up * Manager.Instance.despawnYCoordinate, timeFactor);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
