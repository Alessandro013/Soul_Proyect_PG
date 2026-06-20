using UnityEngine;

public class Salud : MonoBehaviour
{
    [Header("Estadísticas del Jugador")]
    public int vidaMaxima = 10;
    public int vidaActual;
    public int escudoMaximo = 5;
    public int escudoActual;
    public int manaMaximo = 10;
    public int manaActual;

    [Header("Conexión con la Interfaz")]
    public ManejadorHUD manejadorHUD; // El puente hacia las barras de la pantalla

    protected virtual void Start()
    {
        // Al iniciar, llenamos todas las estadísticas al máximo
        vidaActual = vidaMaxima;
        escudoActual = escudoMaximo;
        manaActual = manaMaximo;

        // Actualizamos la pantalla por primera vez
        ActualizarPantalla();
    }

    public virtual void RecibirDanio(int danio)
    {
        vidaActual -= danio;
        Debug.Log(gameObject.name + " recibió " + danio + " de daño. Vida restante: " + vidaActual);

        // ¡Le avisamos al HUD que los números cambiaron!
        ActualizarPantalla();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Esta función empaqueta todos los números y se los manda a tu compañero (el script ManejadorHUD)
    private void ActualizarPantalla()
    {
        if (manejadorHUD != null)
        {
            manejadorHUD.ActualizarBarras(vidaActual, vidaMaxima, escudoActual, escudoMaximo, manaActual, manaMaximo);
        }
    }

    protected virtual void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        // En lugar de borrarlo y romper las físicas de los enemigos, lo "apagamos"
        gameObject.SetActive(false); 
        
        // Más adelante aquí pondremos la pantalla de Game Over
    }

    // Agrega esto justo debajo de tu función Start()
    void Update()
    {
        // Si presionamos la tecla H (Hurt / Herida), el jugador recibe 2 de daño
        if (Input.GetKeyDown(KeyCode.H))
        {
            RecibirDanio(2);
        }
    }
}