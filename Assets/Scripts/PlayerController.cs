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
    public Transform puntoChequeoSuelo;
    public float radioChequeo = 0.3f;
    public LayerMask capaSuelo;
    private bool enSuelo = false;

    private bool muerte = false;

    public BarraVida vida;
    public Vector3 posicionInicial = new Vector3(0, -0.2f, -0.07f);

    void Start()
    {
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();
        Application.targetFrameRate = 60;

        velocidadOriginal = velocidad;
        //posicionInicial = transform.position; // Guardar posición inicial

        vida = FindObjectOfType<BarraVida>();

        if (vida == null)
        {
            Debug.LogError("No se encontró la barra de vida.");
        }
    }

    void Update()
    {

        Debug.LogError("Vida en Jugador: "+vida.ObtenerVida());

        if (!muerte)
        {
            verificarSuelo();
            moverPersonaje();
        }

        if (vida.ObtenerVida() == 0 && !muerte)
        {
            EjecutarMuerte();

            HasMuerto pantallaMuerte = FindObjectOfType<HasMuerto>();
            if (pantallaMuerte != null)
            {
                pantallaMuerte.MostrarPantallaMuerte();
            }
        }

    }

    public void ReiniciarPosicion()
    {
        
        controladorPersonaje.enabled = false; // Deshabilitar el CharacterController
        transform.position = posicionInicial; // Cambiar posición
        controladorPersonaje.enabled = true;  // Rehabilitar el CharacterController

        movimiento = Vector3.zero; // Reinicia el movimiento
        animacion.SetBool("Muerte", false);
        animacion.SetBool("Saltando", false);
        animacion.SetBool("Caminando", false);
        animacion.SetBool("Agachado", false);


        /* Debug.Log("Saltando: "+animacion.GetBool("Saltando"));
        Debug.Log("Caminando: "+animacion.GetBool("Caminando"));
        Debug.Log("Agachado: "+animacion.GetBool("Agachado")); */
        

        muerte = false;


    }

    public void EjecutarMuerte()
    {
        Debug.Log("Animación de muerte ejecutada.");
        animacion.SetBool("Muerte", true);
        muerte = true;
    }

    void verificarSuelo()
    {
        enSuelo = Physics.CheckSphere(puntoChequeoSuelo.position, radioChequeo, capaSuelo);

        if (enSuelo)
        {
            enAire = false;
            movimiento.y = 0;
        }
        else
        {
            movimiento.y -= gravedad * Time.deltaTime;
        }
    }

    void moverPersonaje()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        movimiento.z = inputHorizontal * velocidad;

        if (enSuelo)
        {
            enAire = false;
            animacion.SetBool("Saltando", false);

            if (inputHorizontal == 0 && !agachado)
            {
                animacion.SetBool("Caminando", false);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                agachado = true;
                animacion.SetBool("Agachado", true);
                velocidad = velocidadOriginal / 2;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                agachado = false;
                animacion.SetBool("Agachado", false);
                velocidad = velocidadOriginal;
            }

            if (Input.GetButtonDown("Jump") && !enAire && !agachado)
            {
                enAire = true;
                movimiento.y = fuerzaSalto;
                animacion.SetBool("Saltando", true);
            }
        }

        if (inputHorizontal != 0 && !agachado)
        {
            rotacionPersonaje = Quaternion.LookRotation(new Vector3(0, 0, inputHorizontal));
            this.transform.rotation = rotacionPersonaje;
            animacion.SetBool("Caminando", true);
        }

        controladorPersonaje.Move(movimiento * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(puntoChequeoSuelo.position, radioChequeo);
    }
}
