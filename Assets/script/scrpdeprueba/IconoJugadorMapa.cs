using UnityEngine;

public class IconoJugadorMapa : MonoBehaviour {
    [Header("Referencias")]
    public Transform jugadorReal; // Arrastra a Soldier_0 aquí
    
    [Header("Ajustes de Escala")]
    // Este valor depende de qué tan grande sea tu mapa en el HUD
    // Si el icono se mueve muy rápido, baja este número (ej. 5)
    // Si se mueve muy lento, súbelo (ej. 50)
    public float factorEscala = 20f; 

    private RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        if (jugadorReal != null) {
            // Calculamos la posición proporcional
            Vector3 nuevaPos = new Vector3(
                jugadorReal.position.x * factorEscala, 
                jugadorReal.position.y * factorEscala, 
                0
            );

            // Aplicamos la posición al RectTransform de la imagen
            rectTransform.localPosition = nuevaPos;
        }
    }
}