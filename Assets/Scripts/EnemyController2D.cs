using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController2D : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public GameObject target;
    public bool atacando;
    public float velocidadMovimiento = 1f;
    private int direccionMovimiento;
    public float distanciaPerseguir = 5f;
    public float distanciaAtaque = 1.3f;
    public float vidaEnemigo = 10f; // instancia de la barra de vida del enemigo

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("PlayerX");
    }

    // Update is called once per frame
    void Update()
    {
        ComportamientoEnemigo();
    }

    public void ComportamientoEnemigo()
    {
        ani.SetBool("correr",false);
        cronometro += 1 * Time.deltaTime;
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }

        switch (rutina)
        {
            case 0:
                ani.SetBool("caminar", false);
                    break;
            case 1:
                direccionMovimiento = Random.Range(0, 2);
                rutina++;
                break;

            case 2:

                switch (direccionMovimiento)
                {
                    case 0:
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime);
                        break;

                    case 1:
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        transform.Translate(Vector3.back * velocidadMovimiento * Time.deltaTime);
                        break;
                }
                ani.SetBool("correr", true);
                break;
        }
    }
}
