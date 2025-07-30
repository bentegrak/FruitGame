using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class HookMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Hareket Ayarları")]
    [Tooltip("Kancanın hareket hızı")] public float moveSpeed = 18f;
    [Tooltip("Ekran kenarlarından boşluk")] public float edgeMargin = 1.8f;
    [Tooltip("Kanca collider genişliği")] public float hookSize = 0.6f;

    [Header("Ağız Sprite'ları")]
    public GameObject mouthIdle;
    public GameObject mouthEat1;
    public GameObject mouthEat2;
    public GameObject mouthEat3; 
    public GameObject mouthEat4; 

    private Rigidbody2D rb;
    private Animator animator;
    private float minX, maxX;

    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        CalculateDynamicBounds();
        Debug.Log($"Hareket Alanı: {minX:F2} ile {maxX:F2} arası");

        // Başlangıçta ağızları ayarla
        if (mouthIdle != null) mouthIdle.SetActive(true);
        if (mouthEat1 != null) mouthEat1.SetActive(false);
        if (mouthEat2 != null) mouthEat2.SetActive(false);
    }

    void CalculateDynamicBounds()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        minX = -cameraWidth * 1.15f + edgeMargin + (hookSize / 2);
        maxX = cameraWidth * 1.15f - edgeMargin - (hookSize / 2);
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        bool moved = false;

        if (moveInput != 0)
        {
            Vector2 newPos = rb.position + Vector2.right * (moveInput * moveSpeed * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            rb.MovePosition(newPos);
            moved = true;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
            mousePos.y = rb.position.y;
            rb.MovePosition(mousePos);
            moved = true;
        }
#endif

        HandleMovementAnimation(moved);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        HandleMovementAnimation(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(eventData.position);
        touchPos.x = Mathf.Clamp(touchPos.x, minX, maxX);
        touchPos.y = rb.position.y;
        rb.MovePosition(touchPos);
        HandleMovementAnimation(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HandleMovementAnimation(false);
    }

    void HandleMovementAnimation(bool moved)
    {
        if (animator != null)
        {
            if (moved && !isMoving)
            {
                animator.SetBool("isWalking", true);
                isMoving = true;
            }
            else if (!moved && isMoving)
            {
                animator.SetBool("isWalking", false);
                isMoving = false;
            }
        }
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

            StartCoroutine(MouthEatEffect());

            Destroy(other.gameObject);
        }
    }

    private IEnumerator MouthEatEffect()
    {
        if (mouthIdle != null) mouthIdle.SetActive(false);

        // Tüm yeme efektlerini sırayla göster
        if (mouthEat1 != null) mouthEat1.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        if (mouthEat1 != null) mouthEat1.SetActive(false);

        if (mouthEat2 != null) mouthEat2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        if (mouthEat2 != null) mouthEat2.SetActive(false);

        if (mouthEat3 != null) mouthEat3.SetActive(true); // Yeni eklenen
        yield return new WaitForSeconds(0.1f);
        if (mouthEat3 != null) mouthEat3.SetActive(false); // Yeni eklenen

        if (mouthEat4 != null) mouthEat4.SetActive(true); // Yeni eklenen
        yield return new WaitForSeconds(0.1f);
        if (mouthEat4 != null) mouthEat4.SetActive(false); // Yeni eklenen

        if (mouthIdle != null) mouthIdle.SetActive(true);
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
}
