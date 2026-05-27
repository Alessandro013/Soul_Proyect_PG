using UnityEngine;

public class CaballeroAtaque : MonoBehaviour
{
    [Header("Ajustes de Ataque")]
    public Transform controladorAtaque; 
    public float radioAtaque = 0.5f;
    public float puntosDano = 2f;
    public LayerMask capaEnemigo; 

    [Header("Controles")]
    public KeyCode teclaAtaque = KeyCode.Space; // Puedes cambiarla desde el Inspector

    private Animator anim;

    void Start()
    {
        // Obtenemos el animador para activar la animación de ataque
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Si presionas la tecla asignada (Espacio por defecto)
        if (Input.GetKeyDown(teclaAtaque))
        {
            EjecutarAtaque();
        }
    }

    void EjecutarAtaque()
    {
        // 1. Activa la animación (asegúrate de que el parámetro en tu Animator se llame "Atacar")
        if (anim != null) {
            anim.SetTrigger("attack");
        }

        // 2. Detecta y hace daño
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaEnemigo);

        foreach (Collider2D colision in enemigosGolpeados)
        {
            EnemigoSalud salud = colision.GetComponent<EnemigoSalud>();
            if (salud != null)
            {
                salud.TomarDano(puntosDano);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
        }
    }
}