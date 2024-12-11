using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    private Rigidbody rb;

    public Animator animator;

    void Start()
    {
        // Obtén el Rigidbody del personaje
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Obtener entrada horizontal del jugador
        float horizontal = Input.GetAxis("Horizontal");

        // Calcular la nueva posición
        Vector3 movement = new Vector3(0, 0, horizontal) * speed * Time.fixedDeltaTime;

        if (horizontal != 0)
        {
            // Cambiar la dirección del personaje dependiendo de si va a la derecha o a la izquierda
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
        }

        // Mover al personaje con el Rigidbody
        rb.MovePosition(transform.position + movement);
    }
}
