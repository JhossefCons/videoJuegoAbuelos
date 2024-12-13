using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animacion;
    private float velocidad = 1.5f;
    private float inputHorizontal;
    private Quaternion rotacionPersonaje;
    private CharacterController controladorPersonaje;
    private Vector3 movimiento;
    private float gravedad = 9.8f;
    private float fuerzaSalto = 6f;
    private bool enAire = false;
    private bool agachado = false;
    private float velocidadOriginal;

    [Header("Configuración de Suelo")]
    public Transform puntoChequeoSuelo; // Punto de chequeo para detectar el suelo
    public float radioChequeo = 0.3f; // Radio del chequeo
    public LayerMask capaSuelo; // Determina qué capas son suelo
    private bool enSuelo = false; // Si está tocando el suelo

    void Start()
    {
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();
        Application.targetFrameRate = 60;

        velocidadOriginal = velocidad; // Inicializa la velocidad original
    }

    void Update()
    {
        // Detectar si está en el suelo
        verificarSuelo();
        moverPersonaje();
    }

    void verificarSuelo()
    {
        // Usamos OverlapSphere para detectar colisiones con el suelo
        enSuelo = Physics.CheckSphere(puntoChequeoSuelo.position, radioChequeo, capaSuelo);

        // Si está en el suelo, puede moverse normalmente
        if (enSuelo)
        {
            enAire = false;
            movimiento.y = 0; // Reiniciar movimiento vertical
        }
        else
        {
            movimiento.y -= gravedad * Time.deltaTime; // Aplicar gravedad si no está en el suelo
        }
    }

    void moverPersonaje()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        movimiento.z = inputHorizontal * velocidad;

        if (enSuelo)
        {
            Debug.Log("En el suelo");
            enAire = false;
            animacion.SetBool("Saltando", false); // Detener animación de salto

            if (inputHorizontal == 0 && !agachado)
            {
                animacion.SetBool("Caminando", false); // Animación Idle
            }

            if (Input.GetKeyDown(KeyCode.S)) // Presiona 'S' para agacharse
            {
                agachado = true;
                animacion.SetBool("Agachado", true);
                velocidad = velocidadOriginal / 2; // Reducir velocidad
            }
            else if (Input.GetKeyUp(KeyCode.S)) // Suelta 'S' para levantarse
            {
                agachado = false;
                animacion.SetBool("Agachado", false);
                velocidad = velocidadOriginal; // Restaurar velocidad
            }

            if (Input.GetButtonDown("Jump") && !enAire && !agachado) // Saltar
            {
                enAire = true;
                movimiento.y = fuerzaSalto;
                animacion.SetBool("Saltando", true);
            }
        }

        if (inputHorizontal != 0 && !agachado) // Rotar y caminar si no está agachado
        {
            rotacionPersonaje = Quaternion.LookRotation(new Vector3(0, 0, inputHorizontal));
            this.transform.rotation = rotacionPersonaje;
            animacion.SetBool("Caminando", true);
        }

        controladorPersonaje.Move(movimiento * Time.deltaTime); // Aplicar movimiento
    }

    private void OnDrawGizmos()
    {
        // Visualización del radio de detección del suelo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(puntoChequeoSuelo.position, radioChequeo);
    }
}
