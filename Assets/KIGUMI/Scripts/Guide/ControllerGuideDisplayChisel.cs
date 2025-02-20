using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabDisplayChisel : MonoBehaviour
{
    public GameObject player; // プレイヤー（VRカメラのTransform）
    public GameObject guideCanvas; // 操作方法の画像を表示するCanvas
    public GuideLineDrawer guideLineDrawer; // ガイドラインのスクリプト
    public XRGrabInteractable grabbableObject; // 掴めるオブジェクト
    public CanvasFollowChisel canvasFollowScript; // Canvasの追従スクリプト
    public float displayDistance = 2.0f; // 表示する距離の閾値

    private bool isGrabbed = false; // 掴んでいるかどうか

    void Start()
    {
        // 初期状態で非表示
        guideCanvas.SetActive(false);
        if (guideLineDrawer != null) guideLineDrawer.enabled = false;

        // 掴むオブジェクトが設定されていれば、イベントを登録
        if (grabbableObject != null)
        {
            grabbableObject.selectEntered.AddListener(OnGrab);
            grabbableObject.selectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        if (player == null) return;

        // 掴んでいる間は非表示
        if (isGrabbed)
        {
            guideCanvas.SetActive(false);
            if (guideLineDrawer != null) guideLineDrawer.enabled = false;
            return;
        }

        // アバターとの距離を計算
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // 一定の距離以内なら画像とガイドラインを表示、離れたら非表示
        bool shouldDisplay = (distance <= displayDistance);
        guideCanvas.SetActive(shouldDisplay);
        if (guideLineDrawer != null) guideLineDrawer.enabled = shouldDisplay;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Canvas の移動スクリプトに通知
        if (canvasFollowScript != null)
        {
            canvasFollowScript.OnGrab();
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;

        // Canvas の移動スクリプトに通知
        if (canvasFollowScript != null)
        {
            canvasFollowScript.OnRelease();
        }
    }

    void OnDestroy()
    {
        // イベントの解除
        if (grabbableObject != null)
        {
            grabbableObject.selectEntered.RemoveListener(OnGrab);
            grabbableObject.selectExited.RemoveListener(OnRelease);
        }
    }
}
