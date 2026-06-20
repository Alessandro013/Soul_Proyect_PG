using UnityEngine;
using UnityEngine.SceneManagement;

public class Salud : MonoBehaviour
{
    [Header("Estadísticas")]
    public int vidaMaxima = 10;
    public int vidaActual;
    public int escudoMaximo = 5;
    public int escudoActual;
    public int manaMaximo = 10; // Agregado para consistencia
    public int manaActual;      // Ahora es public para que GestorHabilidades pueda verlo

    [Header("Regeneración")]
    public float tiempoParaRegenerar = 3f;
    private float tiempoUltimoDanio = 0f;

    public ManejadorHUD manejadorHUD;

    protected virtual void Start()
    {
        vidaActual = vidaMaxima;
        escudoActual = escudoMaximo;
        manaActual = manaMaximo;
        ActualizarPantalla();
    }

    void Update()
    {
        // Lógica de regeneración del escudo
        if (escudoActual < escudoMaximo && Time.time >= tiempoUltimoDanio + tiempoParaRegenerar)
        {
            escudoActual++;
            ActualizarPantalla();
            tiempoUltimoDanio = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.H)) RecibirDanio(2);
    }

    public virtual void RecibirDanio(int danio)
    {
        tiempoUltimoDanio = Time.time;

        if (escudoActual > 0)
        {
            escudoActual -= danio;
            if (escudoActual < 0)
            {
                vidaActual += escudoActual;
                escudoActual = 0;
            }
        }
        else
        {
            vidaActual -= danio;
        }

        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarPantalla();

        if (vidaActual <= 0) Morir();
    }

    // Cambiado a public para que GestorHabilidades pueda llamarlo
    public void ActualizarPantalla()
    {
        if (manejadorHUD != null)
            manejadorHUD.ActualizarBarras(vidaActual, vidaMaxima, escudoActual, escudoMaximo, manaActual, manaMaximo);
    }

    protected virtual void Morir()
    {
        SceneManager.LoadScene("GameOver");
    }
}