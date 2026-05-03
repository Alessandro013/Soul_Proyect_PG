using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;
    public TextMeshProUGUI contadorTexto;
    private int monedasTotales = 0;

    void Awake() {
        if (instancia == null) {
            instancia = this;
        } else if (instancia != this) {
            Destroy(gameObject);
            return;
        }

        if (contadorTexto == null) {
            Debug.LogWarning("GameManager: contadorTexto no está asignado en el Inspector.");
        }
    }

    public void SumarMonedas(int cantidad) {
        monedasTotales += cantidad;
        if (contadorTexto != null) {
            contadorTexto.text = monedasTotales.ToString();
        } else {
            Debug.LogWarning("GameManager: no se puede actualizar contadorTexto porque está vacío.");
        }
    }
}