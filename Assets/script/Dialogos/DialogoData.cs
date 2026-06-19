using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Dialogo/Conversacion")]
public class DialogoData : ScriptableObject
{
    public string nombreNPC;
    public Sprite retrato;
    [TextArea(3, 10)] public string mensaje;
    
    [System.Serializable]
    public struct Opcion {
        public string textoRespuesta; // <-- ESTO ES LO QUE EL MANAGER BUSCA
        public DialogoData siguienteDialogo;
    }
    public Opcion[] opciones;
}