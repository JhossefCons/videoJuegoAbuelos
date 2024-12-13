using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int rutina; // Controla la rutina de comportamiento del enemigo
    public float cronometro; // Cron�metro para cambiar de rutina
    public Animator ani; // Controlador de animaciones
    public GameObject target; // Referencia al jugador
    public bool atacando; // Controla si el enemigo est� atacando
    public float velocidadMovimiento = 4f; // Velocidad del movimiento horizontal
    private Vector3 direccionMovimiento; // Direcci�n de movimiento (adelante o atr�s)
    public float distanciaPerseguir = 5f; // Distancia para que el enemigo comience a perseguir al jugador
    public float distanciaAtaque = 1.3f; // Distancia para que el enemigo comience a atacar al jugador

    private void OnTriggerEnter(Collider other)
    {
        // Si el enemigo choca con un obst�culo, cambia de direcci�n
        if (other.CompareTag("obstaculo"))
        {
            direccionMovimiento = -direccionMovimiento; // Invierte la direcci�n
            transform.rotation = Quaternion.LookRotation(direccionMovimiento); // Mira en la nueva direcci�n
            print("Cambio de direcci�n por colisi�n con " + other.name);
        }

        if (other.CompareTag("arma"))
        {
            Debug.Log("danio");
        }
    }

    // Start se llama una vez antes de la primera ejecuci�n de Update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("PlayerX"); // Asigna el objetivo (jugador)
        direccionMovimiento = Vector3.forward; // Direcci�n inicial hacia adelante
    }

    public void comportamientoEnemigo()
    {
        float distanciaJugador = Vector3.Distance(transform.position, target.transform.position);

        // Si la distancia entre el enemigo y el jugador es mayor a la distancia de persecuci�n
        if (distanciaJugador > distanciaPerseguir)
        {
            ani.SetBool("CorrerEnemigo", false); // No est� corriendo hacia el jugador
            cronometro += Time.deltaTime;

            if (cronometro >= 3) // Cambiar rutina cada 3 segundos
            {
                rutina = Random.Range(0, 2); // Rutinas 0: quedarse quieto, 1: caminar horizontalmente
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0: // Quedarse quieto
                    ani.SetBool("CaminarEnemigo", false);
                    break;

                case 1: // Caminar horizontalmente (hacia adelante o atr�s)
                    ani.SetBool("CaminarEnemigo", true);
                    direccionMovimiento = Random.Range(0, 2) == 0 ? Vector3.forward : Vector3.back; // Elige aleatoriamente entre adelante o atr�s
                    rutina++;
                    break;

                case 2: // Continuar movi�ndose horizontalmente
                    transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime, Space.World);
                    ani.SetBool("CaminarEnemigo", true);
                    // Aqu� hacemos que el enemigo mire hacia la direcci�n en la que se mueve
                    transform.rotation = Quaternion.LookRotation(direccionMovimiento);
                    break;
            }
        }
        else // Si el jugador est� cerca, comenzar la persecuci�n o el ataque
        {
            if (distanciaJugador > distanciaAtaque && !atacando)
            {
                ani.SetBool("CorrerEnemigo", true);
                // Perseguir al jugador horizontalmente (solo en Z)
                Vector3 direccionHaciaJugador = (target.transform.position - transform.position).normalized;
                direccionMovimiento = new Vector3(0, 0, direccionHaciaJugador.z).normalized; // Mantener solo la direcci�n hacia adelante o atr�s
                transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime, Space.World);
                // Aqu� hacemos que el enemigo mire hacia el jugador
                transform.LookAt(new Vector3(transform.position.x, transform.position.y, target.transform.position.z));
                ani.SetBool("GolpearEnemigo", false);
            }
            else // Si est� lo suficientemente cerca, atacar
            {
                ani.SetBool("CaminarEnemigo", false);
                ani.SetBool("CorrerEnemigo", false);
                ani.SetBool("GolpearEnemigo", true);
                atacando = true;
            }
        }
    }

    public void FinalAni()
    {
        ani.SetBool("GolpearEnemigo", false);
        atacando = false;
    }

    // Update se llama una vez por frame
    void Update()
    {
        comportamientoEnemigo();
    }
}
