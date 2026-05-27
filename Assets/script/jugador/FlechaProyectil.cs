using UnityEngine;

public class FlechaProyectil : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidadBase = 10f;
    public float velocidadHoming = 15f; // Más rápida si persigue

    [Header("Configuración de Homing (Persecución)")]
    public float radioDeteccion = 5f; // Qué tan lejos busca enemigos
    public LayerMask capaEnemigo; // La capa donde están los enemigos

    [Header("Configuración de Rotación")]
    public float velocidadRotacion = 500f; // Qué tan rápido gira hacia el enemigo

    private Rigidbody2D rb;
    private Transform objetivoEnemigo;
    private Vector2 direccionBase;

void Start()
{
    rb = GetComponent<Rigidbody2D>();
    
    // IMPORTANTE: La dirección base ahora es hacia donde apunta la flecha al nacer
    direccionBase = transform.right; 

    objetivoEnemigo = EncontrarEnemigoMasCercano();
    Destroy(gameObject, 5f);
}

    void FixedUpdate()
    {
        // 4. Lógica de movimiento
        if (objetivoEnemigo != null)
        {
            // --- MOVIMIENTO HOMING (PERSIGUIENDO) ---
            Vector2 direccionHaciaObjetivo = (Vector2)objetivoEnemigo.position - rb.position;
            direccionHaciaObjetivo.Normalize();

            // Calcular cuánto debe girar la flecha
            float anguloGiro = Vector3.Cross(direccionHaciaObjetivo, transform.right).z;
            rb.angularVelocity = -anguloGiro * velocidadRotacion;

            // Mover la flecha hacia adelante
            rb.linearVelocity = transform.right * velocidadHoming;
        }
        else
        {
            // --- MOVIMIENTO RECTO ---
            // Si no hay objetivo, ir recto y quitar la rotación angular
            rb.linearVelocity = direccionBase * velocidadBase;
            rb.angularVelocity = 0;
        }
    }

    private Transform EncontrarEnemigoMasCercano()
    {
        // Detectar todos los enemigos cercanos en un círculo
        Collider2D[] enemigosCercanos = Physics2D.OverlapCircleAll(transform.position, radioDeteccion, capaEnemigo);
        
        Transform masCercano = null;
        float distanciaMasCercana = Mathf.Infinity;

        // Iterar para encontrar el que esté a menor distancia
        foreach (Collider2D enemigo in enemigosCercanos)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                masCercano = enemigo.transform;
            }
        }
        return masCercano;
    }

    // --- DETECCIÓN DE COLISIONES Y DESTRUCCIÓN ---
private void OnTriggerEnter2D(Collider2D other)
{
    // 1. Ignorar si choca con el jugador o con otra flecha
    if (other.CompareTag("Player") || other.CompareTag("Proyectil")) 
    {
        return; 
    }

    // 2. Si choca con un enemigo
    if (other.CompareTag("Enemigo"))
    {
        other.SendMessage("RecibirDanio", 1, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
    // 3. Si choca con una pared o suelo
    else if (other.CompareTag("Pared") || other.CompareTag("ObjetoSolido"))
    {
        Destroy(gameObject);
    }
}

    // Dibujar el radio de detección en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}