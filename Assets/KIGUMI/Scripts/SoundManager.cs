using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // �C���X�y�N�^�ŕ\�������邽�߂�Serializable�������w��
public class AudioData
{
    public float minStep; // ���̒l�ȏ�
    public float maxStep; // ���̒l�ȉ��ł��̉������Đ�
    public AudioSource source; // ���͈̔͂ɑΉ�����AudioSource
}


public class SoundManager : MonoBehaviour
{
    public List<AudioData> audioDataList; // AudioData�̃��X�g

    // currentMoveStep�Ɋ�Â��ēK�؂�AudioSource��I�����čĐ�
    public void PlaySound(float stepAmount)
    {
        foreach (var data in audioDataList)
        {
            if (stepAmount >= data.minStep && stepAmount <= data.maxStep)
            {
                data.source.Play();
                break;
            }
        }
    }
}
