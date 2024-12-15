using UnityEngine;
using System.Collections;

public class HasMuerto : MonoBehaviour
{
    public GameObject gameObjectPanel; // Panel de muerte
    public PlayerController jugador;  // Referencia al jugador
    public BarraVida barraVida; // Referencia a la barra de vida

    public void MostrarPantallaMuerte()
    {
        gameObjectPanel.SetActive(true);

        // Iniciar la corutina para quitar la pantalla de muerte después de 4 segundos y reiniciar
        StartCoroutine(QuitarPantallaMuerteYReiniciar());
    }

    private IEnumerator QuitarPantallaMuerteYReiniciar()
    {
        yield return new WaitForSeconds(4f); // Esperar 4 segundos

        gameObjectPanel.SetActive(false); // Ocultar pantalla de muerte
    }

}
