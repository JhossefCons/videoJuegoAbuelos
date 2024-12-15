using UnityEngine;

public class MedicSoundController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip audio1;
    [SerializeField] private AudioClip audio2;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }
    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("ReproducirSonido");
            audioSource.PlayOneShot(audio1);
    }
}
