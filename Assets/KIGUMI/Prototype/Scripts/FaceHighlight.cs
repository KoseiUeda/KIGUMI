using UnityEngine;
using UnityEngine.XR;
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
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private DisplayVertices displayVertices; // DisplayVerticesスクリプトへの参照
    private Dictionary<int, GameObject> highlightObjects = new Dictionary<int, GameObject>(); // 各ペアごとに異なるハイライトオブジェクトを保持
    public float highlightOffset = 0.01f; // ハイライトを少し上に移動するオフセット
    public float moveDistance = 0.1f; // 頂点を移動させる距離

    private bool isHighlighted = false; // ハイライトされているかどうかを追跡する
    private int currentPairHash = -1; // 現在ハイライトされているペアのハッシュ
    private List<int> currentTriangleIndices = new List<int>(); // 現在ハイライトされている三角形のインデックス

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        displayVertices = GetComponent<DisplayVertices>(); // DisplayVerticesスクリプトの取得

        // 初期ハイライトオブジェクトをペアごとに作成してディクショナリに追加
        foreach (var pair in facePairs)
        {
            GameObject highlightObject = new GameObject("Highlight");
            highlightObject.transform.SetParent(transform);
            highlightObject.transform.localPosition = Vector3.zero;
            highlightObject.transform.localRotation = Quaternion.identity;
            highlightObject.transform.localScale = Vector3.one;

            MeshRenderer highlightMeshRenderer = highlightObject.AddComponent<MeshRenderer>();
            highlightMeshRenderer.material = pair.highlightMaterial;

            MeshFilter highlightMeshFilter = highlightObject.AddComponent<MeshFilter>();
            Mesh highlightMesh = new Mesh();
            highlightMeshFilter.mesh = highlightMesh;

            highlightObject.SetActive(false);
            highlightObjects.Add(pair.GetHashCode(), highlightObject);
        }
    }

    void Update()
    {
        // VRコントローラーの入力を取得
        bool triggerPressed = false;
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithRole(InputDeviceRole.RightHanded, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value) && value)
            {
                triggerPressed = true;
                break;
            }
        }

        if (isHighlighted && triggerPressed)
        {
            Vector3 moveDirection = CalculateMoveDirection(currentPairHash, currentTriangleIndices[0]);
            MoveVerticesAndAdjacentFaces(currentTriangleIndices, moveDirection);
            UpdateColliderMesh(); // コライダーメッシュを更新

            // FacePair の moveCount を更新
            var pair = facePairs.Find(p => p.GetHashCode() == currentPairHash);
            if (pair != null)
            {
                pair.moveCount++;
                Debug.Log($"Moved vertices of pair with hash {currentPairHash} by {moveDirection}");
            }
        }
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
                            var highlightIndicesAndPairHash = GetHighlightFaceIndicesAndPairHash(triangleIndex);
                            if (highlightIndicesAndPairHash.Item1.Count > 0)
                            {
                                int pairIndex = facePairs.FindIndex(p => p.GetHashCode() == highlightIndicesAndPairHash.Item2);
                                Debug.Log($"Touched Element: {pairIndex}");
                                HighlightFaces(highlightIndicesAndPairHash.Item1, highlightIndicesAndPairHash.Item2);
                                isHighlighted = true;
                                currentPairHash = highlightIndicesAndPairHash.Item2;
                                currentTriangleIndices = highlightIndicesAndPairHash.Item1;
                                break; // ハイライトするオブジェクトが見つかったらループを抜ける
                            }
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

    void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called with: " + other.gameObject.name);
        if (other.CompareTag("HighlightObject"))
        {
            Debug.Log("HighlightObject exited: " + other.gameObject.name);
            ResetHighlight(); // オブジェクトから離れたらハイライトをリセット
            isHighlighted = false;
            currentPairHash = -1;
            currentTriangleIndices.Clear();
        }
    }

    (List<int>, int) GetHighlightFaceIndicesAndPairHash(int triangleIndex)
    {
        foreach (FacePair pair in facePairs)
        {
            foreach (int faceIndex in pair.faceIndices)
            {
                if (faceIndex == triangleIndex)
                {
                    Debug.Log($"Found matching face index {faceIndex} in pair with hash {pair.GetHashCode()}");
                    return (new List<int>(pair.faceIndices), pair.GetHashCode());
                }
            }
        }
        return (new List<int>(), -1);
    }

    void HighlightFaces(List<int> triangleIndices, int pairHash)
    {
        if (!highlightObjects.ContainsKey(pairHash))
        {
            Debug.LogWarning($"Pair hash {pairHash} not found in highlightObjects");
            return;
        }

        GameObject highlightObject = highlightObjects[pairHash];
        Mesh mesh = meshFilter.mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        List<Vector3> quadVertices = new List<Vector3>();

        foreach (int triangleIndex in triangleIndices)
        {
            int index0 = triangles[triangleIndex * 3];
            int index1 = triangles[triangleIndex * 3 + 1];
            int index2 = triangles[triangleIndex * 3 + 2];

            quadVertices.Add(vertices[index0] + normals[index0] * highlightOffset);
            quadVertices.Add(vertices[index1] + normals[index1] * highlightOffset);
            quadVertices.Add(vertices[index2] + normals[index2] * highlightOffset);
        }

        Mesh highlightMesh = highlightObject.GetComponent<MeshFilter>().mesh;
        highlightMesh.Clear();
        highlightMesh.vertices = quadVertices.ToArray();

        List<int> highlightTriangles = new List<int>();
        for (int i = 0; i < quadVertices.Count; i += 3)
        {
            highlightTriangles.Add(i);
            highlightTriangles.Add(i + 1);
            highlightTriangles.Add(i + 2);
        }
        highlightMesh.triangles = highlightTriangles.ToArray();
        highlightMesh.RecalculateNormals();

        highlightObject.SetActive(true);
        Debug.Log($"Highlight applied to object with pair hash {pairHash}");
    }

    void MoveVerticesAndAdjacentFaces(List<int> triangleIndices, Vector3 moveDirection)
    {
        Mesh mesh = meshFilter.mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        HashSet<int> vertexIndices = new HashSet<int>();

        // 選択された三角形の頂点インデックスを取得
        foreach (int triangleIndex in triangleIndices)
        {
            vertexIndices.Add(triangles[triangleIndex * 3]);
            vertexIndices.Add(triangles[triangleIndex * 3 + 1]);
            vertexIndices.Add(triangles[triangleIndex * 3 + 2]);
        }

        // 重なっている頂点を取得
        List<int> overlappingVertexIndices = new List<int>();
        foreach (var index in vertexIndices)
        {
            foreach (var group in displayVertices.vertexGroups)
            {
                if (group.Value.Contains(index))
                {
                    overlappingVertexIndices.AddRange(group.Value);
                }
            }
        }

        Vector3 moveOffset = moveDirection.normalized * moveDistance;

        foreach (int vertexIndex in overlappingVertexIndices)
        {
            vertices[vertexIndex] += moveOffset;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;

        if (displayVertices != null)
        {
            displayVertices.UpdateVertexPositions(new HashSet<int>(overlappingVertexIndices), moveOffset);
        }

        // ハイライトの位置を更新
        HighlightFaces(currentTriangleIndices, currentPairHash);
    }

    Vector3 CalculateMoveDirection(int pairHash, int selectedFaceIndex)
    {
        Vector3[] normals = meshFilter.mesh.normals;
        int[] triangles = meshFilter.mesh.triangles;

        foreach (FacePair pair in facePairs)
        {
            if (pair.GetHashCode() == pairHash)
            {
                Vector3 moveDirection = Vector3.zero;
                Vector3 faceNormal = (normals[triangles[selectedFaceIndex * 3]] + normals[triangles[selectedFaceIndex * 3 + 1]] + normals[triangles[selectedFaceIndex * 3 + 2]]).normalized;

                if (pair.moveX)
                {
                    moveDirection.x = faceNormal.x > 0 ? -moveDistance : moveDistance;
                }
                if (pair.moveY)
                {
                    moveDirection.y = faceNormal.y > 0 ? -moveDistance : moveDistance;
                }
                if (pair.moveZ)
                {
                    moveDirection.z = faceNormal.z > 0 ? -moveDistance : moveDistance;
                }
                Debug.Log($"Calculated move direction for pair with hash {pairHash}: {moveDirection}");
                return moveDirection;
            }
        }
        return Vector3.zero;
    }

    void ResetHighlight()
    {
        foreach (var highlightObject in highlightObjects.Values)
        {
            if (highlightObject.activeSelf)
            {
                highlightObject.SetActive(false);
            }
        }
        Debug.Log("Highlight reset");
    }

    void UpdateColliderMesh()
    {
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshFilter.mesh;
        Debug.Log("Collider mesh updated");
    }

    public FacePair GetFacePairByIndex(int index)
    {
        foreach (var pair in facePairs)
        {
            foreach (var faceIndex in pair.faceIndices)
            {
                if (faceIndex == index)
                {
                    return pair;
                }
            }
        }
        return null;
    }

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
