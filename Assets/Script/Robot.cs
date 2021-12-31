using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float ladderSpeed;


    private bool isMoving;
    private Rigidbody2D rb;
    private Animator animator;
    private new SpriteRenderer renderer;
    private int direction;

    //Behavior Values: 0-Acts Normally/-1-Charges are different/ Should Attract/ 1- Charges are same should repel
    public int behavior;
    public bool onLadder;
    public bool isGrounded;
    public int chargeState;

    public static bool isClimbing;
    public static bool isJumping;

    // Start is called before the first frame update
    void Start()
    {

        isMoving = false;
        onLadder = false;
        isClimbing = false;
        isJumping = false;
        isGrounded = true;
        chargeState = 0;
        behavior = 0;
        //Facing Right
        direction = 1;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }
    //Update should handle any player input
    void Update()
    {
        
        ChangeChargeState();
        ClimbLadder();
        Move();
    }
    //Fixed Update handles results of inputs
    private void FixedUpdate()
    {
        Jump();
        AnimateMove();
        DisplayChargeEffects(chargeState);

    }

   
    /*
     * Moves player up the ladder by pressing 'W' or down the ladder by pressing 'S'
     */
    private void ClimbLadder()
    {
        //Moves player up the ladder when "W" is pressed in front of ladder
        if (onLadder)
        {

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                animator.speed = 0;
                isClimbing = false;

            }
            if (Input.GetKey(KeyCode.W))
            {
                animator.speed = 1;
                rb.constraints =  RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = new Vector2(0, ladderSpeed);
                isClimbing = true;
                

            }

            if (Input.GetKey(KeyCode.S))
            {
                    isClimbing = true;
                    animator.speed = 1;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = new Vector2(0, -ladderSpeed);
               



            }
            if(isClimbing && !isGrounded)
            {
                animator.Play("Climb");
            }

        }
        if(!onLadder)
        {
            isClimbing = false;
        }
        

    }

   
  

    //Enables Jump functionality
    //Can only jump if isGrounded is true
    //@param rb The rigidbody that will be jumping
    //Note: May change W key later for ladder functionality
    private void Jump()
    {
        if (isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            isJumping = true;
           if(!isMoving)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            //Note: ForceMode2D.Impulse makes the jumps consistant height
            rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            float currentVelocity = rb.velocity.magnitude;

            if (currentVelocity >= jumpHeight)
            {
                float reduction = jumpHeight / currentVelocity;
                rb.velocity *= reduction;
            }

        }
        

        AdjustAirAnimations();
    }

    //Changes current animations if the player is airborne
    private void AdjustAirAnimations()
    {
        if (!isClimbing)
        {
            if (!isGrounded && rb.velocity.y > 0f)
            {
                animator.Play("Jump_Up");
                isJumping = true;
            }
            if (!isGrounded && rb.velocity.y < 0f)
            {
                animator.Play("Fall");
                isJumping = false;
            }
        }

    }


    //Enables left(A) to right(D) movement and animations.
    //Plays Idle animation if no key is pressed or if 'A' and 'D' are pressed at the same time
   
    private void Move()
    {
        

        //Idle if both left and right are pressed
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            
            isMoving = false;

        }
        //Idle if on the ground and not moving
        
        //Flips sprite and moves left when A is pressed
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            isMoving = true;
            direction = -1;
            rb.velocity = new Vector2(-speed,rb.velocity.y);
        }
        //Moves right and D is pressed
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            isMoving = true;
            direction = 1;
            rb.velocity = new Vector2(speed, rb.velocity.y);

        }
        //Checks if the 'A' or 'D' key was released. If so, sets 'isMoving' to false
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            
            isMoving = false;

            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }


    }
    /*
     * Plays 'Idle' animation if player is not moving or 'Run' animation if player is moving.
     */
    private void AnimateMove()
    {
        if (!isMoving && isGrounded && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space)))
        {

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.Play("Idle");
            animator.speed = 0;

        }
        if (isMoving)
        {
            animator.speed = 1;
            if (direction < 0)
            {
                renderer.flipX = true;
            }
            if (direction > 0)
            {
                renderer.flipX = false;
            }
            if (isGrounded)
            {
                animator.Play("Run");
            }
        }
    }

    //Toggles the Players current Magnetic Charge
    //Left Mouse Button toggles Positive Charge
    //Right Mouse Button toggles Negative Charge
    private void ChangeChargeState()
    {
        //Keeps current charge when key is held down, or normal when neither are held down
        /*if(Input.GetKey(KeyCode.Q))
        {
           chargeState = "Positive";
            
        }
        
        else if (Input.GetKey(KeyCode.E))
        {
            chargeState = "Negative";
           
        }
        else
        {
            chargeState = null;
        }

        DisplayChargeEffects(chargeState);
        Debug.Log("Current Charge: " + chargeState);*/

        if (Level_Controller.isPaused == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (chargeState != 1)
                {
                    chargeState = 1;
                }
                else if (chargeState == 1)
                {
                    chargeState = 0;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (chargeState != -1)
                {
                    chargeState = -1;
                }
                else if (chargeState == -1)
                {
                    chargeState = 0;
                }
            }
        }


   



}

//Displays the current charge effect on the Player
//Currently changes the players color to reflect their current charge
//@param state The players current state, Positive(1), Negative(-1), or Neutral(0)
private void DisplayChargeEffects(int state)
{
switch (state)
{
    case 0:
        renderer.color = Color.white;
        break;
    case 1:
        renderer.color = Color.blue;

        break;
    case -1:
        renderer.color = Color.red;

        break;
}
}
    /*
     * Gets the player's current charge
     * @return chargeState The player's charge states:-1,0, or 1
     */
    public int GetCharge()
{
return chargeState;
}

    /*
     * Gets the player's current magnetic behavior
     * @return behavior The player's magnetic behavior: -1: Player is attracted to current object/ 0: Player behaves normal/ 1: Player is repelled by current object
     */
public int GetBehavior()
{
return behavior;
}

    /*
     * Set the player's magnetic behavior
     */
public void SetBehavior(int state)
{
behavior = state;
}


   


}
