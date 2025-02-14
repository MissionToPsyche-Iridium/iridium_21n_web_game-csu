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
        double timeSinceInstantiated = Manager.getAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (Manager.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else{
            transform.localPosition = Vector3.Lerp(Vector3.up * Manager.Instance.spawnYCoordinate,Vector3.up * Manager.Instance.despawnYCoordinate, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
