using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks,IDamageable
{
    public enum EnumStateAnimation {Idle,Walk,Run }
    public EnumStateAnimation stateAnimation;

    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject playerModel;

    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprinntSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;

    [SerializeField] Item[] items;

    int itemIndex;
    int previousitemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView pv;

    const float maxHealth = 100f;
    float currenHealth = maxHealth;

    PlayerManager playerManager;

    [SerializeField] Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv= GetComponent<PhotonView>();

        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            EquipItem(0);
            //Destroy(playerModel);

            var renderer = playerModel.transform.GetComponentsInChildren<Renderer>();

            foreach (var rend in renderer)
            {
                rend.enabled = false;
            }
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    private void Update()
    {

        if (!pv.IsMine)
        {
            return;
        }

        PlayAnimation(stateAnimation.ToString());
        CheckMoving(moveAmount);
        look();
        Move(); 
        jump();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        if(transform.position.y < -10f)
        {
            Die();
        }
    }

    
    string previousStateAnimation = "";
    string currentStateAnimation;
    void PlayAnimation(string _stateAnimation)
    {
        currentStateAnimation = _stateAnimation;

        if (currentStateAnimation != previousStateAnimation)
        {
            animator.SetBool(currentStateAnimation, true);
            animator.SetBool(previousStateAnimation, false);

            previousStateAnimation = currentStateAnimation;
        }
        else { animator.SetBool(currentStateAnimation, true); }

        //pv.RPC("RPC_PlayAnimation", RpcTarget.All, _stateAnimation);
    }

    [PunRPC]
    void RPC_PlayAnimation(string _stateAnimation)
    {
        currentStateAnimation = _stateAnimation;

        if (currentStateAnimation != previousStateAnimation)
        {
            animator.SetBool(currentStateAnimation, true);
            animator.SetBool(previousStateAnimation, false);

            previousStateAnimation = currentStateAnimation;
        }
        else { animator.SetBool(currentStateAnimation, true); }
    }

    void look()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprinntSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        //Debug.Log("moveDir : " + moveDir);
        //Debug.Log("moveAmount : " + moveAmount);

        
    }

    void CheckMoving(Vector3 move)
    {
        if ((move.x > 0.5f || move.x < -0.5f)||(move.y>0.5 || move.y < -0.5f )|| (move.z > 0.5f || move.z < - 0.5f))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                stateAnimation = PlayerController.EnumStateAnimation.Run;
                return;
            }
            stateAnimation = PlayerController.EnumStateAnimation.Walk;
            return;
        }
        else { stateAnimation= PlayerController.EnumStateAnimation.Idle; }

        //Debug.Log(move);
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);

            animator.SetTrigger("Jump");
            //animator.ResetTrigger("Jump");
        }
    }

    void EquipItem(int _index)
    {
        if (_index == previousitemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if(previousitemIndex != -1)
        {
            items[previousitemIndex].itemGameObject.SetActive(false);
        }

        previousitemIndex = itemIndex;

        if (pv.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash); 
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!pv.IsMine&& targetPlayer == pv.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if(!pv.IsMine)
            return;

        currenHealth -= damage;

        healthbarImage.fillAmount = currenHealth / maxHealth;

        if(currenHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
     playerManager.Die();
    }
}
