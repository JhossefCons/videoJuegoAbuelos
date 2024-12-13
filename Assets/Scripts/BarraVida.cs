using UnityEngine;
using UnityEngine.UI; 

public class BarraVida : MonoBehaviour
{
   
    public Image barraVida; 
    private float vidaMaxima = 100f;
    private float vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima; 
    }

    void Update()
    {
        ReducirVidaConTiempo();
    }

    private void ReducirVidaConTiempo()
    {
        // Reducir vida lentamente con el tiempo
        vidaActual -= Time.deltaTime;

        if (vidaActual < 0) vidaActual = 0; 

        // Actualiza la barra de vida (UI)
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        // Recibir daño al personaje
        vidaActual -= cantidad;

        if (vidaActual < 0) vidaActual = 0; 

        // Actualiza la barra de vida
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public void RestablecerVida()
    {
        vidaActual = vidaMaxima;
        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public float ObtenerVida()
    {
        return vidaActual;
    }
}
