using UnityEngine;
using System.Collections;

public class GestorHabilidades : MonoBehaviour
{
    [Header("Referencias del Jugador")]
    public Salud estadisticas;
    public Animator animatorJugador;

    [Header("Configuración de Habilidades")]
    public int costoMana = 2;
    public float enfriamientoPunos = 1f;
    public float enfriamientoEscudo = 5f;
    public float enfriamientoDragon = 10f;

    [Header("Puntos de Lanzamiento (Objetos Hijos)")]
    public Transform puntoAtaquePunos;
    public Transform puntoAtaqueDragon;

    [Header("Efectos Visuales (Prefabs)")]
    // Aquí arrastrarás tus archivos desde la carpeta Project
    public GameObject prefabPunos;
    public GameObject prefabEscudo;
    public GameObject prefabDragon;

    private float siguienteUsoPunos;
    private float siguienteUsoEscudo;
    private float siguienteUsoDragon;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (Input.GetKeyDown(KeyCode.Z)) IntentarHabilidad(UsarPunos, ref siguienteUsoPunos, enfriamientoPunos);
            if (Input.GetKeyDown(KeyCode.X)) IntentarHabilidad(UsarEscudoExplosivo, ref siguienteUsoEscudo, enfriamientoEscudo);
            if (Input.GetKeyDown(KeyCode.V)) IntentarHabilidad(UsarAtaqueDragon, ref siguienteUsoDragon, enfriamientoDragon);
        }
    }

    void IntentarHabilidad(System.Action accion, ref float siguienteUso, float cooldown)
    {
        if (Time.time >= siguienteUso && estadisticas.manaActual >= costoMana)
        {
            estadisticas.manaActual -= costoMana;
            estadisticas.ActualizarPantalla();
            accion.Invoke();
            siguienteUso = Time.time + cooldown;
        }
        else if (estadisticas.manaActual < costoMana)
        {
            Debug.Log("¡No tienes maná!");
        }
    }

    void UsarPunos()
    {
        if (animatorJugador != null) animatorJugador.SetTrigger("GolpePunos");
        // Usamos el prefabPunos y el puntoAtaquePunos
        InstanciarEfecto(prefabPunos, puntoAtaquePunos, 0.5f);
    }

    void UsarEscudoExplosivo()
    {
        if (animatorJugador != null) animatorJugador.SetTrigger("ActivarEscudo");
        // Usamos el prefabEscudo centrado en el jugador
        InstanciarEfecto(prefabEscudo, transform, 1.0f);
    }

    void UsarAtaqueDragon()
    {
        if (animatorJugador != null) animatorJugador.SetTrigger("AlientoDragon");
        // Usamos el prefabDragon y el puntoAtaqueDragon
        InstanciarEfecto(prefabDragon, puntoAtaqueDragon, 1.5f);
    }

    void InstanciarEfecto(GameObject prefab, Transform punto, float duracion)
    {
        if (prefab != null && punto != null)
        {
            // Creamos una copia del Prefab en la posición y rotación del punto
            GameObject nuevoEfecto = Instantiate(prefab, punto.position, punto.rotation);
            // Destruimos el objeto creado automáticamente tras los segundos indicados
            Destroy(nuevoEfecto, duracion);
        }
    }
}