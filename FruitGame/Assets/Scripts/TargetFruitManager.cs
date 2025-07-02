using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetFruitManager : MonoBehaviour
{
    public static TargetFruitManager Instance;

    [Header("Hedef Ayarları")]
    public List<Sprite> meyveSpriteListesi;
    public Image hedefMeyveUI;

    [HideInInspector] public Sprite aktifHedef;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(YeniHedefSec), 1f, Random.Range(15f, 20f));
        YeniHedefSec(); // İlk hedef
    }

    void YeniHedefSec()
    {
        aktifHedef = meyveSpriteListesi[Random.Range(0, meyveSpriteListesi.Count)];
        hedefMeyveUI.sprite = aktifHedef;
    }
}
