using UnityEngine;

public class BigHammerFollow : MonoBehaviour
{
    public Transform rightHandIKTarget; // RightHandIK_target をセット

    void LateUpdate()
    {
        if (rightHandIKTarget != null)
        {
            transform.position = rightHandIKTarget.position;
            transform.rotation = rightHandIKTarget.rotation;
        }
    }
}
