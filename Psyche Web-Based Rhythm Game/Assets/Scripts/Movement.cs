using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    public Vector3 startPosition;
    public Vector3 startScale;
    private float movementDistance = 5f;
    private float buttonA;
    private float buttonS;
    private float buttonD;
    private float buttonF;
    private float duration;
    
    public Vector3 targetScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        startPosition = rigidbody2D.transform.position;
        buttonS = startPosition.x; //use this variable as Vector3 can't use math functionsbecause it transform them into string resulting in rounding; example: 0.144+= 5 will reseult in 5 not 5.144
        buttonA = buttonS-movementDistance;
        buttonD = buttonS+movementDistance;
        buttonF = buttonD+movementDistance;
        duration = .20f;
        startScale = rigidbody2D.transform.localScale;
        targetScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        
    }

// rotates the spaceship along its x-axis for .2 seconds. 
     private IEnumerator ScaleTransform()
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            // Wait for the next frame
            yield return null;
        }
    }

// resets the x local scale of the spaceship to the original to allow an "animation" of the it rotation on its x axis. 
    void resetScale()
    {
        rigidbody2D.transform.localScale = startScale;
    }

// Updates movement of the spaceship according to the button pressed. The button is passed as a parameter. 
    void updateMovement(float direction)
    {
        rigidbody2D.transform.position = new Vector3(direction, startPosition.y, startPosition.z);
    }

    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            updateMovement(buttonA);
            resetScale();
            StartCoroutine(ScaleTransform());        
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            updateMovement(buttonS);
            resetScale();
            StartCoroutine(ScaleTransform());  
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            updateMovement(buttonD);
            resetScale();
            StartCoroutine(ScaleTransform());  
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            updateMovement(buttonF);
            resetScale();
            StartCoroutine(ScaleTransform());  
        }
    }
}
