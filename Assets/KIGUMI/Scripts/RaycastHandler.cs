using UnityEngine;
using UnityEngine.XR;

public class RaycastHandler : MonoBehaviour
{
    public LineRenderer lineRenderer; // レイを表示するためのラインレンダラー
    public float rayLength = 10f; // レイの長さ
    public LayerMask raycastLayerMask; // レイキャストの対象レイヤー

    private bool isHandEmpty = true; // 手が空かどうかのフラグ

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned. Please assign a LineRenderer component.");
        }

        lineRenderer.enabled = false; // 初期状態ではレイを表示しない
    }

    void Update()
    {
        // 手が空の場合のみレイを表示
        if (isHandEmpty)
        {
            // レイキャストの方向と位置を設定
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;

            // レイキャストを発射してターゲットを調べる
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, raycastLayerMask))
            {
                // レイを表示し、終点を設定
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, rayOrigin);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                // レイがターゲットにヒットしなかった場合
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, rayOrigin);
                lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);
            }
        }
        else
        {
            // 手に何かを持っている場合はレイを非表示にする
            lineRenderer.enabled = false;
        }
    }

    // 手が空であるかどうかを設定するメソッド
    public void SetHandEmpty(bool isEmpty)
    {
        isHandEmpty = isEmpty;
    }
}
