using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class RaycastHandlerImproved : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform HammeringTarget;
    public Transform ChiselingTarget;

    public GameObject HammeringTargetObject;
    public GameObject ChiselingTargetObject;

    public Transform playerTransform;
    public GameObject ontObject;

    private OntaSnapController ontSnapController;
    private Rigidbody ontRigidbody;

    public float rayLength = 10f;
    public float angleThreshold = 10f;

    public InputActionProperty leftGripAction;

    private GameObject activeTargetObject = null;
    private bool isHoldingObject = false;

    void Start()
    {
        foreach (var interactor in FindObjectsOfType<XRBaseInteractor>())
        {
            interactor.selectEntered.AddListener(OnGrab);
            interactor.selectExited.AddListener(OnRelease);
        }

        leftGripAction.action.Enable();

        lineRenderer = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        HammeringTargetObject?.SetActive(false);
        ChiselingTargetObject?.SetActive(false);

        ontSnapController = ontObject.GetComponent<OntaSnapController>();
        ontRigidbody = ontObject.GetComponent<Rigidbody>();

        if (ontSnapController == null || ontRigidbody == null)
        {
            Debug.LogError("Missing required components on Onta object.");
        }
    }

    void Update()
    {
        if (isHoldingObject)
        {
            HideRayAndObjects();
            return;
        }

        bool isGripPressed = GetGripInput();

        if (isGripPressed)
        {
            ShowRay();
            UpdateRay();
            HandleClosestTarget();
        }
        else if (activeTargetObject != null)
        {
            TeleportToTarget();
        }
        else
        {
            HideRayAndObjects();
        }
    }

    private bool GetGripInput()
    {
        float gripValue = leftGripAction.action.ReadValue<float>();
        return gripValue > 0.1f;
    }

    private void ShowRay()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
    }

    private void UpdateRay()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);
    }

    private void HandleClosestTarget()
    {
        float distanceToHammering = Vector3.Distance(transform.position, HammeringTarget.position);
        float distanceToChiseling = Vector3.Distance(transform.position, ChiselingTarget.position);

        bool isHammeringInRange = IsTargetInRange(HammeringTarget);
        bool isChiselingInRange = IsTargetInRange(ChiselingTarget);

        if (isHammeringInRange && isChiselingInRange)
        {
            if (distanceToHammering < distanceToChiseling)
            {
                ShowTargetObject(HammeringTargetObject);
                HideTargetObject(ChiselingTargetObject);
                activeTargetObject = HammeringTargetObject;
            }
            else
            {
                ShowTargetObject(ChiselingTargetObject);
                HideTargetObject(HammeringTargetObject);
                activeTargetObject = ChiselingTargetObject;
            }
        }
        else if (isHammeringInRange)
        {
            ShowTargetObject(HammeringTargetObject);
            HideTargetObject(ChiselingTargetObject);
            activeTargetObject = HammeringTargetObject;
        }
        else if (isChiselingInRange)
        {
            ShowTargetObject(ChiselingTargetObject);
            HideTargetObject(HammeringTargetObject);
            activeTargetObject = ChiselingTargetObject;
        }
        else
        {
            HideTargetObject(HammeringTargetObject);
            HideTargetObject(ChiselingTargetObject);
            activeTargetObject = null;
        }
    }

    private bool IsTargetInRange(Transform target)
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        Vector3 toTarget = (target.position - rayOrigin).normalized;
        float angle = Vector3.Angle(rayDirection, toTarget);

        return angle < angleThreshold;
    }

    private void TeleportToTarget()
    {
        playerTransform.position = activeTargetObject.transform.position;

        if (activeTargetObject == HammeringTargetObject)
        {
            MoveOntaToHammering();
        }
        else if (activeTargetObject == ChiselingTargetObject)
        {
            MoveOntaToChiseling();
        }

        HideRayAndObjects();
    }

    private void MoveOntaToHammering()
    {
        ontObject.transform.localPosition = ontSnapController.targetLocalPositionHammering;
        ontObject.transform.localRotation = Quaternion.Euler(ontSnapController.targetLocalRotationHammering);

        EnableOntaBehavior(true);
        Debug.Log("Onta moved to Hammering position.");
    }

    private void MoveOntaToChiseling()
    {
        ontObject.transform.localPosition = ontSnapController.targetLocalPositionChiseling;
        ontObject.transform.localRotation = Quaternion.Euler(ontSnapController.targetLocalRotationChiseling);

        EnableOntaBehavior(false);
        Debug.Log("Onta moved to Chiseling position.");
    }

    private void EnableOntaBehavior(bool enableHammering)
    {
        var ontaBehavior = ontObject.GetComponent<OntaBehavior>();
        var faceHighlight = ontObject.GetComponent<FaceHighlight>();

        if (ontaBehavior != null)
        {
            ontaBehavior.enabled = enableHammering;
        }

        if (faceHighlight != null)
        {
            faceHighlight.enabled = !enableHammering;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHoldingObject = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHoldingObject = false;
    }

    private void HideRayAndObjects()
    {
        lineRenderer.enabled = false;
        HideTargetObject(HammeringTargetObject);
        HideTargetObject(ChiselingTargetObject);
        activeTargetObject = null;
    }

    private void ShowTargetObject(GameObject targetObject)
    {
        if (!targetObject.activeSelf)
        {
            targetObject.SetActive(true);

            foreach (var ps in targetObject.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Clear();
                ps.Play();
            }
        }
    }

    private void HideTargetObject(GameObject targetObject)
    {
        if (targetObject.activeSelf)
        {
            foreach (var ps in targetObject.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            targetObject.SetActive(false);
        }
    }
}
