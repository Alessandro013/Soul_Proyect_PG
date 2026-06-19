using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText; // El texto principal del mensaje
    
    public TextMeshProUGUI nombreNPC_Text;
    public TextMeshProUGUI nombreJugador_Text;

    public Image retratoNPC;
    public Image retratoJugador;

    public static bool estaHablando = false;
    public Transform contenedorBotones; 
    public GameObject prefabBoton;      
    public float velocidadEscritura = 0.05f; 

    private bool terminamosDeEscribir = false;
    private string mensajeCompleto;
    private Coroutine corrutinaEscritura;

    public void StartConversation(DialogoData data)
    {
        estaHablando = true;
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;
        mensajeCompleto = data.mensaje;

        // LÓGICA DE ILUMINACIÓN, NOMBRES Y ALINEACIÓN
        if (data.nombreNPC == "Viajero") 
        {
            // Eres tú: Ilumina caballero, nombre a tu lado, texto alineado a la derecha
            retratoJugador.color = Color.white; 
            retratoNPC.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            nombreJugador_Text.text = data.nombreNPC;
            nombreNPC_Text.text = "";
            dialogueText.alignment = TextAlignmentOptions.Right; // Texto a la derecha
        }
        else 
        {
            // Es el NPC: Ilumina bruja, nombre a su lado, texto alineado a la izquierda
            retratoNPC.color = Color.white;
            retratoJugador.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            nombreNPC_Text.text = data.nombreNPC;
            nombreJugador_Text.text = "";
            dialogueText.alignment = TextAlignmentOptions.Left; // Texto a la izquierda
        }

        terminamosDeEscribir = false;
        if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
        corrutinaEscritura = StartCoroutine(EscribirDialogo(data.mensaje));

        LimpiarBotones();
        foreach (var opcion in data.opciones)
        {
            GameObject btn = Instantiate(prefabBoton, contenedorBotones);
            btn.GetComponentInChildren<TextMeshProUGUI>(true).text = opcion.textoRespuesta;
            btn.GetComponent<Button>().onClick.AddListener(() => {
                if (opcion.siguienteDialogo != null) StartConversation(opcion.siguienteDialogo);
                else EndConversation();
            });
        }
    }

    IEnumerator EscribirDialogo(string textoCompleto)
    {
        dialogueText.text = "";
        foreach (char letra in textoCompleto.ToCharArray())
        {
            dialogueText.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }
        terminamosDeEscribir = true;
    }

    void Update()
    {
        if (estaHablando && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)))
        {
            if (!terminamosDeEscribir)
            {
                StopCoroutine(corrutinaEscritura);
                dialogueText.text = mensajeCompleto;
                terminamosDeEscribir = true;
            }
        }
    }

    void LimpiarBotones()
    {
        foreach (Transform child in contenedorBotones) Destroy(child.gameObject);
    }

    public void EndConversation()
    {
        LimpiarBotones();
        estaHablando = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}