using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

[System.Serializable]
public class FacePair
{
    public int[] touchIndices = new int[1]; // 触れる面のインデックス
    public int[] highlightIndices = new int[6]; // ハイライトする面のインデックス
    public Material highlightMaterial; // ペアに対して適用するマテリアル
    public bool moveX = false; // X方向に移動するかどうかのチェックボックス
    public bool moveY = false; // Y方向に移動するかどうかのチェックボックス
    public bool moveZ = false; // Z方向に移動するかどうかのチェックボックス
    public int moveCount = 0; // 加工回数を記録する
    public int carvingCount = 0; // 削り回数を記録する
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

    private OntaBehavior ontaBehavior; // OntaBehaviorの参照
    private HashSet<int> activePairHashes = new HashSet<int>(); // アクティブなペアのハッシュを保持
    private int currentPairHash = -1; // 現在ハイライトされているペアのハッシュ
    private List<int> currentTriangleIndices = new List<int>(); // 現在ハイライトされている三角形のインデックス
    private bool wasTriggerPressed = false; // 前のフレームでトリガーボタンが押されていたかどうか

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        displayVertices = GetComponent<DisplayVertices>(); // DisplayVerticesスクリプトの取得
        ontaBehavior = GetComponent<OntaBehavior>(); // OntaBehaviorスクリプトの取得

        if (ontaBehavior == null)
        {
            Debug.LogError("OntaBehavior component not found on the same GameObject.");
        }

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
            UnityEngine.Mesh highlightMesh = new UnityEngine.Mesh();
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

        // トリガーボタンが押された瞬間を検出
        if (triggerPressed && !wasTriggerPressed && currentPairHash != -1)
        {
            Vector3 moveDirection = CalculateMoveDirection(currentPairHash, currentTriangleIndices[0]);
            MoveVerticesAndAdjacentFaces(currentTriangleIndices, moveDirection);
            UpdateColliderMesh(); // コライダーメッシュを更新

            // FacePair の moveCount と carvingCount を更新
            var pair = facePairs.Find(p => p.GetHashCode() == currentPairHash);
            if (pair != null)
            {
                pair.moveCount++;
                pair.carvingCount++;
                Debug.Log($"Moved vertices of pair with hash {currentPairHash} by {moveDirection}");

                // 面の削る処理を呼び出し、削る深さを指定
                if (ontaBehavior != null)
                {
                    float carvingDepth = 0.005f; // 例として0.005の深さで削る
                    ontaBehavior.CarveFace(carvingDepth);
                }
            }
        }

        wasTriggerPressed = triggerPressed; // 現在のトリガーボタンの状態を保存
    }

    // 省略された他のメソッド...
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chisel")) // 任意のオブジェクトにタグを設定して検出
        {
            MeshFilter otherMeshFilter = other.GetComponent<MeshFilter>();
            if (otherMeshFilter != null)
            {
                UnityEngine.Mesh mesh = otherMeshFilter.sharedMesh;
                if (mesh != null && mesh.isReadable) // メッシュが読み取り可能であることを確認
                {
                    Vector3 localPoint = other.transform.InverseTransformPoint(transform.position);
                    int triangleIndex = FindClosestTriangle(mesh, localPoint);
                    Debug.Log($"Touched Triangle Index: {triangleIndex}");

                    // 触れた面に対応するハイライト処理を呼び出す
                    var highlightIndicesAndPairHash = GetHighlightFaceIndicesAndPairHash(triangleIndex);
                    if (highlightIndicesAndPairHash.Item1.Count > 0)
                    {
                        HighlightFaces(highlightIndicesAndPairHash.Item1, highlightIndicesAndPairHash.Item2, meshFilter.mesh);
                        activePairHashes.Add(highlightIndicesAndPairHash.Item2); // アクティブなペアのハッシュを追加
                        currentPairHash = highlightIndicesAndPairHash.Item2;
                        currentTriangleIndices = highlightIndicesAndPairHash.Item1;
                    }
                    else
                    {
                        Debug.LogWarning($"No highlight indices found for triangle index {triangleIndex}");
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
        if (other.CompareTag("Chisel"))
        {
            ResetHighlight();
            activePairHashes.Clear(); // アクティブなペアのハッシュをクリア
            currentPairHash = -1;
            currentTriangleIndices.Clear();
        }
    }

    int FindClosestTriangle(UnityEngine.Mesh mesh, Vector3 point)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        float minDistance = float.MaxValue;
        int closestTriangle = -1;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p0 = vertices[triangles[i]];
            Vector3 p1 = vertices[triangles[i + 1]];
            Vector3 p2 = vertices[triangles[i + 2]];

            Vector3 center = (p0 + p1 + p2) / 3;
            float distance = Vector3.Distance(center, point);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTriangle = i / 3;
            }
        }

        return closestTriangle;
    }

    (List<int>, int) GetHighlightFaceIndicesAndPairHash(int triangleIndex)
    {
        foreach (FacePair pair in facePairs)
        {
            foreach (int touchIndex in pair.touchIndices)
            {
                if (touchIndex == triangleIndex)
                {
                    Debug.Log($"Found matching face index {triangleIndex} in pair with hash {pair.GetHashCode()}");
                    return (new List<int>(pair.highlightIndices), pair.GetHashCode());
                }
            }
        }
        return (new List<int>(), -1);
    }

    void HighlightFaces(List<int> triangleIndices, int pairHash, UnityEngine.Mesh mesh)
    {
        if (!highlightObjects.ContainsKey(pairHash))
        {
            Debug.LogWarning($"Pair hash {pairHash} not found in highlightObjects");
            return;
        }

        GameObject highlightObject = highlightObjects[pairHash];
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        List<Vector3> quadVertices = new List<Vector3>();

        foreach (int triangleIndex in triangleIndices)
        {
            if (triangleIndex * 3 + 2 >= triangles.Length) // 配列の範囲外にアクセスしないように確認
            {
                Debug.LogError($"Triangle index {triangleIndex} is out of bounds for the triangles array.");
                continue;
            }

            int index0 = triangles[triangleIndex * 3];
            int index1 = triangles[triangleIndex * 3 + 1];
            int index2 = triangles[triangleIndex * 3 + 2];

            quadVertices.Add(vertices[index0] + normals[index0] * highlightOffset);
            quadVertices.Add(vertices[index1] + normals[index1] * highlightOffset);
            quadVertices.Add(vertices[index2] + normals[index2] * highlightOffset);
        }

        UnityEngine.Mesh highlightMesh = highlightObject.GetComponent<MeshFilter>().mesh;
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
        Debug.Log($"Highlight applied to object with pair hash {pairHash} on triangle indices: {string.Join(", ", triangleIndices)}");
    }

    void MoveVerticesAndAdjacentFaces(List<int> triangleIndices, Vector3 moveDirection)
    {
        UnityEngine.Mesh mesh = meshFilter.mesh;
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
        HighlightFaces(currentTriangleIndices, currentPairHash, mesh);
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