using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton yapısı (birden fazla müzik yöneticisini engeller)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.clip = Resources.Load<AudioClip>("tiny-inventors_medium-1-329138"); // mp3 uzantısız yazılır
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Aynı anda birden fazla olmasın
        }
    }
}

