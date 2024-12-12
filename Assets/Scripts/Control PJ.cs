using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Prueba : MonoBehaviour
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
    private bool agachado = false; // Nueva variable para agacharse
    private float velocidadOriginal; // Guardar la velocidad original

    void Start()
    {
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();
        Application.targetFrameRate = 60;

        velocidadOriginal = velocidad; // Inicializa la velocidad original
    }

    // Update is called once per frame
    void Update()
    {
        moverPersonaje();
    }

    void moverPersonaje()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        movimiento.z = inputHorizontal * velocidad;

        // en el suelo
        if (controladorPersonaje.isGrounded)
        {

            Debug.Log("EN EL SUELO");

            enAire = false;
            animacion.SetBool("Saltando", false);
            animacion.SetBool("Caminando", false); // aplicar animacion Idle

            // Controlar si el personaje está agachado
            if (Input.GetKeyDown(KeyCode.C)) // Presionar 'C' para agacharse
            {
                agachado = true;
                animacion.SetBool("Agachado", true);
                velocidad = velocidadOriginal / 2; // Reducir la velocidad al agacharse
            }
            else if (Input.GetKeyUp(KeyCode.C)) // Soltar 'C' para levantarse
            {
                agachado = false;
                animacion.SetBool("Agachado", false);
                velocidad = velocidadOriginal; // Restaurar la velocidad original
            }
        }
        else
        { // no suelo
            movimiento.y -= gravedad * Time.deltaTime; // caida, quitar gravedad
            
        }

        if (inputHorizontal != 0 && !agachado) // Solo rotar y caminar si no está agachado
        {
            rotacionPersonaje = Quaternion.LookRotation(new Vector3(0, 0, inputHorizontal));
            this.transform.rotation = rotacionPersonaje;
            animacion.SetBool("Caminando", true);
        }

        if (Input.GetButton("Jump") && !enAire && !agachado) // Salto permitido solo si no está agachado
        {
            enAire = true;
            animacion.SetBool("Saltando", true);
            movimiento.y = fuerzaSalto;
        }

        controladorPersonaje.Move(movimiento * Time.deltaTime); // moverse
    }
}
