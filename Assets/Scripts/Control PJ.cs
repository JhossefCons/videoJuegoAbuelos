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

    void Start()
    {
        animacion = this.GetComponent<Animator>();
        controladorPersonaje = this.GetComponent<CharacterController>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        moverPersonaje();
        
    }

    void moverPersonaje(){
        
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        movimiento.z = inputHorizontal * velocidad;

        // en el suelo
        if(controladorPersonaje.isGrounded){
            enAire = false;
            animacion.SetBool("Saltando", false);
            animacion.SetBool("Caminando", false); // aplicar animacion Idle

        }else{ // no suelo
            movimiento.y -= gravedad*Time.deltaTime; // caida, quitar gravedad
        }

        if(inputHorizontal != 0){
            rotacionPersonaje = Quaternion.LookRotation(new Vector3(0,0,inputHorizontal));
            this.transform.rotation = rotacionPersonaje;
            animacion.SetBool("Caminando", true);

            Debug.Log("Caminando: " + animacion.GetBool("Caminando"));

            Debug.Log(animacion.GetBool("Caminando"));
        }  

        if(Input.GetButton("Jump") && !enAire){ // salto y no aire
            enAire = true;
            animacion.SetBool("Saltando", true);
            movimiento.y = fuerzaSalto;
        } 
        controladorPersonaje.Move(movimiento*Time.deltaTime); // moverse
    }

}
