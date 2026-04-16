using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

// Script para controlar el movimiento, salto y animaciones del jugador
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

    // Inicializa el componente Animator
    void Start()
    {
        animatorController = GetComponent<Animator>();
    }
    
    void Update()
    {
        
        xInput = 0f;
        if (Keyboard.current.dKey.isPressed)
        {
            xInput = 1f; // Mover derecha
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            xInput = -1f; // Mover izquierda
        }

        // Voltear sprite según dirección
        if (xInput != 0)
        {
            sr.flipX = xInput > 0; // Mirar derecha si xInput positivo
        }

        // Solicitar salto si está en el suelo y se presiona espacio o W
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded || Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
            isGrounded = false; 
        }

        UpdatePlayerAnimation(); // Actualizar animaciones
    }

    // Aplica física en FixedUpdate para consistencia
    void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y); // Aplicar velocidad horizontal

        if (jumpRequested)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar fuerza de salto
            jumpRequested = false; // Resetear solicitud
        }

        // Verificar si está grounded (velocidad Y cercana a cero)
        if (Mathf.Abs(rig.linearVelocity.y) < 0.01f)
        {
            isGrounded = true;
        }
    }

    // Estados de animación del jugador
    public enum PlayerAnimationState
    {
        Idle, 
        Walking, 
        Jumping 
    }

    // Determina el estado de animación basado en el estado del jugador
    void UpdatePlayerAnimation()
    {
        if (!isGrounded)
        {
            UpdateAnimation(PlayerAnimationState.Jumping); // Saltando si no está grounded
        }
        else if (xInput != 0)
        {
            UpdateAnimation(PlayerAnimationState.Walking); // Caminando si hay entrada horizontal
        }
        else
        {
            UpdateAnimation(PlayerAnimationState.Idle); // Idle si no hay movimiento
        }
    }

    // Actualiza los parámetros del Animator según el estado
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