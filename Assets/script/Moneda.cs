using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int valor = 10;

    private void OnTriggerEnter2D(Collider2D other) {
        ProcesarContacto(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ProcesarContacto(collision.gameObject);
    }

    private void ProcesarContacto(GameObject other) {
        Debug.Log("Moneda tocada por: " + other.name + " (tag=" + other.tag + ")");

        if (other.CompareTag("Player") || other.name.Contains("Caballero")) {
            if (GameManager.instancia != null) {
                GameManager.instancia.SumarMonedas(valor);
                Debug.Log("Monedas sumadas correctamente.");
                Destroy(gameObject);
            } else {
                Debug.LogError("ERROR: No encontré el GameManager en la escena.");
            }
        } else {
            Debug.LogWarning("El objeto " + other.name + " no tiene el Tag 'Player'.");
        }
    }
}