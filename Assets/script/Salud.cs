using UnityEngine;

public class Salud : MonoBehaviour
{
    public int vidaMaxima = 10;
    public int vidaActual;

    protected virtual void Start()
    {
        vidaActual = vidaMaxima;
    }

    public virtual void RecibirDanio(int danio)
    {
        vidaActual -= danio;
        Debug.Log(gameObject.name + " recibió " + danio + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}