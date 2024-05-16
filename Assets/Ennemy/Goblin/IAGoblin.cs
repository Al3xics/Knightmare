using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAGoblin : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private bool lookRight = true;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);

        if (Vector2.Distance(transform.position, startPosition) < 0.5f && !lookRight) FlipCharacter();
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
}
