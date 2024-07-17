using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
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
    private bool hasBeenCarved = false;    // ��鏈�����s��ꂽ���ǂ����������t���O
    private Rigidbody rb;                  // Rigidbody�ւ̎Q��

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start���ɏ����ړ��X�e�b�v��ݒ�
        rb = GetComponent<Rigidbody>();     // Rigidbody�R���|�[�l���g���擾
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)  // �n���}�[���I�u�W�F�N�g�ɐG�ꂽ���ǂ���
        {
            if (!hasBeenCarved)
            {
                Debug.Log("Onta needs to be carved before it can be hammered into place.");
                return;
            }

            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // �ړ��O�̃X�e�b�v�����O�ɏo��

            if (CheckOverlap() && transform.position.y - currentMoveStep > minY)
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
                Debug.Log("Onta and Menta are overlapping. Cannot move.");
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
        hasBeenCarved = true;  // ��鏈�����s��ꂽ���Ƃ������t���O��ݒ�

        // currentMoveStep ���X�V
        currentMoveStep = initialMoveStep;

        // AudioManager���g���č�鉹���Đ�
        if (audioManager != null)
        {
            audioManager.PlayCarvingSound(carvingCount);
        }
    }

    bool CheckOverlap()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, GetComponent<Collider>().bounds.size / 2, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Menta"))
            {
                Debug.Log("Overlap detected with Menta.");
                return false;
            }
        }

        return true;
    }
}
