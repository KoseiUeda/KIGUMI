using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VRResetManager : MonoBehaviour
{
    [Header("Reset Buttons (Input System)")]
    [Tooltip("左コントローラーの X ボタン にバインドした Input Action")]
    public InputActionProperty leftXAction;
    [Tooltip("右コントローラーの A ボタン にバインドした Input Action")]
    public InputActionProperty rightAAction;

    // 前フレームの状態を保持して「押された瞬間」を検出
    private bool prevXPressed = false;
    private bool prevAPressed = false;

    void Awake()
    {
        // アクションを有効化
        leftXAction.action.Enable();
        rightAAction.action.Enable();
    }

    void Update()
    {
        // 現在の押下状態を読む
        bool xPressed = leftXAction.action.ReadValue<float>() > 0.5f;
        bool aPressed = rightAAction.action.ReadValue<float>() > 0.5f;

        // 両方が同時に押され、かつ前フレームは同時押しでなかったらリセット
        if (xPressed && aPressed && !(prevXPressed && prevAPressed))
        {
            PerformReset();
        }

        prevXPressed = xPressed;
        prevAPressed = aPressed;
    }

    private void PerformReset()
    {
        Debug.Log("VRResetManager: リセットトリガー検出。シーンを再読み込みします。");

        // ■ 方法1：シーン再読み込みによる完全リセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // ■ 方法2：個別にリセットしたい場合はコメントアウトし、
        //    以下のように各オブジェクトのリセットメソッドを呼び出してください。
        //
        // foreach (var ob in FindObjectsOfType<OntaBehavior>())
        //     ob.ResetOnta();
        // // 他にリセットしたいコンポーネントがあれば同様に呼び出し
    }
}
