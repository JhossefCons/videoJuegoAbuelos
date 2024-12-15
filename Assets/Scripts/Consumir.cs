using UnityEngine;

public class Consumir : MonoBehaviour
{
    public BarraVida vida;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("medicinaPotente"))
        {
            vida.Curarse(30f);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("medicinaNormal"))
        {
            vida.Curarse(10f);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("medicinaSuave"))
        {
            vida.Curarse(5f);
            Destroy(other.gameObject);
        }
    }
}
