using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour
{
	[Header("Key Bindings")]
	[SerializeField] private KeyCode spaceKey = KeyCode.Space;
	[SerializeField] private KeyCode attackKey = KeyCode.LeftAlt;
	[SerializeField] private KeyCode attackButton = KeyCode.Mouse0;

	[Header("Movement")]
	[SerializeField] private float speed = 2f;
	[SerializeField] private float jumpForce = 500f;
	public bool OnAttack = false;

	[Header("Grounded")]
	[SerializeField] private bool grounded;
	[SerializeField] private float groundRadius = 0.02f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask theGround; // check every layer except the layer "Player" (because it is not checked in the inspector)

	[Header("Sound")]
	[SerializeField] private AudioClip soundAttack;
	[SerializeField] private AudioClip soundJump;
	[SerializeField] private AudioClip soundHurt;
	[SerializeField] private AudioClip soundDead;
	[SerializeField] private AudioClip soundWin;
	[SerializeField] private AudioClip soundGoblin;
	[SerializeField] private AudioClip soundPickUp;

	[Header("Progress Bar")]
	[SerializeField] private ProgressBar pb;
	[SerializeField] private float damage = 10f;

	[Header("Game Over")]
	[SerializeField] private GameObject canvasGameOver;

	private Rigidbody2D rb;
	private Animator animator;
	private AudioSource audioSource;
	private bool lookRight = true;
	private bool inputSpaceKey = false;

	void Awake ()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		OnAttack = false;
	}	
	
	void Update ()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, theGround);
		animator.SetBool("Grounded", grounded);

		float move = Input.GetAxis("Horizontal");
		transform.Translate(Vector2.right * move * speed * Time.deltaTime);
		animator.SetFloat("Speed", Mathf.Abs(move));
		animator.SetFloat("Vspeed", rb.velocity.y);

		if (move > 0 && !lookRight)
		{
			Flip();
		}
		else if (move < 0 && lookRight)
		{
			Flip();
		}

		if (Input.GetKeyDown(spaceKey) && grounded)
		{
			inputSpaceKey = true;

		}

		if ((Input.GetKeyDown(attackKey) || Input.GetKeyDown(attackButton)) && grounded && !OnAttack)
		{
			OnAttack = true;
			animator.SetTrigger("Attack");
			audioSource.PlayOneShot(soundAttack);
		}

	}

	private void FixedUpdate()
	{
		if (inputSpaceKey && grounded)
		{
			rb.AddForce(new Vector2(0, jumpForce));
			audioSource.PlayOneShot(soundJump);
			inputSpaceKey = false;
		}
	}


	void Flip()
	{
		lookRight = !lookRight;
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Hurt()
	{
		animator.SetTrigger("Hurt");
		audioSource.PlayOneShot(soundHurt);
		pb.Val -= damage;
		
		if (pb.Val <= 0)
		{
			canvasGameOver.SetActive(true);

		}
	}

	public void Win()
	{
		animator.SetTrigger("Win");
		audioSource.PlayOneShot(soundWin);
	}

	public void Dead()
	{
		animator.SetTrigger("Dead");
		audioSource.PlayOneShot(soundDead);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		switch (collision.gameObject.tag)
		{
			case "WesternShooter": // vu que les actions pour WesternShooter et Goblin sont les même, inutile de les recopier, donc comme il n'y a pas de break, le WesternShooter executera ce qu'il y a dans Goblin
			case "Goblin":
				if (OnAttack)
				{
					StartCoroutine(ResetAttackAnyway());
					Rigidbody2D rbGoblin = collision.gameObject.GetComponent<Rigidbody2D>();
					rbGoblin.bodyType = RigidbodyType2D.Dynamic;
					rbGoblin.AddForce(Vector2.up * 2000);
					audioSource.PlayOneShot(soundGoblin);
				}
				else
				{
					PlayerHurtMove(collision);
				}
				break;

			case "Mace":
				PlayerHurtMove(collision);
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch (collision.tag)
		{
			case "Water":
				pb.Val = 0;
				canvasGameOver.SetActive(true);
				break;

			case "Heart":
				pb.Val += collision.gameObject.GetComponent<HeartScript>().value;
				Destroy(collision.gameObject);
				audioSource.PlayOneShot(soundPickUp);
				break;
		}
	}

	private IEnumerator ResetAttackAnyway()
	{
		yield return new WaitForSeconds(1f);
		OnAttack = false;
	}


	private void PlayerHurtMove(Collision2D collision)
	{
		Vector2 move = collision.transform.position - transform.position;
		rb.AddForce(move.normalized * -200);
		Hurt();
	}

	public void AttackBool()
	{
		OnAttack = false;
	}
}
