using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OntaSnapController : MonoBehaviour
{
    public Vector3 targetLocalPositionHammering;
    public Vector3 targetLocalRotationHammering;
    public Vector3 targetLocalPositionChiseling;
    public Vector3 targetLocalRotationChiseling;

    public float snapRange = 0.5f;
    public float snapCooldown = 1.0f;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private OntaBehavior ontaBehavior;
    private FaceHighlight faceHighlight;
    private float lastSnapTime = 0f;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        ontaBehavior = GetComponent<OntaBehavior>();
        faceHighlight = GetComponent<FaceHighlight>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable not found on the object.");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the object.");
        }

        if (ontaBehavior == null)
        {
            Debug.LogError("OntaBehavior not found on the object.");
        }

        if (faceHighlight == null)
        {
            Debug.LogError("FaceHighlight not found on the object.");
        }

        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        ontaBehavior.enabled = false;
        faceHighlight.enabled = false;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        ontaBehavior.enabled = false;
        faceHighlight.enabled = false;
        Debug.Log("Onta grabbed. Both OntaBehavior and FaceHighlight disabled.");
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Onta released");

        if (Time.time - lastSnapTime < snapCooldown)
        {
            Debug.Log("Snap cooldown active, skipping snap.");
            return;
        }

        float distanceToHammering = Vector3.Distance(transform.localPosition, targetLocalPositionHammering);
        float distanceToChiseling = Vector3.Distance(transform.localPosition, targetLocalPositionChiseling);

        if (distanceToHammering <= snapRange)
        {
            SnapToHammering();
            ontaBehavior.ResetOnta(); // Hammering位置にスナップされたらOntaBehaviorをリセット
        }
        else if (distanceToChiseling <= snapRange)
        {
            SnapToChiseling();
        }
        else
        {
            DisableAllScripts();
        }
    }

    private void SnapToHammering()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.localPosition = targetLocalPositionHammering;
        transform.localRotation = Quaternion.Euler(targetLocalRotationHammering);

        rb.isKinematic = true;

        ontaBehavior.enabled = true;
        faceHighlight.enabled = false;

        lastSnapTime = Time.time;
        Debug.Log("Onta snapped to Hammering position. OntaBehavior enabled, FaceHighlight disabled.");
    }

    private void SnapToChiseling()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.localPosition = targetLocalPositionChiseling;
        transform.localRotation = Quaternion.Euler(targetLocalRotationChiseling);

        rb.isKinematic = true;

        ontaBehavior.enabled = false;
        faceHighlight.enabled = true;

        lastSnapTime = Time.time;
        Debug.Log("Onta snapped to Chiseling position. FaceHighlight enabled, OntaBehavior disabled.");
    }

    private void DisableAllScripts()
    {
        ontaBehavior.enabled = false;
        faceHighlight.enabled = false;
        Debug.Log("Both OntaBehavior and FaceHighlight disabled.");
    }
}