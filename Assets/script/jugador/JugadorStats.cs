using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar si mueres

public class JugadorStats : MonoBehaviour
{
    [Header("Configuración Máxima")]
    public int vidaMaxima = 3;
    public int escudoMaximo = 5;
    public float manaMaximo = 100f;

    [Header("Estado Actual")]
    public int vidaActual;
    public int escudoActual;
    public float manaActual;

    public ManejadorHUD hud;
    private bool estaMuerto = false;

    void Start() {
        vidaActual = vidaMaxima;
        escudoActual = escudoMaximo;
        manaActual = manaMaximo;
        ActualizarHUD();
    }

    public bool ConsumirMana(float cantidad) {
        if (manaActual >= cantidad) {
            manaActual -= cantidad;
            ActualizarHUD();
            return true;
        }
        return false;
    }

    // Método para recibir daño (1 punto siempre, priorizando escudo)
    public void RecibirDanio() {
        if (estaMuerto) return;

        if (escudoActual > 0) {
            escudoActual--;
        } else {
            vidaActual--;
        }

        ActualizarHUD();

        if (vidaActual <= 0) {
            Morir();
        }
    }

    private void Morir() {
        estaMuerto = true;
        Debug.Log("GAME OVER");
        // Opción A: Reiniciar escena
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Opción B: Destruir objeto
        Destroy(gameObject);
    }

    // DETECTAR COLISIÓN CON ENEMIGO O PROYECTIL
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("ProyectilEnemigo")) {
            RecibirDanio();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ProyectilEnemigo")) {
            RecibirDanio();
            Destroy(other.gameObject); // Destruye el proyectil al impactar
        }
    }

    public void ActualizarHUD() {
        if (hud != null) {
            hud.ActualizarBarras(vidaActual, vidaMaxima, escudoActual, escudoMaximo, (int)manaActual, (int)manaMaximo);
        }
    }
}