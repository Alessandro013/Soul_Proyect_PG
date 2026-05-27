using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float movespeed = 5f; // Aumentada un poco, 2f suele ser muy lento

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        // --- BLOQUEO DE DIÁLOGO ---
        if (DialogueManager.estaHablando) 
        {
            moveInput = Vector2.zero; 
            anim.SetFloat("speed", 0f);
            return; 
        }

        // Obtener entrada de movimiento (WASD / Flechas)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Actualizar el parámetro "speed" en el Animator para las animaciones Walk/Idle
        // Usamos sqrMagnitude porque es más eficiente que calcular la magnitud real
        anim.SetFloat("speed", moveInput.sqrMagnitude);

        // --- ROTACIÓN (Girar el Sprite) ---
        // Solo cambia el scale si el jugador se está moviendo horizontalmente
        if (moveInput.x > 0) 
        {
            transform.localScale = new Vector3(3.25f, 2.88f, 1f);
        }
        else if (moveInput.x < 0) 
        {
            transform.localScale = new Vector3(-3.25f, 2.88f, 1f);
        }
    }

    void FixedUpdate() 
    {
        // Aplicar el movimiento al Rigidbody2D
        // Usamos normalized para que el movimiento diagonal no sea más rápido
        rb.MovePosition(rb.position + moveInput.normalized * movespeed * Time.fixedDeltaTime);
    }
}