using UnityEngine;
using System.Collections;

public class IntroMapa : MonoBehaviour
{
    public GameObject canvasIntro; 
    public float tiempoVisible = 3f; 

    void Start()
    {
        StartCoroutine(MostrarIntro());
    }

    IEnumerator MostrarIntro()
    {
        // 1. Congelamos el tiempo
        Time.timeScale = 0f;
        
        canvasIntro.SetActive(true); 
        
        // 2. Usamos WaitForSecondsRealtime porque el tiempo está congelado
        // Si usamos WaitForSeconds normal, ¡nunca terminaría la espera!
        yield return new WaitForSecondsRealtime(tiempoVisible); 
        
        canvasIntro.SetActive(false);
        
        // 3. Descongelamos el tiempo para que el jugador pueda moverse
        Time.timeScale = 1f;
    }
}