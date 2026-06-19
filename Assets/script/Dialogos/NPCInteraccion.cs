using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    // Ahora en lugar de escribir todo aquí, solo arrastras el archivo "DialogoData"
    public DialogoData dialogoInicial; 
    
    public GameObject iconoAviso; 

    private bool playerCerca = false;
    private DialogueManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<DialogueManager>();
        if(iconoAviso != null) iconoAviso.SetActive(false);
    }

    void Update()
    {
        // El manager ahora recibe el archivo completo (dialogoInicial)
        if (playerCerca && Input.GetKeyDown(KeyCode.C) && !DialogueManager.estaHablando)
        {
            if (manager != null && dialogoInicial != null)
            {
                manager.StartConversation(dialogoInicial);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = true;
            if(iconoAviso != null) iconoAviso.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            if(iconoAviso != null) iconoAviso.SetActive(false);
        }
    }
}