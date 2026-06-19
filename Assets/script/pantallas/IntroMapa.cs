using UnityEngine;
using System.Collections;

public class IntroMapa : MonoBehaviour
{
    public GameObject canvasIntro; // Arrastra aquí tu Canvas_Introduccion
    public float tiempoVisible = 5f; // Tiempo que estará en pantalla

    void Start()
    {
        StartCoroutine(MostrarIntro());
    }

    IEnumerator MostrarIntro()
    {
        canvasIntro.SetActive(true); // Activa el canvas
        yield return new WaitForSeconds(tiempoVisible); // Espera 3 segundos
        canvasIntro.SetActive(false); // Lo desactiva
    }
}