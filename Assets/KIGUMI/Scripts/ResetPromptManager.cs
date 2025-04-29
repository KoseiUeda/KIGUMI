using UnityEngine;

public class ResetPromptManager : MonoBehaviour
{
    [Header("参照する OntaBehavior")]
    [Tooltip("チゼリング用 OntaObject の OntaBehavior")]
    public OntaBehavior ontaBehavior;

    [Header("表示条件のパラメータ")]
    [Tooltip("Y がこれ以下で OK")]
    public float minY = 0.5f;
    [Tooltip("MoveStep がこれ以下でほぼ 0 判定")]
    public float moveStepEpsilon = 0.0001f;
    [Tooltip("X 座標がほぼこの値のとき")]
    public float targetX = -0.53f;
    [Tooltip("Z 座標がほぼこの値のとき")]
    public float targetZ = 0.855f;
    [Tooltip("位置判定用の許容誤差")]
    public float positionEpsilon = 0.01f;

    [Header("Canvas 表示設定")]
    [Tooltip("World Space に設定した Canvas")]
    public Canvas resetPromptCanvas;
    [Tooltip("キャンバスを出すコントローラの Transform")]
    public Transform controllerTransform;
    [Tooltip("コントローラ前方への距離 (m)")]
    public float forwardOffset = 0.2f;
    [Tooltip("追加で右上方向にオフセット(ワールド座標系)")]
    public Vector3 additionalOffset = new Vector3(0.1f, 0.1f, 0);

    bool hasShown = false;

    void Start()
    {
        if (resetPromptCanvas != null)
            resetPromptCanvas.gameObject.SetActive(false);
        else
            Debug.LogError("ResetPromptManager: resetPromptCanvas が未設定です");

        if (ontaBehavior == null)
            Debug.LogError("ResetPromptManager: ontaBehavior が未設定です");
        if (controllerTransform == null)
            Debug.LogError("ResetPromptManager: controllerTransform が未設定です");
    }

    void Update()
    {
        if (hasShown || ontaBehavior == null) return;

        bool zeroStep = ontaBehavior.CurrentMoveStep <= moveStepEpsilon;
        bool atMinY = ontaBehavior.transform.localPosition.y <= minY;
        bool matchX = Mathf.Abs(ontaBehavior.transform.localPosition.x - targetX) <= positionEpsilon;
        bool matchZ = Mathf.Abs(ontaBehavior.transform.localPosition.z - targetZ) <= positionEpsilon;

        if (zeroStep && atMinY && matchX && matchZ)
        {
            ShowResetPrompt();
            hasShown = true;
        }
    }

    void ShowResetPrompt()
    {
        if (resetPromptCanvas.gameObject.activeSelf) return;

        resetPromptCanvas.gameObject.SetActive(true);

        // ベース位置：コントローラ前方
        Vector3 fwd = controllerTransform.forward;
        Vector3 basePos = controllerTransform.position + fwd * forwardOffset;

        // 追加オフセットを加える
        resetPromptCanvas.transform.position = basePos + additionalOffset;
        resetPromptCanvas.transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
    }

    /// <summary>
    /// Canvas 上の「リセット」ボタンにアサイン
    /// </summary>
    public void OnResetButtonPressed()
    {
        ontaBehavior.ResetOnta();
        resetPromptCanvas.gameObject.SetActive(false);
        hasShown = false;
    }
}
