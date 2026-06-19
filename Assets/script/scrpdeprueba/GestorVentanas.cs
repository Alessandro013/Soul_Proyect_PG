using UnityEngine;

public class GestorInterfaz : MonoBehaviour 
{
    [Header("Referencias de Paneles")]
    public GameObject panelMapa;
    public GameObject panelMochila;
    public GameObject panelEstadisticas;

    void Update() 
    {
        // Al presionar la tecla, alternamos el panel correspondiente
        if (Input.GetKeyDown(KeyCode.M)) 
            GestionarPanel(panelMapa);
            
        if (Input.GetKeyDown(KeyCode.Escape)) 
            GestionarPanel(panelMochila);
            
        if (Input.GetKeyDown(KeyCode.I)) 
            GestionarPanel(panelEstadisticas);
    }

    public void GestionarPanel(GameObject panelObjetivo) 
    {
        if (panelObjetivo == null) return;

        // Si el panel objetivo ya está abierto, lo cerramos
        if (panelObjetivo.activeSelf) 
        {
            panelObjetivo.SetActive(false);
        }
        else 
        {
            // Primero cerramos todos para evitar que se solapen
            panelMapa.SetActive(false);
            panelMochila.SetActive(false);
            panelEstadisticas.SetActive(false);

            // Luego abrimos solo el que queremos
            panelObjetivo.SetActive(true);
        }

        // Controlar el tiempo del juego:
        // Si CUALQUIERA está activo, pausamos. Si TODOS están desactivados, reanudamos.
        if (panelMapa.activeSelf || panelMochila.activeSelf || panelEstadisticas.activeSelf) 
        {
            Time.timeScale = 0f; // Pausa
        } 
        else 
        {
            Time.timeScale = 1f; // Reanuda
        }
    }
}