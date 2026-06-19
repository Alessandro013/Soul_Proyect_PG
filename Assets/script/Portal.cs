using System.Collections; // ¡Súper importante para usar Corrutinas!
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nombreEscenaDestino; 
    public AudioSource sonidoPortal; 
    
    // Un candado de seguridad: evita que el portal se active 60 veces por segundo 
    // mientras el jugador esté parado sobre el cuadrito verde.
    private bool teletransportando = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el que choca es el Player y el portal NO se está activando ya...
        if (collision.CompareTag("Player") && !teletransportando) 
        {
            teletransportando = true; // Cerramos el candado
            StartCoroutine(ViajeConSonido()); // Iniciamos la secuencia
        }
    }

    // Esta es la corrutina que maneja el tiempo
    private IEnumerator ViajeConSonido()
    {
        // 1. Reproducimos el efecto de sonido de teletransporte
        if (sonidoPortal != null)
        {
            sonidoPortal.Play();
            
            // 2. Le decimos a Unity: "Espera aquí el tiempo exacto que dura este audio"
            yield return new WaitForSeconds(sonidoPortal.clip.length);
        }
        else
        {
            // Si por alguna razón olvidaste poner el audio, igual espera medio segundo
            yield return new WaitForSeconds(0.5f); 
        }

        // 3. ¡Boom! Ahora sí cambiamos de nivel de forma segura
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}