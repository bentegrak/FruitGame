using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Hedef Meyve Paneli")]
    public Image hedefMeyveImage;
    public Sprite[] meyveSpritelari;

    [Header("Can Paneli")]
    public Image[] kalpler;

    [Header("Puan Sistemi")]
    public Slider puanSlider;
    public int puan = 0;
    public int maxPuan = 100;
    public int puanArtisi = 10;

    [Header("Ayarlamalar")]
    public float hedefDegisiklikSuresi = 15f;

    private Sprite suankiHedefMeyve;
    private int kalanCan = 3;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (kalpler.Length != 3)
        {
            Debug.LogError("Kalpler dizisi 3 element içermeli!");
        }

        if (puanSlider != null)
        {
            puanSlider.minValue = 0;
            puanSlider.maxValue = maxPuan;
            puanSlider.value = 0;
        }

        StartCoroutine(HedefMeyveDegistirRoutine());
        RastgeleHedefMeyveSec();
        KalanCanGoster();
    }

    IEnumerator HedefMeyveDegistirRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hedefDegisiklikSuresi);
            RastgeleHedefMeyveSec();
        }
    }

    void RastgeleHedefMeyveSec()
    {
        int index = Random.Range(0, meyveSpritelari.Length);
        suankiHedefMeyve = meyveSpritelari[index];
        hedefMeyveImage.sprite = suankiHedefMeyve;
    }

    public bool MeyveDogruMu(Sprite yakalananMeyve)
    {
        return yakalananMeyve == suankiHedefMeyve;
    }

    public void CanAzalt()
    {
        if (kalanCan > 0)
        {
            kalanCan--;
            KalanCanGoster();

            if (kalanCan <= 0)
            {
                GameOver();
            }
        }
    }

    void KalanCanGoster()
    {
        for (int i = 0; i < kalpler.Length; i++)
        {
            kalpler[i].enabled = i < kalanCan;
        }
    }

    public void PuanArttir()
    {
        puan += puanArtisi;
        if (puanSlider != null)
        {
            puanSlider.value = puan;
        }

        if (puan >= maxPuan)
        {
            GameWin();
        }
    }

    void GameOver()
    {
        Debug.Log("Oyun Bitti! Canlar tükendi.");
        Time.timeScale = 0f;
    }

    void GameWin()
    {
        Debug.Log("Tebrikler! Maksimum puana ulaştınız.");
        Time.timeScale = 0f;
    }
}
