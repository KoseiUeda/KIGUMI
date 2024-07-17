using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f;  // 初期移動ステップ
    public float minY = 1.0f;             // 最小Y座標（移動停止位置）
    public float decreaseFactor = 0.94f;  // 移動ステップ減少係数
    private float currentMoveStep;        // 現在の移動ステップ
    private bool canMove = true;          // 移動可能フラグ
    private float cooldown = 0.5f;        // 冷却時間
    public SoundManager soundManager;     // SoundManagerへの参照

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start時に初期移動ステップを設定
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove)  // ハンマーがオブジェクトに触れたかどうか
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // 移動前のステップをログに出力
            if (transform.position.y - currentMoveStep > minY)
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);  // Y軸に沿って移動
                currentMoveStep *= decreaseFactor;  // 移動ステップを減少
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");  // 移動後のステップをログに出力

                soundManager.PlaySound(currentMoveStep);  // SoundManagerを通じて音を再生

                canMove = false;  // 移動フラグをfalseに設定
                Invoke("ResetMovement", cooldown);  // 冷却時間後に移動フラグをリセット
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);  // Y座標が最小値に達した場合
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;  // 移動フラグをリセット
    }
}
