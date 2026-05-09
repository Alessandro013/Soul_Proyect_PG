using UnityEngine;
using System.Collections.Generic;

public class InteligenciaEnemigo : MonoBehaviour
{
    public enum Estado { Patrulla, Esperando, Perseguire, Atacar, Huir, Muerte }
    
    [Header("Configuración de Estados")]
    public Estado estadoActual = Estado.Patrulla;
    public float vidaActual = 100f;
    public float vidaMinimaParaHuir = 20f;

    [Header("Configuración de Patrulla")]
    public List<Transform> puntosRuta; 
    public float tiempoDeEspera = 2.0f; // Segundos que se quedará parado
    private int indiceActual = 0;
    private float cronometroEspera = 0f;

    [Header("Rangos")]
    public float rangoDeteccion = 5f;
    public float rangoAtaque = 1.5f;

    [Header("Movimiento")]
    public Transform jugador;
    public float velocidadMax = 3f;
    public float fuerzaGiro = 5f;
    
    private Vector2 velocidadActual;
    private bool estaMuerto = false;
    private Animator anim;
    private Vector3 escalaOriginal;

    void Start()
    {
        anim = GetComponent<Animator>();
        escalaOriginal = transform.localScale;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (estaMuerto) return;

        DeterminarEstado();
        EjecutarEstado();
        ActualizarAnimaciones();
    }

    void DeterminarEstado()
    {
        if (vidaActual <= 0) { estadoActual = Estado.Muerte; return; }
        
        float distancia = Vector2.Distance(transform.position, jugador.position);

        // La persecución y huida siempre tienen prioridad sobre la patrulla/espera
        if (vidaActual <= vidaMinimaParaHuir) estadoActual = Estado.Huir;
        else if (distancia <= rangoAtaque) estadoActual = Estado.Atacar;
        else if (distancia <= rangoDeteccion) estadoActual = Estado.Perseguire;
        else if (estadoActual != Estado.Esperando) // Si no hay nadie cerca, patrulla
            estadoActual = Estado.Patrulla;
    }

    void EjecutarEstado()
    {
        switch (estadoActual)
        {
            case Estado.Patrulla:
                Patrullar();
                break;
            case Estado.Esperando:
                QuedarseEsperando();
                break;
            case Estado.Perseguire:
                AplicarSteering(jugador.position);
                break;
            case Estado.Atacar:
                velocidadActual = Vector2.Lerp(velocidadActual, Vector2.zero, Time.deltaTime * 5f);
                anim.SetTrigger("attack"); 
                break;
            case Estado.Huir:
                Vector2 dir = (Vector2)transform.position - (Vector2)jugador.position;
                AplicarSteering((Vector2)transform.position + dir);
                break;
            case Estado.Muerte:
                Morir();
                break;
        }
    }

    void Patrullar()
    {
        if (puntosRuta.Count == 0) return;
        Transform destino = puntosRuta[indiceActual];
        
        if (Vector2.Distance(transform.position, destino.position) < 0.3f)
        {
            // Al llegar, entramos en modo espera
            estadoActual = Estado.Esperando;
            cronometroEspera = tiempoDeEspera;
            velocidadActual = Vector2.zero; // Frenamos en seco
        }
        else
        {
            AplicarSteering(destino.position);
        }
    }

    void QuedarseEsperando()
    {
        // El enemigo se queda quieto y el tiempo corre
        cronometroEspera -= Time.deltaTime;

        if (cronometroEspera <= 0)
        {
            // Cuando el tiempo se acaba, calculamos el siguiente punto y volvemos a patrullar
            indiceActual = (indiceActual + 1) % puntosRuta.Count;
            estadoActual = Estado.Patrulla;
        }
    }

    void AplicarSteering(Vector2 objetivo)
    {
        Vector2 deseada = (objetivo - (Vector2)transform.position).normalized * velocidadMax;
        Vector2 steer = deseada - velocidadActual;
        velocidadActual += steer * fuerzaGiro * Time.deltaTime;
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, velocidadMax);

        transform.position += (Vector3)velocidadActual * Time.deltaTime;

        if (velocidadActual.x > 0.1f) 
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        else if (velocidadActual.x < -0.1f) 
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
    }

    void ActualizarAnimaciones()
    {
        if (anim != null)
        {
            // Si la magnitud es 0 (cuando espera), isWalking pasará a false solo
            anim.SetBool("isWalking", velocidadActual.magnitude > 0.1f);
        }
    }

    void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;
        velocidadActual = Vector2.zero;
        anim.SetTrigger("die"); 
        Destroy(gameObject, 2f); 
    }

    // GIZMOS: Para ver los rangos y la ruta verde en la escena
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

        if (puntosRuta != null && puntosRuta.Count > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < puntosRuta.Count; i++)
            {
                if (puntosRuta[i] != null)
                {
                    Vector3 actual = puntosRuta[i].position;
                    Vector3 siguiente = puntosRuta[(i + 1) % puntosRuta.Count].position;
                    Gizmos.DrawLine(actual, siguiente);
                }
            }
        }
    }
}