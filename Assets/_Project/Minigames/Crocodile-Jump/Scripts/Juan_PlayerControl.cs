using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Juan_PlayerControl : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Referencias")]
    public Rigidbody2D rig;
    public SpriteRenderer sr;
    Animator animatorController;
    private float xInput;
    private bool jumpRequested;
    private bool isGrounded = true;

    void Start()
    {
        animatorController = GetComponent<Animator>();
    }
    void Update()
    {
        // Movimiento horizontal
        xInput = 0f;
        if (Keyboard.current.dKey.isPressed)
        {
            xInput = 1f;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            xInput = -1f;
        }

        // Voltear sprite
        if (xInput != 0)
        {
            sr.flipX = xInput > 0;
        }

        // Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded || Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
            isGrounded = false;
        }

        UpdatePlayerAnimation();
    }

    void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y);

        if (jumpRequested)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        if (Mathf.Abs(rig.linearVelocity.y) < 0.01f)
        {
            isGrounded = true;
        }
    }

    public enum PlayerAnimationState
    {
        Idle,
        Walking,
        Jumping
    }

    void UpdatePlayerAnimation()
    {

        if (!isGrounded)
        {
            UpdateAnimation(PlayerAnimationState.Jumping);
        }
        else if (xInput != 0)
        {
            UpdateAnimation(PlayerAnimationState.Walking);
        }
        else
        {
            UpdateAnimation(PlayerAnimationState.Idle);
        }
    }

    void UpdateAnimation(PlayerAnimationState nameAnimation)
    {
        switch(nameAnimation)
        {
            case PlayerAnimationState.Idle:
                animatorController.SetBool("isWalking", false); 
                animatorController.SetBool("isJumping", false);
                break;
            case PlayerAnimationState.Walking:
                animatorController.SetBool("isWalking", true);
                animatorController.SetBool("isJumping", false);
                break;
            case PlayerAnimationState.Jumping:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", true);
                break;
        }
    }
}