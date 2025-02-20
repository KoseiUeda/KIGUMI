using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CanvasFollowChisel : MonoBehaviour
{
    public Transform chiselObject; // Chisel（鑿）のオブジェクト
    public Vector3 offset = new Vector3(0.3f, 0, 0); // 離した後のオフセット位置
    public bool facePlayer = true; // プレイヤーの方向を向くかどうか

    private Vector3 initialPosition; // 初期位置を保持
    private bool hasBeenGrabbed = false; // 掴まれたかどうかの判定

    void Start()
    {
        // 最初の `Canvas` の位置を保存
        initialPosition = transform.position;
    }

    void Update()
    {
        if (chiselObject != null)
        {
            if (hasBeenGrabbed)
            {
                // ChiselObject の横に Canvas を移動（ワールド座標）
                transform.position = chiselObject.position + chiselObject.right * offset.x
                                                       + chiselObject.up * offset.y
                                                       + chiselObject.forward * offset.z;
            }

            // プレイヤーの方向を向かせる（必要に応じてオフ）
            if (facePlayer && Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0); // 画像が反転しないように
            }
        }
    }

    // 掴まれたときの処理
    public void OnGrab()
    {
        hasBeenGrabbed = true;
    }

    // 離したときの処理
    public void OnRelease()
    {
        hasBeenGrabbed = true; // 掴んだ後は、常に Offset の位置へ
    }
}
