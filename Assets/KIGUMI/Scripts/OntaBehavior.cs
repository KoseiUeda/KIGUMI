using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f;
    public float minY = 1.0f;
    public float decreaseFactor = 0.94f;  // Œ¸­ŒW”‚ğ’²®
    private float currentMoveStep;
    private bool canMove = true;
    private float cooldown = 0.5f;

    void Start()
    {
        currentMoveStep = initialMoveStep;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove)
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}"); // Debug statement
            if (transform.position.y - currentMoveStep > minY)
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor; // ˆÚ“®‹——£‚ğŒ¸­
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}"); // Debug statement
                canMove = false;
                Invoke("ResetMovement", cooldown);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;
    }
}
