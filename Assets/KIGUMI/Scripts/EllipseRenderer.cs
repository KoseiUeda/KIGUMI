using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    public Transform targetTransform; // HammeringターゲットのTransform
    public LayerMask layerMask; // レイの判定に使用するLayerMask
    public float rayLength = 10.0f; // レイの長さ

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRendererの設定
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2; // 始点と終点
        lineRenderer.useWorldSpace = true; // ワールド座標を使用
        lineRenderer.enabled = false; // 初期は非表示
    }

    void Update()
    {
        if (IsRayHittingTarget(out Vector3 hitPoint))
        {
            // レイを表示して始点と終点を設定
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position); // 手の位置
            lineRenderer.SetPosition(1, hitPoint); // ヒットした位置
        }
        else
        {
            // レイを非表示
            lineRenderer.enabled = false;
        }
    }

    bool IsRayHittingTarget(out Vector3 hitPoint)
    {
        hitPoint = Vector3.zero;
        Ray ray = new Ray(transform.position, transform.forward); // 手の向きにレイを飛ばす
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
        {
            if (hit.transform == targetTransform)
            {
                hitPoint = hit.point;
                return true;
            }
        }
        return false;
    }
}
