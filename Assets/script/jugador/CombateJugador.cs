using UnityEngine;

public class CombateJugador : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public Transform controladorAtaque;
    public float radioAtaque = 1.5f;
    public LayerMask capaEnemigo;
    public GameObject flechaPrefab;

    [Header("Ajustes de Tiempo")]
    public float tiempoEntreAtaques = 0.5f;
    private float tiempoSiguienteAtaque = 0f;

    [Header("Costes de Maná")]
    public float costeEspada = 1f;
    public float costeArco = 5f;

    private Animator anim;
    private JugadorStats stats;

    void Start()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<JugadorStats>();
    }

    void Update()
    {
        if (DialogueManager.estaHablando) return;

        // Solo permite atacar si ha pasado el tiempo suficiente (Cooldown)
        if (Time.time >= tiempoSiguienteAtaque)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EjecutarAtaque("attack", costeEspada, false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EjecutarAtaque("attack2", costeEspada, false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EjecutarAtaque("attackArco", costeArco, true);
            }
        }
    }

    void EjecutarAtaque(string trigger, float coste, bool esArco)
    {
        if (stats.ConsumirMana(coste))
        {
            anim.SetTrigger(trigger);
            tiempoSiguienteAtaque = Time.time + tiempoEntreAtaques;

            // Si es espada, el daño es instantáneo
            if (!esArco)
            {
                GolpeCuerpoACuerpo();
            }
            // NOTA: Si es arco, NO llamamos a DispararArco aquí para que 
            // puedas usar el Evento de Animación en Unity.
        }
    }

    void GolpeCuerpoACuerpo()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaEnemigo);
        foreach (Collider2D enemigo in enemigos)
        {
            enemigo.SendMessage("RecibirDanio", 1, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Golpeaste a: " + enemigo.name);
        }
    }

    // Esta función la llamarás desde el "Animation Event" en tu clip de attackArco
    public void DispararArco()
    {
        if (flechaPrefab != null)
        {
            // Instanciamos la flecha
            GameObject flecha = Instantiate(flechaPrefab, controladorAtaque.position, controladorAtaque.rotation);

            // CORRECCIÓN DE ORIENTACIÓN: 
            // Si el caballero mira a la izquierda (escala negativa), giramos la flecha
            if (transform.localScale.x < 0)
            {
                flecha.transform.rotation = Quaternion.Euler(0, 0, 180f);
            }
        }
    }

private void OnDrawGizmosSelected() {
    // Si asignaste el objeto vacío en el Inspector...
    if (controladorAtaque != null) {
        Gizmos.color = Color.red;
        // Ahora dibujamos la esfera en la posición del OBJETO VACÍO, no del caballero
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }
}
}