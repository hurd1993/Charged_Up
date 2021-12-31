using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_GroundCheck : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask ladderLayerMask;

    GameObject Robot;
    private BoxCollider2D boxCol;
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        Robot = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckToClimb();
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("Spring"))
            {
            
                 if (Robot.GetComponent<Robot>().GetBehavior() == 0)
                 {
                    Robot.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                 }
                if (Robot.GetComponent<Robot>().GetBehavior() == 1)
                {
                    Robot.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1.5f*jumpForce), ForceMode2D.Impulse);
                }
                if (Robot.GetComponent<Robot>().GetBehavior() == -1)
                {
                     Robot.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce/2), ForceMode2D.Impulse);
                }

            }
        
    }

    
    


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Robot.GetComponent<Robot>().isGrounded = true;
            

        }
        if(collision.collider.CompareTag("Ladder_Platform"))
        {
            Robot.GetComponent<Robot>().isGrounded = true;


        }
            

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Ladder_Platform"))
        {
            Robot.GetComponent<Robot>().isGrounded = false;
        }
        


    }

    private void CheckToClimb()
    {
        RaycastHit2D ladderRayCast = Physics2D.Raycast(boxCol.bounds.center, Vector2.down, boxCol.bounds.extents.y, ladderLayerMask);

        if (ladderRayCast.collider != null)
        {
            Robot.GetComponent<Robot>().onLadder = true;
        }
        else
        {
            Robot.GetComponent<Robot>().onLadder = false;
        }
    }
   

}

