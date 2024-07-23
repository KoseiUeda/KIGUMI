using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f;  // 初期移動ステップ
    public float minY = 1.0f;              // 最小Y座標（移動停止位置）
    public float decreaseFactor = 0.94f;   // 移動ステップ減少係数
    private float currentMoveStep;         // 現在の移動ステップ
    private bool canMove = true;           // 移動可能フラグ
    private float cooldown = 0.5f;         // 冷却時間
    public SoundManager soundManager;      // SoundManagerへの参照
    public AudioManager audioManager;      // AudioManagerへの参照
    public int carvingCount = 0;           // 削り回数をカウント
    public float carvingDecreaseFactor = 0.98f; // 削りによる減少係数の変化
    private float carvingImpact = 0.002f;  // 削りの影響量
    private bool isInserted = false;       // 挿入が完了したかどうかを示すフラグ
    public GameObject menta;               // Mentaオブジェクトへの参照
    private float initialY;                // 初期Y座標を保持する変数

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start時に初期移動ステップを設定
        initialY = transform.position.y;    // 初期Y座標を設定
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)  // ハンマーがオブジェクトに触れたかどうか
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // 移動前のステップをログに出力
            if (transform.position.y - currentMoveStep > minY)
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor;  // 移動ステップを減少
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");  // 移動後のステップをログに出力

                soundManager.PlaySound(currentMoveStep);  // SoundManagerを通じて音を再生

                canMove = false;  // 移動フラグをfalseに設定
                Invoke("ResetMovement", cooldown);  // 冷却時間後に移動フラグをリセット
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);  // Y座標が最小値に達した場合

                soundManager.PlaySound(currentMoveStep);  // SoundManagerを通じて最後の音を再生

                isInserted = CheckInsertion();  // 挿入チェック
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;  // 移動フラグをリセット
    }

    // 面を削る処理を追加
    public void CarveFace(float carvingDepth)
    {
        carvingCount++;  // 削り回数をカウント
        initialMoveStep += carvingImpact;  // 削りの影響量に応じて初期移動距離を増加
        decreaseFactor *= carvingDecreaseFactor;  // 削るほど減少係数も調整

        // currentMoveStep を更新
        currentMoveStep = initialMoveStep;

        // AudioManagerを使って削る音を再生
        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount);
        }
    }

    bool CheckInsertion()
    {
        // Mentaの位置とサイズを取得
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Ontaの位置とサイズを取得
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Ontaの底面がMentaの上面に収まっているかチェック
        return ontaBounds.min.y <= mentaBounds.max.y;
    }
}
