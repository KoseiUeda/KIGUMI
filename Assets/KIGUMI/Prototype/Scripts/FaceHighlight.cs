using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FacePair
{
    public int[] faceIndices = new int[6]; // 6つのFace Index
    public Material highlightMaterial; // ペアに対して適用するマテリアル
    public bool moveX = false; // X方向に移動するかどうかのチェックボックス
    public bool moveY = false; // Y方向に移動するかどうかのチェックボックス
    public bool moveZ = false; // Z方向に移動するかどうかのチェックボックス
    public int moveCount = 0; // 加工回数を記録する
}

public class FaceHighlight : MonoBehaviour
{
    public List<FacePair> facePairs = new List<FacePair>(); // インスペクタで設定するための面のペアのリスト
    private MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.CompareTag("HighlightObject")) // 任意のオブジェクトにタグを設定して検出
        {
            Debug.Log("HighlightObject detected: " + other.gameObject.name);
            MeshFilter otherMeshFilter = other.GetComponent<MeshFilter>();
            if (otherMeshFilter != null)
            {
                Mesh mesh = otherMeshFilter.sharedMesh;
                if (mesh != null && mesh.isReadable) // メッシュが読み取り可能であることを確認
                {
                    Debug.Log("Mesh is readable: " + mesh.name);
                    int[] triangles = mesh.triangles;
                    Vector3[] vertices = mesh.vertices;
                    Vector3[] normals = mesh.normals;

                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        Vector3 p0 = vertices[triangles[i]];
                        Vector3 p1 = vertices[triangles[i + 1]];
                        Vector3 p2 = vertices[triangles[i + 2]];

                        Vector3 normal = (normals[triangles[i]] + normals[triangles[i + 1]] + normals[triangles[i + 2]]).normalized;
                        Vector3 center = (p0 + p1 + p2) / 3;

                        Vector3 direction = transform.InverseTransformPoint(center) - other.transform.position;
                        if (Vector3.Dot(normal, direction) > 0)
                        {
                            int triangleIndex = i / 3;
                            Debug.Log($"Touched Triangle Index: {triangleIndex}");
                            break; // 最初のヒットした面をログに表示してループを抜ける
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Mesh is not readable. Please enable Read/Write in the import settings for mesh: " + otherMeshFilter.sharedMesh.name);
                }
            }
        }
    }

    // GetTotalMoveCount メソッドを追加
    public int GetTotalMoveCount()
    {
        int totalMoveCount = 0;
        foreach (var pair in facePairs)
        {
            totalMoveCount += pair.moveCount;
        }
        return totalMoveCount;
    }
}
