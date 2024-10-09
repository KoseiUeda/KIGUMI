using UnityEngine;

public class ChiselCollisionLogger : MonoBehaviour
{
    private Mesh mesh;  // オブジェクトのメッシュ
    private MeshCollider meshCollider;  // メッシュコライダー
    private Vector3 hitPoint;  // 衝突した点のワールド座標

    void Start()
    {
        // メッシュとメッシュコライダーを取得
        meshCollider = GetComponent<MeshCollider>();
        mesh = GetComponent<MeshFilter>().mesh;

        if (mesh == null)
        {
            Debug.LogError("Mesh not found on the object.");
        }

        if (meshCollider == null)
        {
            Debug.LogError("MeshCollider not found on the object.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Chisel")
        {
            Debug.Log("Chisel collided!");

            // 衝突点の情報を取得
            foreach (ContactPoint contact in collision.contacts)
            {
                hitPoint = contact.point;

                // 衝突した面番号を取得
                int triangleIndex = GetHitTriangleIndex(contact);

                // コンソールに面番号を表示
                if (triangleIndex >= 0)
                {
                    Debug.Log("Hit triangle index: " + triangleIndex);
                }
                else
                {
                    Debug.Log("Triangle index not found.");
                }
            }
        }
    }

    int GetHitTriangleIndex(ContactPoint contact)
    {
        // ローカル空間に変換した衝突点の座標を取得
        Vector3 localPoint = transform.InverseTransformPoint(contact.point);

        // 衝突したメッシュの三角形インデックスを取得
        int closestTriangleIndex = FindClosestTriangle(localPoint);

        return closestTriangleIndex;
    }

    int FindClosestTriangle(Vector3 point)
    {
        int[] triangles = mesh.triangles;  // メッシュの三角形インデックス
        Vector3[] vertices = mesh.vertices;  // メッシュの頂点

        float minDistance = float.MaxValue;
        int closestTriangle = -1;

        // 各三角形をチェックして、最も近いものを見つける
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p0 = vertices[triangles[i]];
            Vector3 p1 = vertices[triangles[i + 1]];
            Vector3 p2 = vertices[triangles[i + 2]];

            // 三角形の中心点を計算
            Vector3 triangleCenter = (p0 + p1 + p2) / 3;

            // 衝突点との距離を計算
            float distance = Vector3.Distance(triangleCenter, point);

            // 最も近い三角形を保存
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTriangle = i / 3;  // 三角形のインデックスを計算
            }
        }

        return closestTriangle;
    }
}
