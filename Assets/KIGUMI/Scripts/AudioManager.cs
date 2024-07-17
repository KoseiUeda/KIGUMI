using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] carvingSounds; // �قȂ鉹���N���b�v�̔z��
    private AudioSource audioSource; // AudioSource�̎Q��

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource�R���|�[�l���g��ǉ�
        audioSource.volume = 0.4f; // ���ʂ�0.7�ɐݒ�
    }

    public void PlayCarvingSound(int carvingCount)
    {
        int index = Mathf.Clamp(carvingCount, 0, carvingSounds.Length - 1);
        if (carvingSounds[index] != null && audioSource != null)
        {
            audioSource.PlayOneShot(carvingSounds[index]);
        }
    }
}
