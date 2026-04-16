using UnityEngine;
using UnityEngine.InputSystem;



public class MA_PlayerControl : MonoBehaviour
{
    
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rig;

    Animator animatorController;
    private bool isGrounded=true;

    private float xInput;
    private bool jumpRequested;
    
    public SpriteRenderer sr;


      // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // animatorController = GetComponent<Animator>();
    }

    public enum PlayerAnimation
    {
    idle, walk, jump
    }

    void UpdatePlayerAnimation()
    {
    if (!isGrounded)
        UpdateAnimation(PlayerAnimation.jump);
    else if (xInput != 0)
        UpdateAnimation(PlayerAnimation.walk);
    else
        UpdateAnimation(PlayerAnimation.idle);
    }

    void UpdateAnimation(PlayerAnimation nameAnimation)
    {
    switch (nameAnimation)
    {
        case PlayerAnimation.idle:
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", false);
            break;

        case PlayerAnimation.walk:
            animatorController.SetBool("isWalking", true);
            animatorController.SetBool("isJumping", false);
            break;

        case PlayerAnimation.jump:
            animatorController.SetBool("isWalking", false);
            animatorController.SetBool("isJumping", true);
            break;
    }
    }

    // Update is called once per frame
    void Update()
    {
        xInput = 0f;
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            xInput = -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            xInput = 1f;
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
            isGrounded = false;
        }


        if(xInput != 0f)
        {
            sr.flipX=xInput<0;
        }
        

       // UpdatePlayerAnimation();
        
    }

    public void FixedUpdate()
    {


        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y);
        


        if (jumpRequested)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }


    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            isGrounded = false;
        }
    }



    


}
