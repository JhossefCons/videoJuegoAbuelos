using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public bool atacando;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani= GetComponent<Animator>();
        target = GameObject.Find("PlayerX");
    }

    // Update is called once per frame
    void Update()
    {
        comportamientoEnemigo();
    }

    public void comportamientoEnemigo()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
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
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    ani.SetBool("CaminarEnemigo", true);
                    break;
            }
        }
        else
        {
            if(Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
                ani.SetBool("CaminarEnemigo", true);
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                ani.SetBool("GolpearEnemigo", false);
            }
            else
            {
                ani.SetBool("CaminarEnemigo", false);
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
}
