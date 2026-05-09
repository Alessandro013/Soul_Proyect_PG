using UnityEngine;
using System.Collections;

public class IAEnemigo : MonoBehaviour {
    [Header("Configuración")]
    public Transform[] puntosPatrulla;
    public float radioDeteccion = 5f;
    public float velocidad = 2f;
    public Transform jugador;

    [Header("Combate")]
    public float fuerzaEmpuje = 5f;
    public float tiempoRetrocesoDespuesAtaque = 0.5f;
    public float tiempoRecargaAtaque = 1.5f;

    private int indicePunto = 0;
    private bool persiguiendo = false;
    private bool atacando = false;
    private Rigidbody2D rb2D;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (atacando) {
            return;
        }

        if (jugador == null) {
            if (persiguiendo) {
                persiguiendo = false;
                indicePunto = ObtenerIndicePuntoMasCercano(new Vector2(transform.position.x, transform.position.y));
            }
            Patrullar();
            return;
        }

        Vector2 posicionJugador = new Vector2(jugador.position.x, jugador.position.y);
        Vector2 posicionActual = new Vector2(transform.position.x, transform.position.y);
        float distanciaAlJugador = Vector2.Distance(posicionActual, posicionJugador);

        if (distanciaAlJugador < radioDeteccion) {
            persiguiendo = true;
            Perseguir(posicionJugador);
        } else {
            if (persiguiendo) {
                persiguiendo = false;
                indicePunto = ObtenerIndicePuntoMasCercano(posicionActual);
            }
            Patrullar();
        }
    }

    private void Patrullar() {
        Transform puntoDestino = ObtenerPuntoPatrullaValido();
        if (puntoDestino == null) {
            return;
        }

        Vector2 destino2D = new Vector2(puntoDestino.position.x, puntoDestino.position.y);
        MoverHacia(destino2D, velocidad);

        Vector2 posicionActual = new Vector2(transform.position.x, transform.position.y);
        if (Vector2.Distance(posicionActual, destino2D) < 0.2f) {
            indicePunto = ObtenerIndiceSiguienteValido(indicePunto);
        }
    }

    private void Perseguir(Vector2 posicionJugador) {
        MoverHacia(posicionJugador, velocidad * 1.5f);
    }

    private void MoverHacia(Vector2 destino2D, float velocidadActual) {
        Vector2 posicionActual = new Vector2(transform.position.x, transform.position.y);
        Vector2 nuevaPosicion2D = Vector2.MoveTowards(posicionActual, destino2D, velocidadActual * Time.deltaTime);
        Vector3 nuevaPosicion = new Vector3(nuevaPosicion2D.x, nuevaPosicion2D.y, transform.position.z);

        if (rb2D != null) {
            rb2D.MovePosition(nuevaPosicion);
        } else {
            transform.position = nuevaPosicion;
        }
    }

    private Transform ObtenerPuntoPatrullaValido() {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0) {
            return null;
        }

        int inicio = indicePunto;
        for (int i = 0; i < puntosPatrulla.Length; i++) {
            int index = (indicePunto + i) % puntosPatrulla.Length;
            if (puntosPatrulla[index] != null) {
                indicePunto = index;
                return puntosPatrulla[index];
            }
        }

        return null;
    }

    private int ObtenerIndiceSiguienteValido(int indiceActual) {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0) {
            return 0;
        }

        int siguiente = (indiceActual + 1) % puntosPatrulla.Length;
        for (int i = 0; i < puntosPatrulla.Length; i++) {
            int index = (siguiente + i) % puntosPatrulla.Length;
            if (puntosPatrulla[index] != null) {
                return index;
            }
        }

        return indiceActual;
    }

    private int ObtenerIndicePuntoMasCercano(Vector2 posicionActual) {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0) {
            return 0;
        }

        float distanciaMinima = float.MaxValue;
        int indiceMasCercano = indicePunto;

        for (int i = 0; i < puntosPatrulla.Length; i++) {
            if (puntosPatrulla[i] == null) {
                continue;
            }

            Vector2 punto2D = new Vector2(puntosPatrulla[i].position.x, puntosPatrulla[i].position.y);
            float distancia = Vector2.Distance(posicionActual, punto2D);
            if (distancia < distanciaMinima) {
                distanciaMinima = distancia;
                indiceMasCercano = i;
            }
        }

        return indiceMasCercano;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && !atacando) {
            StartCoroutine(SecuenciaAtaque(collision.gameObject));
        }
    }

    private IEnumerator SecuenciaAtaque(GameObject player) {
        atacando = true;

        player.SendMessage("RecibirDaño", 1, SendMessageOptions.DontRequireReceiver);

        Rigidbody2D rbJugador = player.GetComponent<Rigidbody2D>();
        if (rbJugador != null) {
            Vector2 direccionEmpuje = (new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
            rbJugador.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
        }

        Vector2 posicionRetroceso = new Vector2(transform.position.x, transform.position.y);
        if (player != null) {
            Vector2 direccionJugador = (new Vector2(player.transform.position.x, player.transform.position.y) - posicionRetroceso).normalized;
            posicionRetroceso -= direccionJugador * 2f;
        }

        float tiempo = 0f;
        while (tiempo < tiempoRetrocesoDespuesAtaque) {
            MoverHacia(posicionRetroceso, velocidad);
            tiempo += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(tiempoRecargaAtaque);
        atacando = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        if (puntosPatrulla == null || puntosPatrulla.Length == 0) {
            return;
        }

        Gizmos.color = Color.cyan;
        Vector3? ultimoPunto = null;
        for (int i = 0; i < puntosPatrulla.Length; i++) {
            if (puntosPatrulla[i] == null) {
                continue;
            }

            Vector3 punto = puntosPatrulla[i].position;
            Gizmos.DrawSphere(punto, 0.15f);
            if (ultimoPunto.HasValue) {
                Gizmos.DrawLine(ultimoPunto.Value, punto);
            }
            ultimoPunto = punto;
        }

        if (ultimoPunto.HasValue && puntosPatrulla.Length > 1) {
            // Conectar el último con el primero válido para cerrar el bucle
            for (int i = 0; i < puntosPatrulla.Length; i++) {
                if (puntosPatrulla[i] != null) {
                    Gizmos.DrawLine(ultimoPunto.Value, puntosPatrulla[i].position);
                    break;
                }
            }
        }
    }
}