using UnityEngine;
using System.Collections.Generic;

public class IA_JefeMinotauro : MonoBehaviour
{
    // Parámetros del Animator (int estado)
    // 0: statico | 1: walk | 2: attack | 3: muerte
    public enum Estado { Patrulla, Esperando, Perseguire, Atacar, Muerte }
    
    [Header("Configuración del Jefe")]
    public Estado estadoActual = Estado.Patrulla;
    public float vidaActual = 500f;

    [Header("Patrulla y Vigilancia")]
    public List<Transform> puntosRuta; 
    public float tiempoVigilancia = 3.0f; 
    private int indiceActual = 0;
    private float cronometroEspera = 0f;

    [Header("Rangos y Movimiento")]
    public Transform jugador;
    public float rangoDeteccion = 8f;
    public float rangoAtaque = 2.5f;
    public float velocidadMax = 2.5f;
    public float inerciaGiro = 3f;
    
    [Header("Ataque Especial (Proyectiles)")]
    public GameObject proyectilPrefab; 
    public Transform puntoDisparo; // Arrastra aquí un objeto vacío centrado en el Minotauro
    public float velocidadProyectil = 5f;
    public float tiempoParaDisparar = 4.0f; 
    private float cronometroProyectiles = 0f;

    private Vector2 velocidadActual;
    private bool estaMuerto = false;
    private Animator anim;
    private Vector3 escalaOriginal;

    void Start()
    {
        anim = GetComponent<Animator>();
        escalaOriginal = transform.localScale; // Guardamos tu tamaño de Unity

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
        EjecutarComportamiento();
        ActualizarAnimaciones();
    }

    void DeterminarEstado()
    {
        if (vidaActual <= 0) { estadoActual = Estado.Muerte; return; }
        
        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= rangoAtaque) 
            estadoActual = Estado.Atacar;
        else if (distancia <= rangoDeteccion) 
            estadoActual = Estado.Perseguire;
        else if (estadoActual != Estado.Esperando) 
            estadoActual = Estado.Patrulla;
    }

    void EjecutarComportamiento()
    {
        switch (estadoActual)
        {
            case Estado.Patrulla:
                Patrullar();
                cronometroProyectiles = 0f; // Reset si te pierde de vista
                break;
            case Estado.Esperando:
                Vigilar();
                break;
            case Estado.Perseguire:
                MoverJefe(jugador.position);
                ControlarAtaqueEspecial(); // Dispara tras 4 segundos de persecución
                break;
            case Estado.Atacar:
                FrenadoSuave();
                cronometroProyectiles = 0f;
                break;
            case Estado.Muerte:
                MorirJefe();
                break;
        }
    }

    void ControlarAtaqueEspecial()
    {
        cronometroProyectiles += Time.deltaTime;

        if (cronometroProyectiles >= tiempoParaDisparar)
        {
            DispararPatronEstrella();
            cronometroProyectiles = 0f; 
        }
    }

    void DispararPatronEstrella()
    {
        if (proyectilPrefab == null) return;

        // Direcciones exactas de tu dibujo: Arriba, Abajo, Izq, Der y Diagonales
        Vector2[] direcciones = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
        };

        // El lugar donde nacen los proyectiles (usa el transform del jefe si no asignas puntoDisparo)
        Vector3 spawnPos = (puntoDisparo != null) ? puntoDisparo.position : transform.position;

        foreach (Vector2 dir in direcciones)
        {
            GameObject bullet = Instantiate(proyectilPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rbB = bullet.GetComponent<Rigidbody2D>();
            if (rbB != null)
            {
                rbB.linearVelocity = dir * velocidadProyectil;
            }
            
            // Orientar el proyectil según su dirección
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void MoverJefe(Vector2 objetivo)
    {
        Vector2 deseada = (objetivo - (Vector2)transform.position).normalized * velocidadMax;
        Vector2 steer = deseada - velocidadActual;
        velocidadActual += steer * inerciaGiro * Time.deltaTime;
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, velocidadMax);

        transform.position += (Vector3)velocidadActual * Time.deltaTime;

        // Volteo sin cambiar el tamaño
        if (velocidadActual.x > 0.1f) 
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        else if (velocidadActual.x < -0.1f) 
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
    }

    void Patrullar()
    {
        if (puntosRuta.Count == 0) return;
        Transform destino = puntosRuta[indiceActual];
        if (Vector2.Distance(transform.position, destino.position) < 0.5f)
        {
            estadoActual = Estado.Esperando;
            cronometroEspera = tiempoVigilancia;
            velocidadActual = Vector2.zero;
        }
        else { MoverJefe(destino.position); }
    }

    void Vigilar()
    {
        cronometroEspera -= Time.deltaTime;
        if (cronometroEspera <= 0)
        {
            indiceActual = (indiceActual + 1) % puntosRuta.Count;
            estadoActual = Estado.Patrulla;
        }
    }

    void ActualizarAnimaciones()
    {
        if (anim == null) return;

        int valorEstado = 0; 
        if (estaMuerto) valorEstado = 3;
        else if (estadoActual == Estado.Atacar) valorEstado = 2;
        else if (velocidadActual.magnitude > 0.2f) valorEstado = 1; // Si se mueve -> walk
        else valorEstado = 0; // Si no -> statico

        anim.SetInteger("estado", valorEstado); // Sincronizado con tu "int estado"
    }

    void FrenadoSuave() { velocidadActual = Vector2.Lerp(velocidadActual, Vector2.zero, Time.deltaTime * 4f); }

    void MorirJefe()
    {
        if (estaMuerto) return;
        estaMuerto = true;
        velocidadActual = Vector2.zero;
        Destroy(gameObject, 3f); 
    }

    // --- ESTO DIBUJA LOS RADIOS VISUALMENTE EN EL EDITOR ---
    private void OnDrawGizmos()
    {
        // Radio de detección (Azul)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        // Radio de ataque (Rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}