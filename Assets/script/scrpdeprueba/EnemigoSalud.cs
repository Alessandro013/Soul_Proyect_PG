using UnityEngine;
using UnityEngine.UI;

public class EnemigoSalud : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float vidaMax = 10f;
    public float vidaActual;

    [Header("Interfaz de Usuario")]
    public Image barraRoja; // Arrastra la imagen con Fill Method: Horizontal

    void Start()
    {
        vidaActual = vidaMax;
        ActualizarBarra();
    }

    public void TomarDano(float cantidad)
    {
        vidaActual -= cantidad;
        
        // Aseguramos que la vida no baje de 0
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMax);
        
        ActualizarBarra();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void ActualizarBarra()
    {
        if (barraRoja != null)
        {
            // Calcula el porcentaje (de 0 a 1) para el Fill Amount
            barraRoja.fillAmount = vidaActual / vidaMax;
        }
    }

    void Morir()
    {
        // Aquí podrías activar una animación de muerte antes del Destroy
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}