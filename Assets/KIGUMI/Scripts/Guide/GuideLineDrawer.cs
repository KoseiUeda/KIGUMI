using UnityEngine;

public class GuideLineDrawer : MonoBehaviour
{
    public Transform startPoint; // ガイドが出るオブジェクト（始点）
    public Transform endPoint;   // Canvasの位置（終点）
    public float trimLength = 0.2f; // 両端を削る距離

    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRendererをアタッチまたは取得
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.0007f; // 線の太さ
        lineRenderer.endWidth = 0.0007f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 白色マテリアル
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        // 初期状態で線を非表示
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            // ベクトル計算で両端を削る
            Vector3 direction = (endPoint.position - startPoint.position).normalized; // 開始→終了の方向
            Vector3 newStart = startPoint.position + direction * trimLength; // 始点をtrimLength分前にずらす
            Vector3 newEnd = endPoint.position - direction * trimLength; // 終点をtrimLength分後ろにずらす

            // 線の始点と終点を更新
            lineRenderer.SetPosition(0, newStart);
            lineRenderer.SetPosition(1, newEnd);

            // GuideLineDrawerが有効になっているときだけ線を表示
            lineRenderer.enabled = true;
        }
    }

    void OnDisable()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}
