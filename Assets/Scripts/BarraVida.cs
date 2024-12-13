using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public PlayerController jugador; // Referencia al script del jugador

    private float vidaMaxima = 30f;
    private float vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    void Update()
    {

        Debug.LogError("Vida en vidaActual: "+vidaActual);

        ReducirVidaConTiempo();

        // Verifica si la vida llega a 0
        if (vidaActual == 0)
        {
            Debug.Log("BANDERA");
            jugador.EjecutarMuerte();
            
            Debug.Log("MUERTE");
        }
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
