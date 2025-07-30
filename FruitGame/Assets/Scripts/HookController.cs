using UnityEngine;
using UnityEngine.UI;

public class HookController : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;

    private float moveDirection = 0f;  // -1: sola, 1: sağa, 0: durdu

    // Mobil tuşlar (UI Button’lara bağlanacak)
    public void MoveLeft()
    {
        moveDirection = -1f;
        animator.SetBool("isWalking", true);
    }

    public void MoveRight()
    {
        moveDirection = 1f;
        animator.SetBool("isWalking", true);
    }

    public void StopMoving()
    {
        moveDirection = 0f;
        animator.SetBool("isWalking", false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Klavye kontrolü (mobil dışında test için)
        float input = Input.GetAxisRaw("Horizontal");

        if (input != 0)
        {
            moveDirection = input;
            animator.SetBool("isWalking", true);
        }
        else if (moveDirection == 0)
        {
            animator.SetBool("isWalking", false);
        }

        // Hareket
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);
    }
}

