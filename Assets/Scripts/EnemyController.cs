using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public GameObject target;
    public bool atacando;
    public float velocidadMovimiento = 4f;
    private Vector3 direccionMovimiento;
    public float distanciaPerseguir = 5f;
    public float distanciaAtaque = 1.3f;
    public float vidaEnemigo = 10f; // instancia de la barra de vida del enemigo

    private bool muerto = false; // Controla si el enemigo ya ha muerto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstaculo"))
        {
            direccionMovimiento = -direccionMovimiento;
            transform.rotation = Quaternion.LookRotation(direccionMovimiento);
            print("Cambio de dirección por colisión con " + other.name);
        }

        if (other.CompareTag("armaPlayer"))
        {
            Debug.Log("Golpe de jugador recibido");
            vidaEnemigo= vidaEnemigo-10f;
        }
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("PlayerX");
        direccionMovimiento = Vector3.forward;
    }

    public void comportamientoEnemigo()
    {
        if (muerto) return; // No ejecutar lógica si está muerto

        float distanciaJugador = Vector3.Distance(transform.position, target.transform.position);

        if (distanciaJugador > distanciaPerseguir)
        {
            ani.SetBool("CorrerEnemigo", false);
            cronometro += Time.deltaTime;

            if (cronometro >= 3)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("CaminarEnemigo", false);
                    break;
                case 1:
                    ani.SetBool("CaminarEnemigo", true);
                    direccionMovimiento = Random.Range(0, 2) == 0 ? Vector3.forward : Vector3.back;
                    rutina++;
                    break;
                case 2:
                    transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime, Space.World);
                    ani.SetBool("CaminarEnemigo", true);
                    transform.rotation = Quaternion.LookRotation(direccionMovimiento);
                    break;
            }
        }
        else
        {
            if (distanciaJugador > distanciaAtaque && !atacando)
            {
                ani.SetBool("CorrerEnemigo", true);
                Vector3 direccionHaciaJugador = (target.transform.position - transform.position).normalized;
                direccionMovimiento = new Vector3(0, 0, direccionHaciaJugador.z).normalized;
                transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime, Space.World);
                transform.LookAt(new Vector3(transform.position.x, transform.position.y, target.transform.position.z));
                ani.SetBool("GolpearEnemigo", false);
            }
            else
            {
                ani.SetBool("CaminarEnemigo", false);
                ani.SetBool("CorrerEnemigo", false);
                ani.SetBool("GolpearEnemigo", true);
                atacando = true;
            }
        }
    }

    public void ejecutarMuerte()
    {
        if (muerto) return; // Evitar múltiples ejecuciones de la muerte

        Debug.Log("Enemigo ha muerto");
        ani.SetBool("MorirEnemigo", true);
        muerto = true;
        Invoke("DestruirEnemigo", 3f); // Destruir al enemigo después de 3 segundos
    }

    private void DestruirEnemigo()
    {
        Destroy(gameObject);
    }

    public void FinalAni()
    {
        ani.SetBool("GolpearEnemigo", false);
        atacando = false;
    }

    void Update()
    {
        if (!muerto)
        {
            comportamientoEnemigo();
        }

        if (vidaEnemigo <= 0)
        {
            ejecutarMuerte();
        }
    }
}
