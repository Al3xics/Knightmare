using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAWesternShooter : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float shootDistance = 5f;
    [SerializeField] private float shootForce = 500f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject bullet;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 startPosition;
    private RaycastHit2D hit;
    private bool lookRight = true;
    private bool shoot = false;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!shoot)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.right);

            if (Vector2.Distance(transform.position, startPosition) < 0.5f && !lookRight)
                FlipCharacter();
        }

        // IA Fire
        Vector2 rayDir = lookRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(transform.position, rayDir, shootDistance, layer);
        Debug.DrawRay(transform.position, rayDir * shootDistance);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                shoot = true;
                animator.SetBool("shoot", true);
                if (!audioSource.isPlaying) audioSource.Play();
            }
        }
        else
        {
            shoot = false;
            animator.SetBool("shoot", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WayPoint"))
        {
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        speed = -speed;
        lookRight = !lookRight;
    }

    public void ShootBullet()
    {
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector2 direction = lookRight ? Vector2.right : Vector2.left;
        b.GetComponent<Rigidbody2D>().AddForce(direction * shootForce);
    }
}
