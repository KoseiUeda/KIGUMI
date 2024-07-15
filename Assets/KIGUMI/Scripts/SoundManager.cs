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
    public void PlaySound(float currentMoveStep)
    {
        foreach (var data in audioDataList) // audioDataList�����[�v���ēK�؂ȃI�[�f�B�I�\�[�X������
        {
            if (currentMoveStep >= data.minStep && currentMoveStep <= data.maxStep)
            {
                data.source.Play(); // �����ɍ��v����AudioSource���Đ�
                break; // ��v�����烋�[�v�𔲂���
            }
        }
    }
}