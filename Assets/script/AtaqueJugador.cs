using UnityEngine;
using System.Collections.Generic;

public class AtaqueJugador : MonoBehaviour {
    [Header("Configuración de Combate")]
    public Transform puntoAtaque; 
    public float radioGolpe = 0.5f; 
    public LayerMask capaEnemigo;   
    public int danioPorGolpe = 10; 

    [Header("Arco")]
    public GameObject proyectilArcoPrefab;
    public Transform puntoDisparoArco;

    [Header("Ataque Especial (Tecla P)")]
    public GameObject efectoEspecialPrefab; // Tu efecto rojo "Creciente de Sangre"
    public int danioEspecial = 25;
    public float costoManaEspecial = 10f;

    private JugadorStats jugadorStats;

    void Start() {
        // Buscamos las estadísticas del jugador para el manejo de maná
        jugadorStats = GetComponent<JugadorStats>();
        if (jugadorStats == null) {
            Debug.LogWarning("AtaqueJugador: no se encontró JugadorStats en el mismo GameObject.");
        }
    }

    void Update() {
        // Detectar si presionas la P para lanzar el poder especial
        if (Input.GetKeyDown(KeyCode.P)) {
            EjecutarAtaqueEspecial();
        }
    }

    // --- LÓGICA DEL ATAQUE ESPECIAL ---
    public void EjecutarAtaqueEspecial() {
        // 1. Verificar si el jugador Jhos tiene suficiente maná
        if (jugadorStats != null && !jugadorStats.ConsumirMana(costoManaEspecial)) {
            Debug.Log("No hay suficiente maná para el Ataque Especial.");
            return;
        }

        // 2. Instanciar el proyectil rojo (Creciente de Sangre)
        if (efectoEspecialPrefab != null && puntoAtaque != null) {
            GameObject especial = Instantiate(efectoEspecialPrefab, puntoAtaque.position, transform.rotation);
            
            // Configuramos la dirección del proyectil basado en hacia dónde mira Jhos
            // BalaJugador scriptEspecial = especial.GetComponent<BalaJugador>();
            // if (scriptEspecial != null) {
            //     Vector2 direccion = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            //     scriptEspecial.SetDireccion(direccion);
            // }
        }

        // 3. Daño en área instantáneo cerca del jugador
        EfectuarGolpeEspecial();
    }

    private void EfectuarGolpeEspecial() {
        // Detecta enemigos en un círculo más grande para el ataque especial
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioGolpe * 1.5f, capaEnemigo);
        
        foreach (Collider2D enemigo in enemigosGolpeados) {
            // Daño a enemigos con el script 'Salud' (comunes)
            Salud saludEnemigo = enemigo.GetComponentInParent<Salud>();
            if (saludEnemigo != null) {
                saludEnemigo.RecibirDanio(danioEspecial);
            }

            // Daño al jefe Minotauro con el script 'SaludMinotauro'
            SaludMinotauro saludJefe = enemigo.GetComponentInParent<SaludMinotauro>();
            if (saludJefe != null) {
                saludJefe.RecibirDaño(danioEspecial);
            }
        }
    }

    // --- ATAQUE BÁSICO DE ESPADA ---
    public void EfectuarGolpe() {
        if (jugadorStats != null && !jugadorStats.ConsumirMana(2f)) return;

        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioGolpe, capaEnemigo);
        HashSet<Object> enemigosDañados = new HashSet<Object>();

        foreach (Collider2D enemigo in enemigosGolpeados) {
            // Intentar dañar enemigo normal
            Salud saludNormal = enemigo.GetComponentInParent<Salud>();
            if (saludNormal != null && enemigosDañados.Add(saludNormal)) {
                saludNormal.RecibirDanio(danioPorGolpe);
            }

            // Intentar dañar al Jefe Minotauro
            SaludMinotauro saludJefe = enemigo.GetComponentInParent<SaludMinotauro>();
            if (saludJefe != null && enemigosDañados.Add(saludJefe)) {
                saludJefe.RecibirDaño(danioPorGolpe);
            }
        }
    }

    // --- DISPARO DE ARCO ---
    public void DispararArco() {
        if (jugadorStats != null && !jugadorStats.ConsumirMana(2)) return;

        if (proyectilArcoPrefab != null && puntoDisparoArco != null) {
            Vector2 direccion = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            GameObject bala = Instantiate(proyectilArcoPrefab, puntoDisparoArco.position, Quaternion.identity);
            
            // BalaJugador balaScript = bala.GetComponent<BalaJugador>();
            // if (balaScript != null) {
            //     balaScript.SetDireccion(direccion);
            // }
        }
    }

    // Visualización del rango de ataque en el editor de Unity
    private void OnDrawGizmosSelected() {
        if (puntoAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoAtaque.position, radioGolpe);
    }
}