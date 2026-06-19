using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void PlayGame()
    {
        // Carga la siguiente escena en la lista (tu nivel 1)
        SceneManager.LoadScene("sala_espera");
    }

    public void QuitGame()
    {
        // Esto cierra el juego real una vez exportado (.exe)
        Application.Quit();

        // Esta instrucción especial le dice al editor de Unity que detenga el Play Mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
