using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rigidbody2D.linearVelocityY = 0;
        if(Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            rigidbody2D.AddForceX(-100, ForceMode2D.Force);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) && rigidbody2D.linearVelocityX != 0)
        {
            rigidbody2D.linearVelocityX = 0;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            rigidbody2D.AddForceX(100, ForceMode2D.Force);
        }
         if(Input.GetKeyUp(KeyCode.RightArrow) && rigidbody2D.linearVelocityX != 0)
        {
            rigidbody2D.linearVelocityX = 0;
        }
    }
}
