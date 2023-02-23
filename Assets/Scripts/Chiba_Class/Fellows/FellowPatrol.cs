using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FellowPatrol : MonoBehaviour
{
    //[SerializeField]
    private Fellow fellow_script_ = null;

    // NavMeshAgent�R���|�[�l���g
    private NavMeshAgent agent_ = null;

    //�A�^�b�`����Ă���I�u�W�F�N�g�̃A�j���[�^�[(�Ȃ��ꍇ�͊K�w�̉��ɂ���\������)
    [SerializeField]
    private Animator animator_ = null;

    [SerializeField]
    [Tooltip("���񂷂�n�_�̔z��")]
   // private Transform[] waypoint_ = null;
    private List<Transform> waypoint_ = new List<Transform> {};

    [SerializeField]
    private float wait_time_ = 3.0f;

    // ���݂̖ړI�n
    private int waypoint_index = 0;

   
    // Start is called before the first frame update
    void Start()
    {
        //�ړI�n���ݒ肳��Ă��Ȃ��ꍇ�͂��̃X�N���v�g�̓I�t�ɂ���
        if(waypoint_.Count == 0)
        {
            this.enabled = false;
        }

        fellow_script_ = GetComponent<Fellow>();
        agent_ = GetComponent<NavMeshAgent>();
       
        // �ŏ��̖ړI�n������
        agent_.SetDestination(waypoint_[0].position);

        animator_.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!fellow_script_.Fellow_flg_)
        {
           
            // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
            if (agent_.remainingDistance <= agent_.stoppingDistance)
            {
                StartCoroutine(NextWayPoint());
            }
        }
        else
        {
            StopCoroutine(NextWayPoint());
            //�A�j���[�V�����X�g�b�v
            animator_.SetBool("isWalking", false);
           
            //���̃R���|�[�l���g���A�N�e�B�u�ɂ���
            this.enabled = false;
        }
    }

    private IEnumerator NextWayPoint()
    {
        //�A�j���[�V�����X�g�b�v
        animator_.SetBool("isWalking", false);

        //�ړ���~����
        agent_.isStopped = true;

        // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
        waypoint_index = (waypoint_index + 1) % waypoint_.Count;

        // �ړI�n�����̏ꏊ��
        agent_.SetDestination(waypoint_[waypoint_index].position);

        //�w�肳�ꂽ���Ԃ����ҋ@
        yield return new WaitForSeconds(wait_time_);  

        //�ړ��ĊJ
        agent_.isStopped = false;

        //�A�j���[�V�����ĊJ
        animator_.SetBool("isWalking", true);
    }

    //�z��̎󂯎��p
    public void SetWayPoint(Transform[] _a)
    {
        //���X�g�̐������܂킷
        for(int i = 0; i < _a.Length; i++)
        {
            //���X�g�̏�����
            waypoint_ = new List<Transform>();

            //���X�g�ɒǉ�
            waypoint_.Add(_a[i]);
        }
    }
}
