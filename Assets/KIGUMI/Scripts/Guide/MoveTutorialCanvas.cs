using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MoveTutorialCanvas : MonoBehaviour
{
    public Transform leftController; // 左コントローラー（追従対象）
    public Vector3 offset = new Vector3(0.1f, 0.1f, 0.2f); // 左手からの相対位置
    public InputActionReference leftStickInput; // 左スティックの入力
    public float stickThreshold = 0.3f; // スティックがこれ以上倒されたらカウントダウン開始
    public float displayDuration = 3.0f; // 傾けてから消えるまでの秒数

    private bool isDisplayed = true; // Canvas が表示中かどうか
    private bool hasTilted = false; // すでに傾けたかどうかのフラグ

    void Update()
    {
        if (leftController != null && isDisplayed)
        {
            // 左コントローラーの位置＋オフセットに Canvas を追従
            transform.position = leftController.position + leftController.right * offset.x
                                                      + leftController.up * offset.y
                                                      + leftController.forward * offset.z;

            // プレイヤーの方向を向かせる
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0); // 画像が反転しないように
            }
        }

        // 左スティックの入力を取得
        if (!hasTilted && leftStickInput != null && leftStickInput.action != null)
        {
            Vector2 stickInput = leftStickInput.action.ReadValue<Vector2>();

            // スティックが一定以上倒されたら、Canvas を傾けてからカウントダウン開始
            if (stickInput.magnitude > stickThreshold)
            {
                TiltCanvas(); // 🎯 Canvas を傾ける
                StartCoroutine(HideAfterSeconds()); // 🎯 指定秒数後に非表示
            }
        }
    }

    // 🎯 Canvas を傾ける処理
    void TiltCanvas()
    {
        hasTilted = true;
        transform.Rotate(15f, 0f, 45f); // X 軸 15°、Z 軸 45° 傾ける
    }

    // 🎯 指定秒数後に非表示にするコルーチン
    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(displayDuration);
        HideCanvas();
    }

    // 🎯 Canvas を非表示にする処理
    void HideCanvas()
    {
        isDisplayed = false;
        gameObject.SetActive(false);
    }
}
