using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    [Header("Configuración")]
    public DialogoData dialogoInicial; 
    
    // Aquí arrastras el icono que vive en tu Canvas/Panel
    public GameObject iconoAviso; 

    private bool playerCerca = false;
    private DialogueManager manager;

    void Start()
    {
        manager = Object.FindFirstObjectByType<DialogueManager>();
        
        // Empezamos con el aviso desactivado
        if (iconoAviso != null) 
            iconoAviso.SetActive(false);
    }

    void Update()
    {
        if (playerCerca && Input.GetKeyDown(KeyCode.C))
        {
            if (manager != null && dialogoInicial != null && !DialogueManager.estaHablando)
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
            if (iconoAviso != null) iconoAviso.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            if (iconoAviso != null) iconoAviso.SetActive(false);
        }
    }
}