using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
	public float speed = 5;
	public Rigidbody2D rb;
	private bool isKnockedBack;
	public Animator anim;
	public int facingDirection = 1;
	[SerializeField] private InputActionAsset inputActions;
	[SerializeField] private string moveActionName = "Player/Move";

	public PlayerCombat playerCombat;

	private InputAction moveAction;
	private InputAction mouseAction;
	private void Update()
	{
		if (mouseAction.WasPressedThisFrame())
		{
			playerCombat.Attack();
		}
	}
	void Awake()
	{
		if (inputActions == null)
		{
			Debug.LogError("Assign an Input Actions asset to Player Movement.", this);
			enabled = false;
			return;
		}

		moveAction = inputActions.FindAction(moveActionName);
		mouseAction = inputActions.FindAction("Attack");
		if (moveAction == null)
		{
			Debug.LogError($"Action '{moveActionName}' was not found in the assigned Input Actions asset.", this);
			enabled = false;
		}
	}

	void OnEnable()
	{
		moveAction.Enable();
	}

	void OnDisable()
	{
		moveAction.Disable();
	}

	void FixedUpdate()
	{
		if (!isKnockedBack)
		{
			Vector2 movement = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
			rb.linearVelocity = movement * speed;
			anim.SetFloat("horizontal", Mathf.Abs(movement.x));
			anim.SetFloat("vertical", Mathf.Abs(movement.y));
			if (movement.x > 0 && transform.localScale.x < 0 || movement.x < 0 && transform.localScale.x > 0)
			{
				Flip();
			}
		}
	}

	void Flip()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		facingDirection *= -1;
	}
	public void Knockback(Transform enemy, float force, float stunTime)
	{
		isKnockedBack = true;
		Vector2 direction = (transform.position - enemy.position).normalized;
		rb.linearVelocity = direction * force;
		StartCoroutine(KnockbackCounter(stunTime));
	}
	IEnumerator KnockbackCounter(float stunTime)
	{
		yield return new WaitForSeconds(stunTime);
		rb.linearVelocity = Vector2.zero;
		isKnockedBack = false;
	}
}
