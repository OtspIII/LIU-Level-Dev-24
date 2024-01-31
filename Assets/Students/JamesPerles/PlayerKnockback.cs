using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForceUp = 10f;
    public float knockbackForceBack = 5f; // Adjust this value as needed

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ApplyKnockback(collision.transform.position);
        }
    }

    public void ApplyKnockback(Vector3 enemyPosition)
    {
        // Get the knockback direction (away from the enemy)
        Vector2 knockbackDirection = (new Vector3(enemyPosition.x, enemyPosition.y, transform.position.z) - transform.position).normalized;

        // Debugging: Print the knockback direction
        Debug.Log("Knockback Direction: " + knockbackDirection);

        // Apply knockback force with separate horizontal and vertical components
        Vector2 knockbackForce = new Vector2(knockbackDirection.x * knockbackForceBack, knockbackDirection.y * knockbackForceUp);
        _rb.AddForce(knockbackForce, ForceMode2D.Impulse);

        // Debugging: Print the applied knockback force
        Debug.Log("Knockback Force: " + knockbackForce);
    }
}