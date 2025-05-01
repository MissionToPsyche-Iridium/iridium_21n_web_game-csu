using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    public Vector3 startPosition;
    public Vector3 startScale;

    private GameObject lane1;
    private GameObject lane2;
    private GameObject lane3;
    private GameObject lane4;
    private float buttonA;
    private float buttonS;
    private float buttonD;
    private float buttonF;
    private float duration;
    public Vector3 targetScale;

    void Start()
    {       
        startPosition = rigidbody2D.transform.position;
        duration = .20f;
        startScale = rigidbody2D.transform.localScale;
        targetScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        lane1 = GameObject.Find("Lane1"); 
        lane2 = GameObject.Find("Lane2");
        lane3 = GameObject.Find("Lane3");
        lane4 = GameObject.Find("Lane4");
        buttonA = lane1.transform.position.x;
        buttonD = lane3.transform.position.x;
        buttonF = lane4.transform.position.x;
        buttonS = lane2.transform.position.x;
    }

     private IEnumerator ScaleTransform()
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
    }

    public void resetScale()
    {
        rigidbody2D.transform.localScale = startScale;
    }

    public void updateMovement(float direction)
    {
        
        rigidbody2D.transform.position = new Vector3(direction, startPosition.y, startPosition.z);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            updateMovement(buttonA);
            resetScale();
            StartCoroutine(ScaleTransform());   
            Manager.Instance.useFuel();    
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            updateMovement(buttonS);
            resetScale();
            StartCoroutine(ScaleTransform());  
            Manager.Instance.useFuel(); 
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            updateMovement(buttonD);
            resetScale();
            StartCoroutine(ScaleTransform());  
            Manager.Instance.useFuel(); 
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            updateMovement(buttonF);
            resetScale();
            StartCoroutine(ScaleTransform());  
            Manager.Instance.useFuel(); 
        }
    }
}
