using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fellow : MonoBehaviour
{
    protected CapsuleCollider collider_ = null;
    protected Rigidbody rb;
 
    [SerializeField]
    protected GameObject death_effect_ = null;

    [SerializeField]
    protected NavMeshAgent agent_;
    // �ǂ�������L�����N�^�[

    //�v���C���[
    // [SerializeField]
    protected GameObject player_ = null;

    [SerializeField]
    protected GameObject[] fellows_obj_ = null;

    [SerializeField]
    protected Fellow[] fellow_ /*= new Fellow[100]*/;

    [SerializeField]
    protected GameObject chase_target_;

    [SerializeField]
    protected Animator animator_;
    //�@���������Ƃ��鋗��
    [SerializeField]
    protected float arrivedDistance_ = 0.4f;
    //�@�ǂ������n�߂鋗��
    [SerializeField]
    protected float followDistance_ = 0.6f;

    

    [SerializeField]
    protected bool follow_flg_ = false;

    [SerializeField]
    public bool Fellow_flg_
    {
        get { return follow_flg_; }
    }

   [SerializeField,Tooltip("���G����")]
    protected float life_inv_time_ = 3.0f;

    //���G����
    protected float life_inv_tmp_ = 0.0f;

    [SerializeField]
    protected Player player_script_ = null;
    protected PlayerFellowCount fellowcount_script_ = null;

    //�������Ō�����ǂ���
    [SerializeField]
    protected bool last_ = false;

    //��������ŉ��Ԗڂ�
    protected int row_ = 0;

   //�ǂݎ��p
    public int Row_
    {
        get { return row_; }
    }

    [SerializeField]
    protected Vector3 getLastFellow_;

    public Vector3 GetLastFellow
    {
        get { return getLastFellow_; }
    }



    [SerializeField]
    protected RaderIcon radericon_ = null;

    [SerializeField]
    public FelloTalk fellotalk_ = null;
    
    public enum fellows_
        {
         normal,
         bob,
         nic,
         spencer,
         alan,
         catherine,
         groupParent,
         groupEntourage

    };
    [Header("���Ԃ̎��")]
    [HideInInspector]
    public  fellows_ type_= fellows_.normal;

    [Header("�f�o�b�N�p")]
    [SerializeField]
    protected bool debug_ = false;



    //�~�����ꂽ�Ƃ��Ɉ�x�������s
    [SerializeField]
    protected bool rescue_flg_ = false;

    public bool Rescue_flg_
    {
        get { return rescue_flg_; }
    }

  

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        death_effect_.SetActive(false);



        radericon_ = GetComponent<RaderIcon>();
        // animator_ = GetComponent<Animator>();
        agent_ = GetComponent<NavMeshAgent>();
        fellows_obj_ = GameObject.FindGameObjectsWithTag("Fellow");

        player_ = GameObject.FindGameObjectWithTag("Player");

        player_script_ = player_.GetComponent<Player>();
        fellowcount_script_ = player_.GetComponent<PlayerFellowCount>();

        collider_ = GetComponent<CapsuleCollider>();

        //�ŏ����烁���o�ϐ��ɓ���悤�Ƃ���Ƃ����ς����邩�烍�[�J���ϐ��ɂ����������Ă��烁���o�ϐ��ɓ��ꂽ
        var _fellow = new Fellow[fellows_obj_.Length];

        for (int h = 0; h < fellows_obj_.Length; h++)
        {
            _fellow[h] = fellows_obj_[h].GetComponent<Fellow>();
        }

        fellow_ = _fellow;

        
    }


    // Update is called once per frame
    protected virtual void Update()
    {
       

        if (follow_flg_)
        {
            //�~�����ꂽ�Ƃ��Ɉ�x�������s
            if (!rescue_flg_)
            {
               
                GameProgress.instance_.FriendWhoHelpedCount();

                //���[�_�[�A�C�R������������
                radericon_.Detectioned();

                RescueProcess();

                rescue_flg_ = true;           
            }

            Move();
            
        }
        //�����͂ɉ������鏈��
        if (fellowcount_script_.fellow_Count_ == 0 && follow_flg_)
        {
            row_ = 0;
            transform.position = new(0, 0, 0);
            this.gameObject.SetActive(false);
            follow_flg_ = false;
        }


    }

    protected virtual void RescueProcess()
    {
       
        
    }

    

   

    protected virtual void Move()
    {
        //���Ԃ��͂��ꂽ��
        if (Vector3.Distance(player_.transform.position, transform.position) > 4)
        {
            //�v���C���[�̋߂��Ƀe���|�[�g
            transform.position = player_.transform.position;
        }

        agent_.speed = player_script_.Player_Move_;

        agent_.SetDestination(chase_target_.transform.position);
        //�@�������Ă��鎞
        if (agent_.remainingDistance < arrivedDistance_)
        {

            agent_.isStopped = true;
            animator_.SetBool("isWalking", false);

            //animator_.SetBool("isRunning", player_script_.Fire2_Flg__);
            //animator_.SetFloat("Speed", 0f);

            //�@�������Ă��Ȃ����Œǂ������o�������ɂȂ�����
        }
        else if (agent_.remainingDistance > followDistance_)
        {

            animator_.SetBool("isWalking", true);
            agent_.isStopped = false;
            if (player_script_.type_==Player.State.Dash)
            {
                animator_.SetBool("isRunning", true);

            }
            else if (player_script_.type_ == Player.State.Run)
            {

                animator_.SetBool("isRunning", false);
            }

            //animator_.SetFloat("Speed", agent_.desiredVelocity.magnitude);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (life_inv_tmp_ > 0)
        {
            life_inv_tmp_ -= 1.0f * Time.deltaTime;
        }
    }
    protected virtual void OnAnimatorIK()
    {
        animator_.SetLookAtWeight(1f, 0.3f, 1f, 0f, 0.5f);
        animator_.SetLookAtPosition(chase_target_.transform.position + Vector3.up * 1.5f);
    }

   

    //�v���C���[�Ƀ^�b�`���ꂽ�Ƃ��̔���
    public virtual void Follow()
    {
       
        follow_flg_ = true;
        if (fellowcount_script_.fellow_Count_ > 1)
        {
            Debug.Log("A");
            row_ = fellowcount_script_.fellow_Count_ ;
            //�ق��ɒ��Ԃ�����ꍇ�������Ō���ɂȂ�
            last_ = true;

            for (int i = 0; i < fellows_obj_.Length; i++)
            {

                if (fellow_[i].Row_ == row_ - 1)
                {
                    chase_target_ = fellows_obj_[i];
                    fellow_[i].AddFellow(); 
                    break;
                }
            }


           
        }
        else
        {
            last_ = true;
            row_ = 1;
            //���Ԃ�A��Ă��Ȃ��Ƃ�
            chase_target_ = player_;

        }
    }
    public virtual void AddFellow()
    {
        last_ = false;

    }

    //�Ō������e�����ꍇ�̈��p������
    public virtual void Last()
    {
        last_ = true;

    }


    protected virtual void DeathProcess()
    {
        fellowcount_script_.fellow_die_row_ = row_;

        row_ = 0;
    
        agent_.enabled = false;

        rb.useGravity = false;
        rb.isKinematic = true;
        collider_.enabled = false;

        death_effect_.SetActive(true);

        follow_flg_ = false;
       
        player_script_.FellowHit();

        Vector3 position = transform.position;
        position.y = 0.07f;
        transform.position = position;

        transform.eulerAngles = new Vector3(90, transform.rotation.y, 0);

        animator_.SetBool("isRunning", false);
        animator_.SetBool("isWalking", false);
    }



    public virtual void Death()
    {

        if(follow_flg_)
        {
            int _fellow_die = fellowcount_script_.fellow_die_row_;
            if(_fellow_die <row_)
            {
               
                row_ -= 1;
                if (row_ == 1)
                {
                    row_ = 1;
                    chase_target_ = player_;

                }
                else
                {
                 
                    for (int j = 0; j < fellows_obj_.Length; j++)
                    {
                        if (fellow_[j].Row_ == row_ - 1)
                        {
                            chase_target_ = fellows_obj_[j];
                            break;

                        }
                    }

                }
            }      

        }

    }



    protected virtual void OnCollisionStay(Collision collision)
    {

        Debug.Log("a");
        // OnCollisionStay
        if (collision.gameObject.tag == "Enemy" )
        {
            
            Debug.Log("a");
            if (player_script_.Player_life_inv_tmp_ <= 0 && follow_flg_ == true)
            {
                if (last_) //�������Ō���̎�
                {
                    Debug.Log("�Ō�����S");
                    //���񂾂Ƃ��̏���

                    if (row_ != 1)
                    {
                        for (int i = 0; i < fellows_obj_.Length; i++)
                        {
                            if (fellow_[i].Row_ == row_ - 1)
                            {
                                fellow_[i].Last();
                                break;
                            }
                        }
                    }
                    DeathProcess();
                    //gameObject.SetActive(false);


                }
                else//�������Ō���ł͂Ȃ��Ƃ�
                {
                    Debug.Log("�Ō������Ȃ����Ԃ���������");
                    DeathProcess();
                    for (int i = 0; i < fellows_obj_.Length; i++)
                    {

                        fellow_[i].Death();
                        //if (fellow_[i].Row_ == row_ + 1)
                        //{
                        //    fellow_[i].Death();
                        //    break;
                        //}
                    }


                    //���G���ԊJ�n
                    //life_inv_tmp_ = life_inv_time_;
                    //player_script_.FellowHit();
                }
            }
        }
    }
}
