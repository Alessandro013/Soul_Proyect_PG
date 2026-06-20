using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Asegúrate de que este nombre coincida exactamente con tu sala de espera
    public void VolverAlJuego()
    {
        SceneManager.LoadScene("sala_espera"); 
    }
}