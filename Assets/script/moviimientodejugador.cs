using UnityEngine;

public partial class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 entrada;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento básico con WASD o Flechas
        entrada.x = Input.GetAxisRaw("Horizontal");
        entrada.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Aplicamos el movimiento al Rigidbody
        rb.MovePosition(rb.position + entrada.normalized * velocidad * Time.fixedDeltaTime);
    }
}