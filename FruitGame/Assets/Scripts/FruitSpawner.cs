using UnityEngine;
using System.Collections;

public class FruitSpawner : MonoBehaviour
{
    [Header("Meyve Sprite'ları")]
    public Sprite[] meyveSpriteLari;

    [Header("Spawn Ayarları")]
    public float spawnAraligi = 1f;
    public float spawnGenisligi = 8f;
    public float baslangicYuksekligi = 7f;

    [Header("Boyut Ayarları")]
    [Tooltip("Dünya birimlerinde istediğiniz ortalama meyve boyutu")]
    public float hedefMeyveBoyutu = 1.5f;

    [Header("Fizik Ayarları")]
    [Tooltip("Meyvelerin düşme hızı (0.1 = çok yavaş, 1 = normal, 2 = hızlı)")]
    [Range(0.1f, 2f)] public float dusmeHizi = 0.5f;
    [Tooltip("Meyveler arası hız farkı (0 = hepsi aynı, 0.5 = %50 varyasyon)")]
    [Range(0f, 1f)] public float hizVaryasyonu = 0.3f;
    [Tooltip("Meyvelerin dönme efekti için tork gücü")]
    public float donmeSiddeti = 5f;

    void Start()
    {
        StartCoroutine(MeyveYagdir());
    }

    IEnumerator MeyveYagdir()
    {
        while (true)
        {
            Sprite rastgeleSprite = meyveSpriteLari[Random.Range(0, meyveSpriteLari.Length)];
            GameObject yeniMeyve = new GameObject(rastgeleSprite.name);
            yeniMeyve.tag = "Meyve";

            // SPRITE AYARLARI
            SpriteRenderer sr = yeniMeyve.AddComponent<SpriteRenderer>();
            sr.sprite = rastgeleSprite;
            sr.sortingOrder = 2;

            // BOYUT AYARLAMA
            float spriteGenislik = rastgeleSprite.bounds.size.x;
            float scaleDegeri = hedefMeyveBoyutu / spriteGenislik;
            yeniMeyve.transform.localScale = new Vector3(scaleDegeri, scaleDegeri, 1f);

            // FİZİK AYARLARI (DÜŞME HIZI EKLENDİ)
            Rigidbody2D rb = yeniMeyve.AddComponent<Rigidbody2D>();
            float rastgeleHizCarpani = Random.Range(1f - hizVaryasyonu, 1f + hizVaryasyonu);
            rb.gravityScale = dusmeHizi * rastgeleHizCarpani;
            rb.AddTorque(Random.Range(-donmeSiddeti, donmeSiddeti), ForceMode2D.Impulse);

            // COLLIDER AYARI
            CircleCollider2D collider = yeniMeyve.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = spriteGenislik * scaleDegeri / 8f;

            // POZİSYON AYARI
            yeniMeyve.transform.position = new Vector3(
                Random.Range(-spawnGenisligi / 2, spawnGenisligi / 2),
                baslangicYuksekligi,
                0
            );

            // OTOMATİK SİLME (Performans için)
            Destroy(yeniMeyve, 10f);

            yield return new WaitForSeconds(spawnAraligi);
        }
    }
}