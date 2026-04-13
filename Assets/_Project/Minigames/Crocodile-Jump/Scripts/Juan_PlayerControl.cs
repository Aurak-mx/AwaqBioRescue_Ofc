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

    private float xInput;
    private bool jumpRequested;
    private bool isGrounded = true;

    void Update()
    {
        // 1. Lectura de movimiento horizontal
        xInput = 0f;
        if (Keyboard.current.dKey.isPressed)
        {
            xInput = 1f;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            xInput = -1f;
        }

        // 2. Voltear el sprite según la dirección
        if (xInput != 0)
        {
            sr.flipX = xInput < 0;
        }

        // 3. Lectura de salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento horizontal manteniendo la velocidad vertical actual
        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y);

        // Aplicar salto
        if (jumpRequested)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        // Detección simple de suelo (basada en velocidad vertical)
        if (Mathf.Abs(rig.linearVelocity.y) < 0.01f)
        {
            isGrounded = true;
        }
    }
}