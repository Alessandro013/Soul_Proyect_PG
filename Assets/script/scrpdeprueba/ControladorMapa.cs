using UnityEngine;

public class ControladorMapa : MonoBehaviour {
    [Header("Referencias de UI")]
    public GameObject mapaGrande; // Arrastra el Panel grande aquí
    public GameObject minimapaPequeño; // Arrastra el contenedor de la esquina

    private bool mapaAbierto = false;

    void Update() {
        // Si presionas la tecla M, se abre o se cierra
        if (Input.GetKeyDown(KeyCode.M)) {
            AlternarMapa();
        }
    }

    // Esta función la usaremos tanto para la tecla como para el clic
    public void AlternarMapa() {
        mapaAbierto = !mapaAbierto;
        
        mapaGrande.SetActive(mapaAbierto);

        // Opcional: Pausar el juego cuando el mapa esté abierto
        if (mapaAbierto) {
            Time.timeScale = 0f; // Pausa el tiempo
        } else {
            Time.timeScale = 1f; // Reanuda el tiempo
        }
    }
}