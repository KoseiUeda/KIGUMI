using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OntaSnapController : MonoBehaviour
{
    // HammeringとChiselingのターゲット位置と回転（ローカル座標系）
    public Vector3 targetLocalPositionHammering;  // Hammering用のターゲットローカル位置
    public Vector3 targetLocalRotationHammering;  // Hammering用のターゲットローカル回転

    public Vector3 targetLocalPositionChiseling;  // Chiseling用のターゲットローカル位置
    public Vector3 targetLocalRotationChiseling;  // Chiseling用のターゲットローカル回転

    public float snapRange = 0.5f;  // スナップする範囲

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private OntaBehavior ontaBehavior;  // OntaBehaviorの参照

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        ontaBehavior = GetComponent<OntaBehavior>();  // OntaBehaviorのコンポーネントを取得

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

        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Onta released");

        // HammeringとChiselingの2つのスナップ位置を確認してスナップ
        float distanceToHammering = Vector3.Distance(transform.localPosition, targetLocalPositionHammering);
        float distanceToChiseling = Vector3.Distance(transform.localPosition, targetLocalPositionChiseling);

        if (distanceToHammering <= snapRange)
        {
            SnapToHammering();  // Hammeringの場所にスナップ
        }
        else if (distanceToChiseling <= snapRange)
        {
            SnapToChiseling();  // Chiselingの場所にスナップ
        }
    }

    private void SnapToHammering()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Hammering用のローカル位置と回転にスナップ
        transform.localPosition = targetLocalPositionHammering;
        transform.localRotation = Quaternion.Euler(targetLocalRotationHammering);

        // OntaBehaviorの動きをリセット（CarvingCountはリセットしない）
        ResetOntaBehavior();

        Debug.Log("Onta snapped to Hammering position and OntaBehavior reset.");
    }

    private void SnapToChiseling()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Chiseling用のローカル位置と回転にスナップ
        transform.localPosition = targetLocalPositionChiseling;
        transform.localRotation = Quaternion.Euler(targetLocalRotationChiseling);

        Debug.Log("Onta snapped to Chiseling position.");
    }

    private void ResetOntaBehavior()
    {
        if (ontaBehavior != null)
        {
            // CarvingCountは保持して、その他の動作に関する変数をリセット
            ontaBehavior.currentMoveStep = ontaBehavior.initialMoveStep;
            ontaBehavior.isInserted = false;
            ontaBehavior.canMove = true;

            Debug.Log("OntaBehavior movement reset.");
        }
    }

    private void ResetPhysics()
    {
        rb.isKinematic = false;
    }
}
