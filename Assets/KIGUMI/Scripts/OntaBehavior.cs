using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
<<<<<<< Updated upstream
    public float initialMoveStep = 0.02f;  // åˆæœŸç§»å‹•ã‚¹ãƒ†ãƒƒãƒ—
    public float minY = 1.0f;              // æœ€å°Yåº§æ¨™ï¼ˆç§»å‹•åœæ­¢ä½ç½®ï¼‰
    public float decreaseFactor = 0.94f;   // ç§»å‹•ã‚¹ãƒ†ãƒƒãƒ—æ¸›å°‘ä¿‚æ•°
    private float currentMoveStep;         // ç¾åœ¨ã®ç§»å‹•ã‚¹ãƒ†ãƒƒãƒ—
    private bool canMove = true;           // ç§»å‹•å¯èƒ½ãƒ•ãƒ©ã‚°
    private float cooldown = 0.5f;         // å†·å´æ™‚é–“
    public SoundManager soundManager;      // SoundManagerã¸ã®å‚ç…§
    public AudioManager audioManager;      // AudioManagerã¸ã®å‚ç…§
    public int carvingCount = 0;           // å‰Šã‚Šå›æ•°ã‚’ã‚«ã‚¦ãƒ³ãƒˆ
    public float carvingDecreaseFactor = 0.98f; // å‰Šã‚Šã«ã‚ˆã‚‹æ¸›å°‘ä¿‚æ•°ã®å¤‰åŒ–
    private float carvingImpact = 0.002f;  // å‰Šã‚Šã®å½±éŸ¿é‡
    private bool isInserted = false;       // æŒ¿å…¥ãŒå®Œäº†ã—ãŸã‹ã©ã†ã‹ã‚’ç¤ºã™ãƒ•ãƒ©ã‚°
    public GameObject menta;               // Mentaã‚ªãƒ–ã‚¸ã‚§ã‚¯ãƒˆã¸ã®å‚ç…§
    private float initialY;                // åˆæœŸYåº§æ¨™ã‚’ä¿æŒã™ã‚‹å¤‰æ•°
=======
<<<<<<< Updated upstream
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
    public float initialMoveStep = 0.02f;  // ‰ŠúˆÚ“®ƒXƒeƒbƒv
    public float minY = 1.0f;              // Å¬YÀ•WiˆÚ“®’â~ˆÊ’uj
    public float decreaseFactor = 0.94f;   // ˆÚ“®ƒXƒeƒbƒvŒ¸­ŒW”
    private float currentMoveStep;         // Œ»İ‚ÌˆÚ“®ƒXƒeƒbƒv
    private bool canMove = true;           // ˆÚ“®‰Â”\ƒtƒ‰ƒO
    private float cooldown = 0.5f;         // —â‹pŠÔ
    public SoundManager soundManager;      // SoundManager‚Ö‚ÌQÆ
    public AudioManager audioManager;      // AudioManager‚Ö‚ÌQÆ
    public int carvingCount = 0;           // í‚è‰ñ”‚ğƒJƒEƒ“ƒg
    public float carvingDecreaseFactor = 0.98f; // í‚è‚É‚æ‚éŒ¸­ŒW”‚Ì•Ï‰»
    private float carvingImpact = 0.002f;  // í‚è‚Ì‰e‹¿—Ê
    private bool isInserted = false;       // ‘}“ü‚ªŠ®—¹‚µ‚½‚©‚Ç‚¤‚©‚ğ¦‚·ƒtƒ‰ƒO
    public GameObject menta;               // MentaƒIƒuƒWƒFƒNƒg‚Ö‚ÌQÆ
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
<<<<<<< HEAD
    private float initialY;                // ‰ŠúYÀ•W‚ğ•Û‚·‚é•Ï”
=======
>>>>>>> Stashed changes
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start‚É‰ŠúˆÚ“®ƒXƒeƒbƒv‚ğİ’è
        initialY = transform.position.y;    // ‰ŠúYÀ•W‚ğİ’è
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)  // ƒnƒ“ƒ}[‚ªƒIƒuƒWƒFƒNƒg‚ÉG‚ê‚½‚©‚Ç‚¤‚©
        {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // ˆÚ“®‘O‚ÌƒXƒeƒbƒv‚ğƒƒO‚Éo—Í
            if (transform.position.y - currentMoveStep > minY && CheckFit())
=======
>>>>>>> Stashed changes
<<<<<<< Updated upstream
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // ç§»å‹•å‰ã®ã‚¹ãƒ†ãƒƒãƒ—ã‚’ãƒ­ã‚°ã«å‡ºåŠ›
            if (transform.position.y - currentMoveStep > minY)
=======
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // ˆÚ“®‘O‚ÌƒXƒeƒbƒv‚ğƒƒO‚Éo—Í
            if (transform.position.y - currentMoveStep > minY && CheckFit())
>>>>>>> Stashed changes
<<<<<<< Updated upstream
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor;  // ˆÚ“®ƒXƒeƒbƒv‚ğŒ¸­
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");  // ˆÚ“®Œã‚ÌƒXƒeƒbƒv‚ğƒƒO‚Éo—Í

                soundManager.PlaySound(currentMoveStep);  // SoundManager‚ğ’Ê‚¶‚Ä‰¹‚ğÄ¶

                canMove = false;  // ˆÚ“®ƒtƒ‰ƒO‚ğfalse‚Éİ’è
                Invoke("ResetMovement", cooldown);  // —â‹pŠÔŒã‚ÉˆÚ“®ƒtƒ‰ƒO‚ğƒŠƒZƒbƒg
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);  // YÀ•W‚ªÅ¬’l‚É’B‚µ‚½ê‡

                soundManager.PlaySound(currentMoveStep);  // SoundManager‚ğ’Ê‚¶‚ÄÅŒã‚Ì‰¹‚ğÄ¶

                isInserted = CheckInsertion();  // ‘}“üƒ`ƒFƒbƒN
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;  // ˆÚ“®ƒtƒ‰ƒO‚ğƒŠƒZƒbƒg
    }

    // –Ê‚ğí‚éˆ—‚ğ’Ç‰Á
    public void CarveFace(float carvingDepth)
    {
        carvingCount++;  // í‚è‰ñ”‚ğƒJƒEƒ“ƒg
        initialMoveStep += carvingImpact;  // í‚è‚Ì‰e‹¿—Ê‚É‰‚¶‚Ä‰ŠúˆÚ“®‹——£‚ğ‘‰Á
        decreaseFactor *= carvingDecreaseFactor;  // í‚é‚Ù‚ÇŒ¸­ŒW”‚à’²®

        // currentMoveStep ‚ğXV
        currentMoveStep = initialMoveStep;

        // AudioManager‚ğg‚Á‚Äí‚é‰¹‚ğÄ¶
        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount);
        }
    }

    bool CheckFit()
<<<<<<< Updated upstream
    {
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta‚Ì’ê–Ê‚ªMenta‚Ìã–Ê‚ÉŠ®‘S‚Éû‚Ü‚Á‚Ä‚¢‚é‚©ƒ`ƒFƒbƒN
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
<<<<<<< Updated upstream
        // Mentaã®ä½ç½®ã¨ã‚µã‚¤ã‚ºã‚’å–å¾—
=======
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
>>>>>>> Stashed changes
=======
<<<<<<< HEAD
    {
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
=======
    {
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
>>>>>>> Stashed changes
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta‚Ì’ê–Ê‚ªMenta‚Ìã–Ê‚ÉŠ®‘S‚Éû‚Ü‚Á‚Ä‚¢‚é‚©ƒ`ƒFƒbƒN
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
<<<<<<< Updated upstream
        // Mentaã®ä½ç½®ã¨ã‚µã‚¤ã‚ºã‚’å–å¾—
=======
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
>>>>>>> Stashed changes
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta‚Ì’ê–Ê‚ªMenta‚Ìã–Ê‚ÉŠ®‘S‚Éû‚Ü‚Á‚Ä‚¢‚é‚©ƒ`ƒFƒbƒN
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
        // Menta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta‚ÌˆÊ’u‚ÆƒTƒCƒY‚ğæ“¾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta‚Ì’ê–Ê‚ªMenta‚Ìã–Ê‚Éû‚Ü‚Á‚Ä‚¢‚é‚©ƒ`ƒFƒbƒN
        return ontaBounds.min.y <= mentaBounds.max.y;
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD

    bool CheckExcessHeight()
    {
        // Œ»İ‚Ì‚‚³‚Æ‰Šú‚‚³‚Ì·‚ğƒ`ƒFƒbƒN
        return (initialY - transform.position.y) >= 0.3f;
    }
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
}