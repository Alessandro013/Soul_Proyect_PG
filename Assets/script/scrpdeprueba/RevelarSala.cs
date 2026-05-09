using UnityEngine;
public class RevelarSala : MonoBehaviour {
    public GameObject cuadritoMinimapa; // El de la esquina
    public GameObject cuadritoMapaGrande; // El de la ventana central

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (cuadritoMinimapa != null) cuadritoMinimapa.SetActive(true);
            if (cuadritoMapaGrande != null) cuadritoMapaGrande.SetActive(true);
        }
    }
}