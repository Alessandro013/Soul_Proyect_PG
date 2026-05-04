using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Configuración de Destino")]
    public string nombreEscenaDestino = "mapa_hielo"; // Aquí escribirás "mapa_hielo", etc.

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo teletransportamos si es el jugador
        if (otro.CompareTag("Player"))
        {
            Debug.Log("Teletransportando a: " + nombreEscenaDestino);
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}