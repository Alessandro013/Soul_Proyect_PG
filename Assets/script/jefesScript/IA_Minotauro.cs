using UnityEngine;

public class IA_Minotauro : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 2.5f;
    public float radioDeteccion = 10f;
    public float radioAtaque = 2.5f;
    
    private Animator anim;
    private float cronometroIA;
    private int decision;
    private bool estaAtacando;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Si no asignas al jugador en el inspector, lo busca por Tag
        if (jugador == null) jugador = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (estaAtacando) return; // Si está atacando, no hace nada más

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia < radioDeteccion)
        {
            LogicaJefe(distancia);
        }
        else
        {
            // Fuera de rango: Quieto
            CambiarAnimacion(0);
        }
    }

    void LogicaJefe(float distancia)
    {
        // Mirar siempre al jugador (voltear el sprite)
        Vector3 escala = transform.localScale;
        if (jugador.position.x > transform.position.x) escala.x = Mathf.Abs(escala.x);
        else escala.x = -Mathf.Abs(escala.x);
        transform.localScale = escala;

        if (distancia > radioAtaque)
        {
            // Perseguir
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
            CambiarAnimacion(1);
        }
        else
        {
            // IA: Decidir qué ataque usar cuando está cerca
            cronometroIA += Time.deltaTime;
            if (cronometroIA >= 1.5f) // Decide cada 1.5 segundos
            {
                decision = Random.Range(2, 4); // Elige entre ataque 2 o 3 (nuestros IDs)
                CambiarAnimacion(decision);
                estaAtacando = true;
                cronometroIA = 0;
                // Hacer daño inmediato al jugador si está en rango de ataque
                HacerDañoAlJugador();
            }
            else
            {
                CambiarAnimacion(0); // Espera un poco entre ataques
            }
        }
    }

    void HacerDañoAlJugador()
    {
        // Buscar el script de stats del jugador
        JugadorStats jugadorStats = jugador.GetComponent<JugadorStats>();
        if (jugadorStats != null)
        {
            jugadorStats.RecibirDanio(3); // Daño de 3 como mencionaste
        }
    }

    void CambiarAnimacion(int id)
    {
        anim.SetInteger("estado", id);
    }

    // Llamar esto al final de las animaciones de ataque usando un Animation Event
    public void FinalizarAtaque()
    {
        estaAtacando = false;
        CambiarAnimacion(0);
    }
}
