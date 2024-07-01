using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVertex : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 offset;
    private Vector3 originalPosition;
    private List<int> indices;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.localPosition;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseUp()
    {
        isDragging = false;
        transform.localPosition = originalPosition; // Reset to the original position for consistent grouping
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPos() + offset;
            transform.position = newPosition;
            SendMessageUpwards("MoveVerticesToPosition", new object[] { indices, newPosition }, SendMessageOptions.DontRequireReceiver);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public void SetIndices(List<int> indices)
    {
        this.indices = indices;
    }

    public bool IndicesOverlap(HashSet<int> otherIndices)
    {
        foreach (int index in indices)
        {
            if (otherIndices.Contains(index))
            {
                return true;
            }
        }
        return false;
    }

    public int GetFirstIndex()
    {
        if (indices != null && indices.Count > 0)
        {
            return indices[0];
        }
        return -1;
    }
}
