using UnityEngine;

public class GestorInterfaz : MonoBehaviour 
{
    [Header("Referencias de Paneles")]
    public GameObject panelMapa;
    public GameObject panelMochila;
    public GameObject panelEstadisticas;

    [Header("Controles Configurables")]
    public KeyCode teclaMapa;
    public KeyCode teclaMochila;
    public KeyCode teclaEstadisticas;

    void Start()
    {
        // Cargamos las teclas guardadas. Si no existen, asignamos el valor por defecto en formato texto.
        string mapaGuardado = PlayerPrefs.GetString("Control_Mapa", "M");
        string mochilaGuardada = PlayerPrefs.GetString("Control_Mochila", "Escape");
        string estadisticasGuardada = PlayerPrefs.GetString("Control_Estadisticas", "I");

        // Convertimos ese texto de vuelta a un comando ejecutable de tipo KeyCode
        teclaMapa = (KeyCode)System.Enum.Parse(typeof(KeyCode), mapaGuardado);
        teclaMochila = (KeyCode)System.Enum.Parse(typeof(KeyCode), mochilaGuardada);
        teclaEstadisticas = (KeyCode)System.Enum.Parse(typeof(KeyCode), estadisticasGuardada);
    }

    void Update() 
    {
        // Ahora evaluamos las variables dinámicas en lugar de valores fijos
        if (Input.GetKeyDown(teclaMapa)) 
            GestionarPanel(panelMapa);
            
        if (Input.GetKeyDown(teclaMochila)) 
            GestionarPanel(panelMochila);
            
        if (Input.GetKeyDown(teclaEstadisticas)) 
            GestionarPanel(panelEstadisticas);
    }

    public void GestionarPanel(GameObject panelObjetivo) 
    {
        if (panelObjetivo == null) return;

        if (panelObjetivo.activeSelf) 
        {
            panelObjetivo.SetActive(false);
        }
        else 
        {
            panelMapa.SetActive(false);
            panelMochila.SetActive(false);
            panelEstadisticas.SetActive(false);

            panelObjetivo.SetActive(true);
        }

        if (panelMapa.activeSelf || panelMochila.activeSelf || panelEstadisticas.activeSelf) 
        {
            Time.timeScale = 0f; 
        } 
        else 
        {
            Time.timeScale = 1f; 
        }
    }
}