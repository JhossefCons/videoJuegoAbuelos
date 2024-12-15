using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public PlayerController jugador; // Referencia al script del jugador

    private float vidaMaxima = 60f;
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
        vidaActual -= Time.deltaTime;
        if (vidaActual < 0) vidaActual = 0;

        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        barraVida.fillAmount = vidaActual / vidaMaxima;
    }

    public void Curarse(float cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) { vidaActual = vidaMaxima; }
        if (vidaActual < 0) vidaActual = 0;
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
