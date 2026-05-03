using UnityEngine;

public class JugadorStats : MonoBehaviour
{
    [Header("Valores iniciales")]
    public int vidaMaxima = 2;
    public int escudoMaximo = 5;
    public float manaMaximo = 200f;

    [Header("Estado actual")]
    public int vidaActual;
    public int escudoActual;
    public float manaActual;

    public ManejadorHUD hud;

    private bool estaMuerto;

    void Start()
    {
        vidaActual = vidaMaxima;
        escudoActual = escudoMaximo;
        manaActual = manaMaximo;
        estaMuerto = false;
        ActualizarHUD();
    }

    public bool ConsumirMana(float cantidad)
    {
        if (estaMuerto) return false;
        if (manaActual < cantidad) return false;

        manaActual = Mathf.Max(0f, manaActual - cantidad);
        ActualizarHUD();
        return true;
    }

    public void RecibirDanio(float danio)
    {
        if (estaMuerto) return;

        int danioEntero = Mathf.CeilToInt(danio);

        if (escudoActual > 0)
        {
            int dañoAlEscudo = Mathf.Min(escudoActual, danioEntero);
            escudoActual -= dañoAlEscudo;
            danioEntero -= dañoAlEscudo;
            Debug.Log($"Jugador recibió {dañoAlEscudo} de daño al escudo. Escudo restante: {escudoActual}");
        }

        if (danioEntero > 0)
        {
            vidaActual -= danioEntero;
            Debug.Log($"Jugador recibió {danioEntero} de daño de vida. Vida restante: {vidaActual}");
        }

        ActualizarHUD();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void CurarVida(int cantidad)
    {
        if (estaMuerto) return;
        vidaActual = Mathf.Clamp(vidaActual + cantidad, 0, vidaMaxima);
        ActualizarHUD();
    }

    public void RestaurarEscudo(int cantidad)
    {
        if (estaMuerto) return;
        escudoActual = Mathf.Clamp(escudoActual + cantidad, 0, escudoMaximo);
        ActualizarHUD();
    }

    public void RestaurarMana(float cantidad)
    {
        if (estaMuerto) return;
        manaActual = Mathf.Clamp(manaActual + cantidad, 0f, manaMaximo);
        ActualizarHUD();
    }

    private void Morir()
    {
        estaMuerto = true;
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes añadir animación de muerte o reiniciar escena si lo quieres.
    }

    public void ActualizarHUD()
    {
        if (hud != null)
        {
            hud.ActualizarBarras(vidaActual, vidaMaxima, escudoActual, escudoMaximo, Mathf.RoundToInt(manaActual), Mathf.RoundToInt(manaMaximo));
        }
    }
}
