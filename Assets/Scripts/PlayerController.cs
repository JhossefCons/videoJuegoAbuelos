using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSourse;

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

    //Sonidos
    [SerializeField] private AudioClip audio1;
    [SerializeField] private AudioClip audio2;

    private bool muerte = false;

    public BarraVida vida;
    public Vector3 posicionInicial = new Vector3(0, -0.2f, -0.07f);

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("arma"))
        {
            vida.RecibirDanio(3);
        }
    }

    void Start()
    {
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();


        velocidadOriginal = velocidad;
    }

    void Update()
    {

        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        if (!muerte)
        {
            verificarSuelo();
            moverPersonaje();
        }

        if (vida.ObtenerVida() <= 0 && !muerte)
        {
            

            HasMuerto pantallaMuerte = FindObjectOfType<HasMuerto>();
            if (pantallaMuerte != null)
            {
                pantallaMuerte.MostrarPantallaMuerte();
            }

            EjecutarMuerte();
        }

    }

    public void EjecutarMuerte()
    {
        Debug.Log("Animación de muerte ejecutada.");
        animacion.SetBool("Muerte", true);
        Invoke("ReiniciarJuego", 2f);
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
                movimiento.x = 0;
                agachado = false;
                animacion.SetBool("Agachado", false);
                velocidad = velocidadOriginal;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                animacion.SetBool("Golpear", true);
                audioSourse.PlayOneShot(audio1);
                movimiento.x = 0;
            }

            if (Input.GetButtonDown("Jump") && !enAire && !agachado)
            {
                enAire = true;
                movimiento.y = fuerzaSalto;
                animacion.SetBool("Saltando", true);
                movimiento.x = 0;
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

    private void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargar la escena actual
    }

    public void FinalGolpe()
    {
        animacion.SetBool("Golpear", false);
    }
}
