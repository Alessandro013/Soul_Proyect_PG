using UnityEngine;

public class NPCInteraccion : MonoBehaviour
{
    public string nombreNPC;
    [TextArea(3, 10)]
    public string mensaje;
    public Sprite retratoNPC;
    
    // Arrastra aquí el objeto de texto o imagen que dice "[C] Hablar"
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
        // Detecta la tecla C si estás cerca y no estás hablando ya
        if (playerCerca && Input.GetKeyDown(KeyCode.C) && !DialogueManager.estaHablando)
        {
            manager.StartConversation(nombreNPC, mensaje, retratoNPC);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = true;
            if(iconoAviso != null) iconoAviso.SetActive(true); // Muestra el aviso
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCerca = false;
            if(iconoAviso != null) iconoAviso.SetActive(false); // Quita el aviso
        }
    }
}