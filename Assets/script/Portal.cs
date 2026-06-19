using UnityEngine;
using UnityEngine.SceneManagement; // Vital para poder cambiar de nivel

public class Portal : MonoBehaviour
{
    // Esta variable ya la tenías, aquí pondrás "bastion_de_froda" en el Inspector
    public string nombreEscenaDestino; 
    
    // Referencia para el sonido que le pondremos
    public AudioSource sonidoPortal; 

    // Esta función se activa sola cuando algo entra en el BoxCollider2D (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que quien chocó fue el jugador y no un enemigo o un NPC
        if (collision.CompareTag("Player")) 
        {
            // Cambiamos de escena
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}