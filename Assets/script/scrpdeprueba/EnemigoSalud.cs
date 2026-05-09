using UnityEngine;
using UnityEngine.UI;

public class EnemigoSalud : MonoBehaviour {
    public float vidaMax = 10;
    private float vidaActual;
    public Image barraRoja; // Arrastra el componente Image aquí

    void Start() {
        vidaActual = vidaMax;
    }

    public void RecibirDaño(float daño) {
        vidaActual -= daño;
        barraRoja.fillAmount = vidaActual / vidaMax; // Baja la barrita
        if (vidaActual <= 0) Morir();
    }

    void Morir() {
        Destroy(gameObject);
    }
}