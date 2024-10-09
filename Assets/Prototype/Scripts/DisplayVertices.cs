using UnityEngine;
using System.Collections.Generic;

public class DisplayVertices : MonoBehaviour
{
    public GameObject vertexPrefab; // 頂点を表示するためのプレハブ（小さなスフィアなど）
    private Mesh mesh;
    private Vector3[] vertices;
    public Dictionary<int, List<int>> vertexGroups;
    private List<GameObject> vertexMarkers;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh originalMesh = meshFilter.sharedMesh;

        // メッシュのコピーを作成し、それを変更可能にする
        mesh = Instantiate(originalMesh);
        meshFilter.mesh = mesh;

        vertices = mesh.vertices;
        vertexGroups = new Dictionary<int, List<int>>();
        vertexMarkers = new List<GameObject>();

        // 頂点をグループ化（インデックスベース）
        for (int i = 0; i < vertices.Length; i++)
        {
            int hashCode = GetPositionHashCode(vertices[i]);
            if (!vertexGroups.ContainsKey(hashCode))
            {
                vertexGroups[hashCode] = new List<int>();
            }
            vertexGroups[hashCode].Add(i);
        }

        // 各グループの位置にスフィアを生成し、MoveVertexスクリプトをアタッチ
        foreach (var group in vertexGroups)
        {
            int index = group.Value[0]; // グループの最初のインデックスを使用
            Vector3 worldPosition = transform.TransformPoint(vertices[index]);
            GameObject vertexMarker = Instantiate(vertexPrefab, worldPosition, Quaternion.identity, transform);

            MoveVertex moveVertexScript = vertexMarker.AddComponent<MoveVertex>();
            moveVertexScript.SetIndices(group.Value);

            vertexMarker.name = $"Vertex Group {group.Key}";
            vertexMarkers.Add(vertexMarker);
        }
    }

    int GetPositionHashCode(Vector3 position)
    {
        return (Mathf.RoundToInt(position.x * 1000f) * 1000000 +
                Mathf.RoundToInt(position.y * 1000f) * 1000 +
                Mathf.RoundToInt(position.z * 1000f)).GetHashCode();
    }

    public void UpdateVertexPositions(HashSet<int> vertexIndices, Vector3 moveOffset)
    {
        foreach (int index in vertexIndices)
        {
            vertices[index] += moveOffset;
        }

        // メッシュを更新
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // マーカーの位置を更新
        foreach (var vertexMarker in vertexMarkers)
        {
            MoveVertex moveVertexScript = vertexMarker.GetComponent<MoveVertex>();
            if (moveVertexScript != null && moveVertexScript.IndicesOverlap(vertexIndices))
            {
                vertexMarker.transform.position = transform.TransformPoint(vertices[moveVertexScript.GetFirstIndex()]);
            }
        }
    }
}
