using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
<<<<<<< Updated upstream
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
=======
<<<<<<< Updated upstream
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
    public float initialMoveStep = 0.02f;  // �����ړ��X�e�b�v
    public float minY = 1.0f;              // �ŏ�Y���W�i�ړ���~�ʒu�j
    public float decreaseFactor = 0.94f;   // �ړ��X�e�b�v�����W��
    private float currentMoveStep;         // ���݂̈ړ��X�e�b�v
    private bool canMove = true;           // �ړ��\�t���O
    private float cooldown = 0.5f;         // ��p����
    public SoundManager soundManager;      // SoundManager�ւ̎Q��
    public AudioManager audioManager;      // AudioManager�ւ̎Q��
    public int carvingCount = 0;           // ���񐔂��J�E���g
    public float carvingDecreaseFactor = 0.98f; // ���ɂ�錸���W���̕ω�
    private float carvingImpact = 0.002f;  // ���̉e����
    private bool isInserted = false;       // �}���������������ǂ����������t���O
    public GameObject menta;               // Menta�I�u�W�F�N�g�ւ̎Q��
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
<<<<<<< HEAD
    private float initialY;                // ����Y���W��ێ�����ϐ�
=======
>>>>>>> Stashed changes
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start���ɏ����ړ��X�e�b�v��ݒ�
        initialY = transform.position.y;    // ����Y���W��ݒ�
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)  // �n���}�[���I�u�W�F�N�g�ɐG�ꂽ���ǂ���
        {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // �ړ��O�̃X�e�b�v�����O�ɏo��
            if (transform.position.y - currentMoveStep > minY && CheckFit())
=======
>>>>>>> Stashed changes
<<<<<<< Updated upstream
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // 移動前のステップをログに出力
            if (transform.position.y - currentMoveStep > minY)
=======
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // �ړ��O�̃X�e�b�v�����O�ɏo��
            if (transform.position.y - currentMoveStep > minY && CheckFit())
>>>>>>> Stashed changes
<<<<<<< Updated upstream
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);
                currentMoveStep *= decreaseFactor;  // �ړ��X�e�b�v������
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");  // �ړ���̃X�e�b�v�����O�ɏo��

                soundManager.PlaySound(currentMoveStep);  // SoundManager��ʂ��ĉ����Đ�

                canMove = false;  // �ړ��t���O��false�ɐݒ�
                Invoke("ResetMovement", cooldown);  // ��p���Ԍ�Ɉړ��t���O�����Z�b�g
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);  // Y���W���ŏ��l�ɒB�����ꍇ

                soundManager.PlaySound(currentMoveStep);  // SoundManager��ʂ��čŌ�̉����Đ�

                isInserted = CheckInsertion();  // �}���`�F�b�N
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;  // �ړ��t���O�����Z�b�g
    }

    // �ʂ���鏈����ǉ�
    public void CarveFace(float carvingDepth)
    {
        carvingCount++;  // ���񐔂��J�E���g
        initialMoveStep += carvingImpact;  // ���̉e���ʂɉ����ď����ړ������𑝉�
        decreaseFactor *= carvingDecreaseFactor;  // ���قǌ����W��������

        // currentMoveStep ���X�V
        currentMoveStep = initialMoveStep;

        // AudioManager���g���č�鉹���Đ�
        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount);
        }
    }

    bool CheckFit()
<<<<<<< Updated upstream
    {
        // Menta�̈ʒu�ƃT�C�Y���擾
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta�̈ʒu�ƃT�C�Y���擾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta�̒�ʂ�Menta�̏�ʂɊ��S�Ɏ��܂��Ă��邩�`�F�b�N
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
<<<<<<< Updated upstream
        // Mentaの位置とサイズを取得
=======
        // Menta�̈ʒu�ƃT�C�Y���擾
>>>>>>> Stashed changes
=======
<<<<<<< HEAD
    {
        // Menta�̈ʒu�ƃT�C�Y���擾
=======
    {
        // Menta�̈ʒu�ƃT�C�Y���擾
>>>>>>> Stashed changes
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta�̈ʒu�ƃT�C�Y���擾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta�̒�ʂ�Menta�̏�ʂɊ��S�Ɏ��܂��Ă��邩�`�F�b�N
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
<<<<<<< Updated upstream
        // Mentaの位置とサイズを取得
=======
        // Menta�̈ʒu�ƃT�C�Y���擾
>>>>>>> Stashed changes
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta�̈ʒu�ƃT�C�Y���擾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta�̒�ʂ�Menta�̏�ʂɊ��S�Ɏ��܂��Ă��邩�`�F�b�N
        return ontaBounds.min.x >= mentaBounds.min.x && ontaBounds.max.x <= mentaBounds.max.x &&
               ontaBounds.min.z >= mentaBounds.min.z && ontaBounds.max.z <= mentaBounds.max.z;
    }

    bool CheckInsertion()
    {
        // Menta�̈ʒu�ƃT�C�Y���擾
        Collider mentaCollider = menta.GetComponent<Collider>();
        Bounds mentaBounds = mentaCollider.bounds;

        // Onta�̈ʒu�ƃT�C�Y���擾
        Collider ontaCollider = GetComponent<Collider>();
        Bounds ontaBounds = ontaCollider.bounds;

        // Onta�̒�ʂ�Menta�̏�ʂɎ��܂��Ă��邩�`�F�b�N
        return ontaBounds.min.y <= mentaBounds.max.y;
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD

    bool CheckExcessHeight()
    {
        // ���݂̍����Ə��������̍����`�F�b�N
        return (initialY - transform.position.y) >= 0.3f;
    }
=======
>>>>>>> 4b46801ccc9551eb9393165074f2278c670820d2
>>>>>>> Stashed changes
}