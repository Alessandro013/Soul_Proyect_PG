using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public static bool estaHablando = false;

    public float velocidadEscritura = 0.05f; 
    private bool terminamosDeEscribir = false;
    private string mensajeCompleto;
    private Coroutine corrutinaEscritura;

    public void StartConversation(string name, string message, Sprite portrait)
    {
        estaHablando = true;
        dialoguePanel.SetActive(true);
        nameText.text = name;
        mensajeCompleto = message; // Guardamos el texto completo
        portraitImage.sprite = portrait;
        
        Time.timeScale = 0f; 
        
        terminamosDeEscribir = false;
        if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
        corrutinaEscritura = StartCoroutine(EscribirDialogo(message));
    }

    IEnumerator EscribirDialogo(string textoCompleto)
    {
        dialogueText.text = "";
        foreach (char letra in textoCompleto.ToCharArray())
        {
            dialogueText.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }
        terminamosDeEscribir = true; // El texto terminó de salir solo
    }

    void Update()
    {
        if (estaHablando)
        {
            // Si el jugador presiona ESPACIO o C mientras se escribe:
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
            {
                if (!terminamosDeEscribir)
                {
                    // SALTAMOS EL TEXTO: Paramos la corrutina y mostramos todo de golpe
                    StopCoroutine(corrutinaEscritura);
                    dialogueText.text = mensajeCompleto;
                    terminamosDeEscribir = true;
                }
                else
                {
                    // Si ya se terminó de escribir y presionas de nuevo, cerramos el diálogo
                    EndConversation();
                }
            }
        }
    }

    public void EndConversation()
    {
        estaHablando = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}