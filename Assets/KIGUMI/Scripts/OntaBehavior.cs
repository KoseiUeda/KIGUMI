using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f; // 初期の移動ステップ
    public float minY = 1.0f; // ローカル座標での最小Y位置
    public float decreaseFactor = 0.94f; // 移動ステップの減少係数
    private float currentMoveStep; // 現在の移動ステップ
    private bool canMove = true; // 移動可能かどうか
    private float cooldown = 0.5f; // クールダウン時間
    public SoundManager soundManager; // サウンドマネージャー
    public AudioManager audioManager; // オーディオマネージャー
    public int carvingCount = 0; // 削り回数
    public float carvingDecreaseFactor = 0.98f; // 削る際の減少係数
    private float carvingImpact = 0.002f; // 削る際の影響
    private bool isInserted = false; // 挿入されたかどうか
    public GameObject menta; // メンタのオブジェクト
    private float initialY; // 初期のローカルY位置

    void Start()
    {
        currentMoveStep = initialMoveStep; // 移動ステップの初期化
        initialY = transform.localPosition.y; // 初期のローカルY位置を記録
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted) // ハンマーが接触した場合のみ
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}"); // 移動前のステップを表示

            if (transform.localPosition.y - currentMoveStep > minY)
            {
                transform.localPosition -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor; // 移動ステップを減少
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");

                soundManager.PlaySound(currentMoveStep); // サウンドを再生

                canMove = false;
                Invoke("ResetMovement", cooldown); // クールダウン後に移動をリセット
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);
                soundManager.PlaySound(currentMoveStep);

                isInserted = CheckInsertion(); // 挿入されているかチェック
            }
        }
    }

    void ResetMovement()
    {
        canMove = true; // 移動を再び有効にする
    }

    public void CarveFace(float carvingDepth)
    {
        carvingCount++; // 削り回数をカウント
        initialMoveStep += carvingImpact; // 移動ステップに影響を与える
        decreaseFactor *= carvingDecreaseFactor; // 減少係数を更新

        currentMoveStep = initialMoveStep; // 現在の移動ステップを更新

        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount); // 削り音を再生
        }
    }

    bool CheckInsertion()
    {
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        return ontaBounds.min.y <= mentaBounds.max.y; // 挿入状態のチェック
    }

    public void ResetOnta()
    {
        currentMoveStep = initialMoveStep; // 初期の移動ステップにリセット
        canMove = true;
        isInserted = false;
        Debug.Log("OntaBehavior has been reset.");
    }
}