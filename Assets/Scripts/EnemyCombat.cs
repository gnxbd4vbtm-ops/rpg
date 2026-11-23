using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
	public Transform attackPoint;
	public float weaponRange;
	public LayerMask playerLayer;
	public float knockbackForce;
	public int damage = 1;
	public float stunTime = 0.5f;
	/*
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
			}
		}
	*/
	public void Attack()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

		if (hits.Length > 0)
		{
			hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
			hits[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunTime);
		}
	}
}
