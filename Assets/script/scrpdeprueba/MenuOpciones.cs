using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuOpciones : MonoBehaviour
{
    [Header("Navegación Teclado")]
    public GameObject primerBotonOpciones; 

    [Header("UI de Controles (Textos)")]
    public TextMeshProUGUI textoBotonMapa; 
    public TextMeshProUGUI textoBotonMochila;
    public TextMeshProUGUI textoBotonEstadisticas;

    [Header("UI de Audio")]
    public Slider sliderVolumen;
    public Toggle toggleMute;

    private string accionAEditar = "";
    private bool esperandoTecla = false;
    
    // El candado de seguridad
    private bool inicializando = false; 

    void OnEnable()
    {
        if (EventSystem.current != null && primerBotonOpciones != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(primerBotonOpciones);
        }
    }

    void Start()
    {
        // Cerramos el candado: estamos configurando el sistema
        inicializando = true; 

        ActualizarInterfazTexto();
        CargarConfiguracionAudio();

        // Abrimos el candado: el menú ya está listo para escuchar al jugador
        inicializando = false; 
    }

    // ==========================================
    // LÓGICA DE CONTROLES
    // ==========================================
    void ActualizarInterfazTexto()
    {
        if (textoBotonMapa != null) 
            textoBotonMapa.text = "Mapa: [ " + PlayerPrefs.GetString("Control_Mapa", "M") + " ]";
            
        if (textoBotonMochila != null) 
            textoBotonMochila.text = "Mochila: [ " + PlayerPrefs.GetString("Control_Mochila", "Escape") + " ]";
            
        if (textoBotonEstadisticas != null) 
            textoBotonEstadisticas.text = "Estadísticas: [ " + PlayerPrefs.GetString("Control_Estadisticas", "I") + " ]";
    }

    public void CambiarTeclaMapa() { ComenzarEspera("Control_Mapa", textoBotonMapa); }
    public void CambiarTeclaMochila() { ComenzarEspera("Control_Mochila", textoBotonMochila); }
    public void CambiarTeclaEstadisticas() { ComenzarEspera("Control_Estadisticas", textoBotonEstadisticas); }

    void ComenzarEspera(string nombreClave, TextMeshProUGUI textoBoton)
    {
        esperandoTecla = true;
        accionAEditar = nombreClave;
        textoBoton.text = "<color=#FFD700>Presiona una tecla...</color>"; 
    }

    void OnGUI()
    {
        Event eventoActual = Event.current;
        if (esperandoTecla && eventoActual.isKey && eventoActual.keyCode != KeyCode.None)
        {
            PlayerPrefs.SetString(accionAEditar, eventoActual.keyCode.ToString());
            PlayerPrefs.Save(); 

            esperandoTecla = false;
            accionAEditar = "";
            ActualizarInterfazTexto();
        }
    }

    // ==========================================
    // LÓGICA DE AUDIO
    // ==========================================
    void CargarConfiguracionAudio()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("VolumenGeneral", 0.5f);
        if (sliderVolumen != null) sliderVolumen.value = volumenGuardado;

        bool muteGuardado = PlayerPrefs.GetInt("AudioMute", 0) == 1;
        if (toggleMute != null) toggleMute.isOn = muteGuardado;

        if (muteGuardado)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = volumenGuardado;
        }
    }

    public void CambiarVolumen(float valor)
    {
        // Si el script se está inicializando, ignoramos el evento
        if (inicializando) return; 

        if (toggleMute != null && !toggleMute.isOn)
        {
            AudioListener.volume = valor;
        }

        PlayerPrefs.SetFloat("VolumenGeneral", valor);
        PlayerPrefs.Save();
    }

    public void CambiarMute(bool estaSilenciado)
    {
        // Si el script se está inicializando, ignoramos el evento
        if (inicializando) return; 

        if (estaSilenciado)
        {
            AudioListener.volume = 0f; 
            PlayerPrefs.SetInt("AudioMute", 1); 
        }
        else
        {
            float volumenActual = (sliderVolumen != null) ? sliderVolumen.value : 0.5f;
            AudioListener.volume = volumenActual;
            PlayerPrefs.SetInt("AudioMute", 0); 
        }
        PlayerPrefs.Save();
    }
}