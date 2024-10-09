using UnityEngine;

public class OntaCollisionLogger : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with object: " + collision.gameObject.name);

        if (collision.gameObject.name == "Chisel")
        {
            Debug.Log("Chisel collided with Onta!");
        }
    }
}
