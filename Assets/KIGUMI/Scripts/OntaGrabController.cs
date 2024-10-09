using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OntaGrabController : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    void Start()
    {
        // XRGrabInteractableとRigidbodyを取得
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable not found on the object.");
            return; // コンポーネントがない場合はこれ以上処理を進めない
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the object.");
            return; // コンポーネントがない場合はこれ以上処理を進めない
        }

        // 物理的な効果を適用
        rb.isKinematic = false;  // 動作中は物理演算に従う
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Onta grabbed");
        // 持っている間、物理演算を無効化（固定化）
        rb.isKinematic = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Onta released");
        // 放したら物理演算を再開
        rb.isKinematic = false;
    }
}
