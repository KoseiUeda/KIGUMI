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
    public GameObject menta;               // Menta�I�u�W�F�N�g�ւ̎Q��
    private float initialY;                // ����Y���W��ێ�����ϐ�

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start���ɏ����ړ��X�e�b�v��ݒ�
        initialY = transform.position.y;    // ����Y���W��ݒ�
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove && !isInserted)  // �n���}�[���I�u�W�F�N�g�ɐG�ꂽ���ǂ���
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // �ړ��O�̃X�e�b�v�����O�ɏo��
            if (transform.position.y - currentMoveStep > minY)
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
}
