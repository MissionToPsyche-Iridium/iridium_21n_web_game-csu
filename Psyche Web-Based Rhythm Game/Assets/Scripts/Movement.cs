using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    public Vector3 startPosition;
    private float movementDistance = 5f;
    private float buttonA;
    private float buttonS;
    private float buttonD;
    private float buttonF;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = rigidbody2D.transform.position;
        buttonS = startPosition.x; //use this variable as Vector3 can't use math functionsbecause it transform them into string resulting in rounding; example: 0.144+= 5 will reseult in 5 not 5.144
        buttonA = buttonS-movementDistance;
        buttonD = buttonS+movementDistance;
        buttonF = buttonD+movementDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rigidbody2D.transform.position = new Vector3(buttonA, startPosition.y, startPosition.z);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            rigidbody2D.transform.position = new Vector3(buttonS, startPosition.y, startPosition.z);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            rigidbody2D.transform.position = new Vector3(buttonD, startPosition.y, startPosition.z);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            rigidbody2D.transform.position = new Vector3(buttonF, startPosition.y, startPosition.z);
        }
        /*
        //rigidbody2D.linearVelocityY = 0;
        if(Input.GetKeyDown(KeyCode.LeftArrow) )
        {
           // rigidbody2D.AddForceX(-100, ForceMode2D.Force);
            rigidbody2D.transform.position = new Vector3(buttonA-=movementDistance, startPosition.y, startPosition.z);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) && rigidbody2D.linearVelocityX != 0)
        {
           // rigidbody2D.linearVelocityX = 0;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
           // rigidbody2D.AddForceX(100, ForceMode2D.Force);
           rigidbody2D.transform.position = new Vector3(buttonA+=movementDistance, startPosition.y, startPosition.z);
        }
         if(Input.GetKeyUp(KeyCode.RightArrow) && rigidbody2D.linearVelocityX != 0)
        {
          //  rigidbody2D.linearVelocityX = 0;
        }*/
    }
}
