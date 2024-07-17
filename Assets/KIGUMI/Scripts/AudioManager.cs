using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // �Đ����鉹���N���b�v
    private AudioSource audioSource; // AudioSource�̎Q��

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource�R���|�[�l���g��ǉ�
        audioSource.volume = 0.4f; // ���ʂ�0.7�ɐݒ�
    }

    public void PlayStrikeSound()
    {
        if (strikeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(strikeSound, 0.4f); // ����0.7�ŉ����N���b�v���Đ�
        }
    }
}
