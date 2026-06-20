using UnityEngine;
using UnityEngine.UI;

public class EnemigoSalud : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaMax = 10f;
    public float vidaActual;

    [Header("Interfaz de Usuario")]
    public Image barraRoja; 

    // Referencia interna a la IA
    private InteligenciaEnemigo ia;

    void Start()
    {
        vidaActual = vidaMax;
        ia = GetComponent<InteligenciaEnemigo>();
        
        // Sincronizamos la IA al inicio
        if (ia != null) ia.vidaActual = vidaMax; 
        
        ActualizarBarra();
    }

    public void TomarDano(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMax);
        
        ActualizarBarra();

        // 1. Le pasamos la vida real a la IA para que sepa si debe huir o morir
        if (ia != null) 
        {
            ia.vidaActual = this.vidaActual;
        }

        // 2. Si la vida llega a 0, preparamos el "cadáver"
        if (vidaActual <= 0)
        {
            // Apagamos el collider para que el jugador no lo siga golpeando ni chocando
            GetComponent<Collider2D>().enabled = false;
            
            // Ocultamos la barra de vida flotante
            if (barraRoja != null) 
            {
                barraRoja.transform.parent.gameObject.SetActive(false);
            }
            
            // NOTA: Ya no usamos Destroy() aquí. La IA se encargará de hacer la animación
            // y destruirlo después de 2 segundos gracias a su propio Update().
        }
    }

    void ActualizarBarra()
    {
        if (barraRoja != null)
        {
            barraRoja.fillAmount = vidaActual / vidaMax;
        }
    }
}