using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class HookMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Hareket Ayarları")]
    [Tooltip("Kancanın hareket hızı")] public float moveSpeed = 18f;
    [Tooltip("Ekran kenarlarından boşluk")] public float edgeMargin = 1.8f;
    [Tooltip("Kanca collider genişliği")] public float hookSize = 0.6f;

    private Rigidbody2D rb;
    private float minX, maxX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        CalculateDynamicBounds();
        Debug.Log($"Hareket Alanı: {minX:F2} ile {maxX:F2} arası");
    }

    void CalculateDynamicBounds()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // %15 ekstra hareket alanı + edgeMargin
        minX = -cameraWidth * 1.15f + edgeMargin + (hookSize / 2);
        maxX = cameraWidth * 1.15f - edgeMargin - (hookSize / 2);
    }

    void Update()
    {
        // PC Kontrolleri
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0)
        {
            Vector2 newPos = rb.position + Vector2.right * (moveInput * moveSpeed * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            rb.MovePosition(newPos);
        }

        // Editor'de fare simülasyonu
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
            mousePos.y = rb.position.y;
            rb.MovePosition(mousePos);
        }
#endif
    }

    // Mobil Dokunmatik Kontroller
    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        touchPos.x = Mathf.Clamp(touchPos.x, minX, maxX);
        touchPos.y = rb.position.y;
        rb.MovePosition(touchPos);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.4f);
        Gizmos.DrawCube(new Vector3((minX + maxX) / 2, transform.position.y, 0),
                       new Vector3(maxX - minX, 1, 0));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(minX, transform.position.y - 5, 0),
                       new Vector3(minX, transform.position.y + 5, 0));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y - 5, 0),
                       new Vector3(maxX, transform.position.y + 5, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meyve"))
        {
            Sprite yakalananMeyveSprite = other.GetComponent<SpriteRenderer>().sprite;

            if (GameManager.instance.MeyveDogruMu(yakalananMeyveSprite))
            {
                Debug.Log("Doğru meyve yakalandı: " + yakalananMeyveSprite.name);
                GameManager.instance.PuanArttir();
            }
            else
            {
                Debug.Log("Yanlış meyve yakalandı: " + yakalananMeyveSprite.name);
                GameManager.instance.CanAzalt();
            }

            Destroy(other.gameObject);
        }
    }


}