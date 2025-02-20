using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerGuideFollow : MonoBehaviour
{
    public XRGrabInteractable chisel; // ノミの XRGrabInteractable
    public GameObject controllerGuideCanvas; // 操作ガイドの Canvas
    public Vector3 offset = new Vector3(0.1f, 0.1f, 0.2f); // 手からのオフセット位置

    private XRBaseInteractor grabbingHand; // 掴んでいる手
    private bool isVisible = false; // Canvas の表示状態

    void Start()
    {
        if (controllerGuideCanvas != null)
        {
            controllerGuideCanvas.SetActive(false); // 初期は非表示
        }

        // Grab イベントを登録
        if (chisel != null)
        {
            chisel.selectEntered.AddListener(OnGrab);
            chisel.selectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        if (isVisible && grabbingHand != null)
        {
            // 掴んでいる手の位置＋オフセットに Canvas を追従
            controllerGuideCanvas.transform.position = grabbingHand.transform.position
                                                       + grabbingHand.transform.right * offset.x
                                                       + grabbingHand.transform.up * offset.y
                                                       + grabbingHand.transform.forward * offset.z;

            // プレイヤーの方向を向く
            if (Camera.main != null)
            {
                controllerGuideCanvas.transform.LookAt(Camera.main.transform);
                controllerGuideCanvas.transform.Rotate(0, 180, 0); // 画像が反転しないように
            }
        }
    }

    // 🎯 FaceHighlight から呼び出す
    public void ShowCanvas()
    {
        if (controllerGuideCanvas != null && grabbingHand != null)
        {
            controllerGuideCanvas.SetActive(true);
            isVisible = true;
        }
    }

    // 🎯 FaceHighlight から呼び出す
    public void HideCanvas()
    {
        if (controllerGuideCanvas != null)
        {
            controllerGuideCanvas.SetActive(false);
            isVisible = false;
        }
    }

    // 🎯 Chisel を掴んだときにどちらの手かを取得
    private void OnGrab(SelectEnterEventArgs args)
    {
        grabbingHand = args.interactorObject as XRBaseInteractor;
    }

    // 🎯 Chisel を離したら手の情報をリセット
    private void OnRelease(SelectExitEventArgs args)
    {
        grabbingHand = null;
        HideCanvas(); // 離したらキャンバスも非表示
    }

    void OnDestroy()
    {
        // イベントの解除
        if (chisel != null)
        {
            chisel.selectEntered.RemoveListener(OnGrab);
            chisel.selectExited.RemoveListener(OnRelease);
        }
    }
}
