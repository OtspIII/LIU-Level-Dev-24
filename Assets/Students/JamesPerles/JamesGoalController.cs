using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController2 : MonoBehaviour
{
    public float range = 5f;
    public string winSceneName = "GameWin"; // Scene name variable exposed in the Unity Editor

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider in colliders)
            {
               
            }

            // Load the scene specified in the winSceneName variable
            SceneManager.LoadScene(winSceneName);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the range of the script
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}