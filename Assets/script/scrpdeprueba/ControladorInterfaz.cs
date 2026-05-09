using UnityEngine;

public class ControladorInterfaz : MonoBehaviour {
    public GameObject ventanaMapa; // Arrastra aquí tu 'VentanaMapa_Grande'

    // Esta función se activará al tocar el botón de abajo
    public void AbrirCerrarMapa() {
        // Si está abierto se cierra, si está cerrado se abre
        bool estadoActual = ventanaMapa.activeSelf;
        ventanaMapa.SetActive(!estadoActual);

        // Opcional: Pausar el juego cuando el mapa esté abierto
        if (!estadoActual) {
            Time.timeScale = 0f; // Pausa
        } else {
            Time.timeScale = 1f; // Reanuda
        }
    }
}