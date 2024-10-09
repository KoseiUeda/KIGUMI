using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f;  // ‰ŠúˆÚ“®ƒXƒeƒbƒv
    public float minY = 1.0f;              // Å¬YÀ•WiˆÚ“®’âŽ~ˆÊ’uj
    public float decreaseFactor = 0.94f;   // ˆÚ“®ƒXƒeƒbƒvŒ¸­ŒW”
    public float currentMoveStep;         // Œ»Ý‚ÌˆÚ“®ƒXƒeƒbƒv
    public bool canMove = true;           // ˆÚ“®‰Â”\ƒtƒ‰ƒO
    private float cooldown = 0.5f;         // —â‹pŽžŠÔ
    public SoundManager soundManager;      // SoundManager‚Ö‚ÌŽQÆ
    public AudioManager audioManager;      // AudioManager‚Ö‚ÌŽQÆ
    public int carvingCount = 0;           // í‚è‰ñ”‚ðƒJƒEƒ“ƒg
    public float carvingDecreaseFactor = 0.98f; // í‚è‚É‚æ‚éŒ¸­ŒW”‚Ì•Ï‰»
    private float carvingImpact = 0.002f;  // í‚è‚Ì‰e‹¿—Ê
    public bool isInserted = false;       // ‘}“ü‚ªŠ®—¹‚µ‚½‚©‚Ç‚¤‚©‚ðŽ¦‚·ƒtƒ‰ƒO
    public GameObject menta;               // MentaƒIƒuƒWƒFƒNƒg‚Ö‚ÌŽQÆ
    private float initialY;                // ‰ŠúYÀ•W‚ð•ÛŽ‚·‚é•Ï”

    void Start()
    {
        currentMoveStep = initialMoveStep;  // StartŽž‚É‰ŠúˆÚ“®ƒXƒeƒbƒv‚ðÝ’è
        initialY = transform.position.y;    // ‰ŠúYÀ•W‚ðÝ’è
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");

            // ローカル座標のY位置がminYより大きいか確認
            if (transform.localPosition.y - currentMoveStep > minY)
            {
                // ローカル座標でOntaを下に移動させる
                transform.localPosition -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor;  // 移動量を減少させる
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");

                soundManager.PlaySound(currentMoveStep);  // サウンド再生
                canMove = false;  // 一度動いたら次の動作まで待機
                Invoke("ResetMovement", cooldown);  // クールダウン後に移動をリセット
            }
            else
            {
                // 最小のY座標に達した場合、ローカル座標でY位置をminYに制限する
                transform.localPosition = new Vector3(transform.localPosition.x, minY, transform.localPosition.z);
                soundManager.PlaySound(currentMoveStep);  // サウンド再生
                isInserted = CheckInsertion();  // 挿入状態の確認
            }
        }
    }


    void ResetMovement()
    {
        canMove = true;  // ˆÚ“®ƒtƒ‰ƒO‚ðƒŠƒZƒbƒg
    }

    // –Ê‚ðí‚éˆ—‚ð’Ç‰Á
    public void CarveFace(float carvingDepth)
    {
        carvingCount++;  // í‚è‰ñ”‚ðƒJƒEƒ“ƒg
        initialMoveStep += carvingImpact;  // í‚è‚Ì‰e‹¿—Ê‚É‰ž‚¶‚Ä‰ŠúˆÚ“®‹——£‚ð‘‰Á
        decreaseFactor *= carvingDecreaseFactor;  // í‚é‚Ù‚ÇŒ¸­ŒW”‚à’²®

        // currentMoveStep ‚ðXV
        currentMoveStep = initialMoveStep;

        // AudioManager‚ðŽg‚Á‚Äí‚é‰¹‚ðÄ¶
        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount);
        }
    }

    bool CheckInsertion()
    {
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ðŽæ“¾
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ðŽæ“¾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta‚Ì’ê–Ê‚ªMenta‚Ìã–Ê‚ÉŽû‚Ü‚Á‚Ä‚¢‚é‚©ƒ`ƒFƒbƒN
        return ontaBounds.min.y <= mentaBounds.max.y;
    }
}
