using UnityEngine;
using UnityEngine.UI; 

public class BarraVida : MonoBehaviour
{
   
    public Image barraVida; 
    private float vidaMaxima = 2f;
    private float vidaActual;
    private Animator animacion;
    private CharacterController controladorPersonaje;

    void Start()
    {   
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();
        Application.targetFrameRate = 60;

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
        // Recibir da�o al personaje
        vidaActual -= cantidad;

        if (vidaActual < 0) vidaActual = 0;

        // Verifica si la vida llega a 0
        if (vidaActual == 0)
        {
            animacion.SetBool("Muerte", true);

            Debug.Log("Muerte");
        }

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
