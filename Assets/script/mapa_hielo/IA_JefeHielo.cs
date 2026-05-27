using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IA_JefeHielo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform[] puntosPatrulla;
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;
    public float distanciaDeteccion = 7f;
    public float distanciaAtaque = 3f;

    [Header("Ataque de Doble Anillo")]
    public GameObject proyectilPrefab;
    public Transform controladorDisparo; 
    public float fuerzaDisparo = 12f;
    public float tiempoEntreAtaques = 3f;
    public float velocidadRotacionAnillo = 180f; // Qué tan rápido giran antes de salir

    [Header("Ajustes de los Anillos")]
    public int balasAnilloInterno = 8;
    public float radioInterno = 1.5f;
    public int balasAnilloExterno = 12;
    public float radioExterno = 3.5f;

    [Header("Referencias")]
    public Transform jugador;
    private int puntoActual = 0;
    private Animator anim;
    private float cronometroAtaque;
    private Vector3 escalaInspector; // Guardará el tamaño que le diste en Unity

    void Start()
    {
        anim = GetComponent<Animator>();
        
        // Buscamos al jugador automáticamente si no está asignado
        if (jugador == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if(playerObj != null) jugador = playerObj.transform;
        }

        // GUARDAMOS TU ESCALA PERSONALIZADA (la que sale en tu foto del Inspector)
        escalaInspector = transform.localScale;
    }

    void Update()
    {
        cronometroAtaque -= Time.deltaTime;
        if (jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaAtaque)
        {
            EstadoAtacar();
        }
        else if (distanciaAlJugador <= distanciaDeteccion)
        {
            EstadoPerseguir();
        }
        else
        {
            EstadoPatrullar();
        }
    }

    void EstadoPatrullar()
    {
        if (puntosPatrulla.Length == 0) return;

        anim.SetBool("isWalking", true);
        Transform punto = puntosPatrulla[puntoActual];
        transform.position = Vector2.MoveTowards(transform.position, punto.position, velocidadPatrulla * Time.deltaTime);

        if (Vector2.Distance(transform.position, punto.position) < 0.2f)
        {
            puntoActual = (puntoActual + 1) % puntosPatrulla.Length;
        }

        GirarHacia(punto.position);
    }

    void EstadoPerseguir()
    {
        anim.SetBool("isWalking", true);
        transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidadPersecucion * Time.deltaTime);
        GirarHacia(jugador.position);
    }

    void EstadoAtacar()
    {
        anim.SetBool("isWalking", false);
        GirarHacia(jugador.position);

        if (cronometroAtaque <= 0)
        {
            anim.SetTrigger("attack");
            cronometroAtaque = tiempoEntreAtaques;
        }
    }

    // --- FUNCIÓN DEL DOBLE ANILLO MÁGICO ---
    // Asegúrate de poner este nombre exacto en el Animation Event
    public void AtaqueDobleAnillo()
    {
        StartCoroutine(SecuenciaDobleAnillo());
    }

    IEnumerator SecuenciaDobleAnillo()
    {
        // Creamos un contenedor temporal que gire
        GameObject contenedor = new GameObject("ContenedorAnillos");
        contenedor.transform.position = controladorDisparo.position;
        contenedor.transform.SetParent(this.transform); // Sigue al jefe mientras carga

        List<GameObject> proyectiles = new List<GameObject>();

        // 1. Invocamos Anillo Interno
        InstanciarAnillo(balasAnilloInterno, radioInterno, contenedor.transform, proyectiles);
        
        // 2. Invocamos Anillo Externo (el más grande)
        InstanciarAnillo(balasAnilloExterno, radioExterno, contenedor.transform, proyectiles);

        // 3. Fase de Giro (El "Anillo" da vueltas alrededor del jefe)
        float tiempoGiro = 0;
        while (tiempoGiro < 1.2f) // Gira por 1.2 segundos
        {
            contenedor.transform.Rotate(Vector3.forward * velocidadRotacionAnillo * Time.deltaTime);
            tiempoGiro += Time.deltaTime;
            yield return null; 
        }

        // 4. ¡BUM! Disparo hacia afuera
        contenedor.transform.SetParent(null); // Ya no sigue al jefe al disparar
        foreach (GameObject proyectil in proyectiles)
        {
            if (proyectil != null)
            {
                proyectil.transform.SetParent(null);
                Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = true;
                    // Dirección desde el centro del jefe hacia la posición actual del proyectil rotado
                    Vector2 direccion = (proyectil.transform.position - controladorDisparo.position).normalized;
                    rb.linearVelocity = direccion * fuerzaDisparo;

                    float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                    proyectil.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
        
        Destroy(contenedor, 2f); // Borramos el objeto vacío
    }

    void InstanciarAnillo(int cantidad, float radio, Transform padre, List<GameObject> lista)
    {
        for (int i = 0; i < cantidad; i++)
        {
            float angulo = i * Mathf.PI * 2 / cantidad;
            Vector3 pos = new Vector3(Mathf.Cos(angulo), Mathf.Sin(angulo), 0) * radio;
            
            GameObject p = Instantiate(proyectilPrefab, padre.position + pos, Quaternion.identity);
            p.transform.SetParent(padre);
            
            Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false; 
            
            lista.Add(p);
        }
    }

    void GirarHacia(Vector3 objetivo)
    {
        // Respetamos la escala que guardamos en Start
        if (objetivo.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(escalaInspector.x), escalaInspector.y, escalaInspector.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(escalaInspector.x), escalaInspector.y, escalaInspector.z);
    }
}