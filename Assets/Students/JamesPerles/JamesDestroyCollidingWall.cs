using UnityEngine;

namespace Students.JamesPerles
{
    public class JamesDestroyCollidingWall : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                DestroyCollidingWallWithTag();
            }
        }

        private void DestroyCollidingWallWithTag()
        {
            // Check if there is a collision with a wall
            Collider2D wallCollider = GetCollidingWall();

            if (wallCollider != null)
            {
                // Destroy the colliding wall
                Destroy(wallCollider.gameObject);
            }
        }

        private Collider2D GetCollidingWall()
        {
            // Get the colliders involved in the collision
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

            // Find the first collider with the "Walls" tag
            foreach (Collider2D collidingWall in colliders)
            {
                if (collidingWall.CompareTag("Walls"))
                {
                    return collidingWall;
                }
            }

            return null;
        }
    }
}