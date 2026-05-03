using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float movespeed = 2f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        // --- BLOQUEO DE DIÁLOGO ---
        // Si el manager dice que estamos hablando, no hacemos nada más en este script
        if (DialogueManager.estaHablando) {
            moveInput = Vector2.zero; // Detenemos cualquier input previo
            anim.SetFloat("speed", 0f); // Ponemos la animación en Idle
            return; // Salimos del Update para que no detecte teclas
        }

        // Movimiento WASD
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Actualizar velocidad en Animator
        anim.SetFloat("speed", moveInput.sqrMagnitude);

        // --- ATAQUES (Ahora bloqueados si hay diálogo) ---
        if (Input.GetKeyDown(KeyCode.Space)) {
            anim.SetTrigger("attack"); 
        }

        if (Input.GetMouseButtonDown(0)) {
            anim.SetTrigger("attack2"); 
        }

        // Cambiado a la tecla C si prefieres unificar con el diálogo, 
        // pero mantengo E si es solo para el arco.
        if (Input.GetKeyDown(KeyCode.E)) {
            anim.SetTrigger("attackArco");
        }

        // --- ROTACIÓN ---
        if (moveInput.x > 0) 
            transform.localScale = new Vector3(3.25f, 2.88f, 1f);
        else if (moveInput.x < 0) 
            transform.localScale = new Vector3(-3.25f, 2.88f, 1f);
    }

    void FixedUpdate() {
        // Si el tiempo está congelado (Time.timeScale = 0), esto no moverá al jugador
        rb.MovePosition(rb.position + moveInput.normalized * movespeed * Time.fixedDeltaTime);
    }
}