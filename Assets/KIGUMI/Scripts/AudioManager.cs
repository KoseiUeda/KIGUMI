using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] carvingSounds; // �قȂ鉹���N���b�v�̔z��
    private AudioSource audioSource; // AudioSource�̎Q��
    public float carvingSoundVolume = 0.4f; // ��鉹�̉��� (0.0 - 1.0)

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource�R���|�[�l���g��ǉ�
        audioSource.volume = carvingSoundVolume; // ���ʂ�ݒ�
    }

    public void PlayCarvingSound(int carvingCount)
    {
        int index = Mathf.Clamp(carvingCount, 0, carvingSounds.Length - 1);
        if (carvingSounds[index] != null && audioSource != null)
        {
            audioSource.PlayOneShot(carvingSounds[index], carvingSoundVolume); // ���ʂ��w�肵�čĐ�
        }
    }
}
