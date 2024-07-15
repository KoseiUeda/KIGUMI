using UnityEngine;

public class OntaBehavior : MonoBehaviour
{
    public float initialMoveStep = 0.02f;  // �����ړ��X�e�b�v
    public float minY = 1.0f;             // �ŏ�Y���W�i�ړ���~�ʒu�j
    public float decreaseFactor = 0.94f;  // �ړ��X�e�b�v�����W��
    private float currentMoveStep;        // ���݂̈ړ��X�e�b�v
    private bool canMove = true;          // �ړ��\�t���O
    private float cooldown = 0.5f;        // ��p����
    public SoundManager soundManager;     // SoundManager�ւ̎Q��

    void Start()
    {
        currentMoveStep = initialMoveStep;  // Start���ɏ����ړ��X�e�b�v��ݒ�
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer" && canMove)  // �n���}�[���I�u�W�F�N�g�ɐG�ꂽ���ǂ���
        {
            Debug.Log($"Before moving: currentMoveStep = {currentMoveStep}");  // �ړ��O�̃X�e�b�v�����O�ɏo��
            if (transform.position.y - currentMoveStep > minY)
            {
                transform.position -= new Vector3(0, currentMoveStep, 0);  // Y���ɉ����Ĉړ�
                currentMoveStep *= decreaseFactor;  // �ړ��X�e�b�v������
                Debug.Log($"After moving: currentMoveStep = {currentMoveStep}");  // �ړ���̃X�e�b�v�����O�ɏo��

                soundManager.PlaySound(currentMoveStep);  // SoundManager��ʂ��ĉ����Đ�

                canMove = false;  // �ړ��t���O��false�ɐݒ�
                Invoke("ResetMovement", cooldown);  // ��p���Ԍ�Ɉړ��t���O�����Z�b�g
            }
            else
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);  // Y���W���ŏ��l�ɒB�����ꍇ
            }
        }
    }

    void ResetMovement()
    {
        canMove = true;  // �ړ��t���O�����Z�b�g
    }
}
