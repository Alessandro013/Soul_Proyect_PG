using UnityEngine;

public class SaludMinotauro : MonoBehaviour
{
    public float vidaMax = 20f; // La vida que pediste
    private float vidaActual;
    private Animator anim;
    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMax;
        anim = GetComponent<Animator>();
    }

    public void RecibirDaño(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        Debug.Log("Minotauro herido. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;
        // El número 4 es el que definimos para la muerte en el Animator
        anim.SetInteger("estado", 4); 
        
        // Desactivamos el script de IA para que deje de perseguirte
        if(GetComponent<IA_Minotauro>() != null) 
            GetComponent<IA_Minotauro>().enabled = false;

        Debug.Log("¡El Minotauro ha caído!");
        Destroy(gameObject, 2f); // Se destruye después de la animación de muerte
    }
}
