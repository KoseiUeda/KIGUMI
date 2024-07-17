using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // �Đ����鉹���N���b�v
    public AudioClip[] carvingSounds; // ��鉹�̔z��
    private AudioSource audioSource; // AudioSource�̎Q��

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource�R���|�[�l���g��ǉ�
    }

    public void PlayStrikeSound()
    {
        if (strikeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(strikeSound);
        }
    }

    public void PlayCarvingSound(int carvingCount)
    {
        if (carvingSounds != null && carvingSounds.Length > 0 && audioSource != null)
        {
            int index = Mathf.Min(carvingCount, carvingSounds.Length - 1);
            audioSource.PlayOneShot(carvingSounds[index]);
        }
    }
}
