using UnityEngine;

public class HabilidadClon : MonoBehaviour
{
    public GameObject clonPrefab;
    public float aumentoVelocidad = 2f;
    public float duracionHabilidad = 5f;
    
    private GameObject clonActual;
    private MovimientoJugador mov;
    private float velocidadOriginal;

    void Start() {
        mov = GetComponent<MovimientoJugador>();
        velocidadOriginal = mov.velocidad;
    }

    void Update() {
        // Presiona 'Q' para activar habilidad y clonar
        if (Input.GetKeyDown(KeyCode.Q) && clonActual == null) {
            ActivarHabilidad();
        }

        // Presiona 'E' para intercambiar lugar con el clon
        if (Input.GetKeyDown(KeyCode.E) && clonActual != null) {
            Intercambiar();
        }
    }

    void ActivarHabilidad() {
        // 1. Crear el clon en nuestra posición
        clonActual = Instantiate(clonPrefab, transform.position, Quaternion.identity);
        
        // 2. Volverse más rápido
        mov.velocidad = velocidadOriginal * aumentoVelocidad;
        
        // 3. Programar el fin de la velocidad extra
        Invoke("ResetearVelocidad", duracionHabilidad);
        
        // 4. El clon se destruye solo después de un tiempo
        Destroy(clonActual, duracionHabilidad);
    }

    void Intercambiar() {
        // Guardamos la posición del jugador
        Vector2 posJugador = transform.position;
        
        // El jugador va a la posición del clon
        transform.position = clonActual.transform.position;
        
        // El clon va a la posición donde estaba el jugador
        clonActual.transform.position = posJugador;
        
        Debug.Log("¡Intercambio de sombras!");
    }

    void ResetearVelocidad() {
        mov.velocidad = velocidadOriginal;
    }
}