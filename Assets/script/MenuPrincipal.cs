using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
        public void PlayGame()
    {
        // Carga la siguiente escena en la lista (tu nivel 1)
         SceneManager.LoadScene("mapa_decierto");
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
