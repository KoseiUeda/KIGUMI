using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // �Đ����鉹���N���b�v
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
}
