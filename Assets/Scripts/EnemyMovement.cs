using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Transform player;
	private EnemyState enemyState;
	private Animator anim;
	private int facingDirection = 1;
	public float speed;
	public float attackRange = 2;
	public float attackCooldown = 2;
	public float attackCooldownTimer;
	public Transform detectionPoint;
	public LayerMask playerLayer;
	public float playerDetectRange = 5;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		ChangeState(EnemyState.Idle);
	}

	void Update()
	{
		if (attackCooldownTimer > 0)
			attackCooldownTimer -= Time.deltaTime;
		if (enemyState == EnemyState.Chasing)
		{
			Chase();
		}
		else if (enemyState == EnemyState.Attacking)
		{
			// Do attack
			rb.linearVelocity = Vector2.zero;
		}
		CheckForPlayer();
	}

	void Chase()
	{
		if (player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
		{
			Flip();
		}
		Vector2 directon = (player.position - transform.position).normalized;
		rb.linearVelocity = directon * speed;
	}

	private void CheckForPlayer()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
		if (hits.Length > 0)
		{
			player = hits[0].transform;
			if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
			{
				attackCooldownTimer = attackCooldown;
				ChangeState(EnemyState.Attacking);
			}
			else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
			{
				ChangeState(EnemyState.Chasing);
			}
		}
		else
		{
			rb.linearVelocity = Vector2.zero;
			ChangeState(EnemyState.Idle);
		}
	}
	private void Flip()
	{
		facingDirection *= -1;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	void ChangeState(EnemyState newState)
	{
		// exit the current animation
		if (enemyState == EnemyState.Idle)
		{
			anim.SetBool("isIdle", false);
		}
		else if (enemyState == EnemyState.Chasing)
		{
			anim.SetBool("isMoving", false);
		}
		else if (enemyState == EnemyState.Attacking)
		{
			anim.SetBool("isAttacking", false);
		}

		// update our current state
		enemyState = newState;

		// update our current state
		if (enemyState == EnemyState.Idle)
		{
			anim.SetBool("isIdle", true);
		}
		else if (enemyState == EnemyState.Chasing)
		{
			anim.SetBool("isMoving", true);
		}
		else if (enemyState == EnemyState.Attacking)
		{
			anim.SetBool("isAttacking", true);
		}
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
	}
}


public enum EnemyState
{
	Idle,
	Chasing,
	Attacking
}
