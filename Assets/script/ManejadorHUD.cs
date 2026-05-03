using UnityEngine;
using UnityEngine.UI;

public class ManejadorHUD : MonoBehaviour
{
    public Image barraVida;
    public Image barraEscudo;
    public Image barraMana;

    public void ActualizarBarras(int vidaActual, int vidaMaxima, int escudoActual, int escudoMaximo, int manaActual, int manaMaximo)
    {
        if (barraVida != null) {
            barraVida.fillAmount = vidaMaxima > 0 ? (float)vidaActual / vidaMaxima : 0f;
        }

        if (barraEscudo != null) {
            barraEscudo.fillAmount = escudoMaximo > 0 ? (float)escudoActual / escudoMaximo : 0f;
        }

        if (barraMana != null) {
            barraMana.fillAmount = manaMaximo > 0 ? (float)manaActual / manaMaximo : 0f;
        }
    }
}
