using UnityEngine;

public class ElevationExit : MonoBehaviour
{
	public Collider2D[] mountainColliders;
	public Collider2D[] boundaryColliders;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}

		foreach (Collider2D mountain in mountainColliders)
		{
			mountain.enabled = true;
		}

		foreach (Collider2D boundary in boundaryColliders)
		{
			boundary.enabled = false;
		}

		collision.GetComponent<SpriteRenderer>().sortingOrder = 10;
	}
}
