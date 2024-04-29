using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public GameObject Camera;
    BoxCollider hitBox;


    [SerializeField]
    BoxCollider weapon;
    [SerializeField]
    float _walkSpeed = 2.0f;
    [SerializeField]
    float _runSpeed = 5.0f;
    [SerializeField]
    float _rollCooltime = 3f;
    bool _walk = false;
    bool _run = false;
    bool _attack = true;
    bool _roll = false;
    bool canRoll = true;
    [SerializeField]
    bool _isDie = false;



    public enum State
    {
        IDLE,
        WALK,
        RUN,
        ATTACK,
        ROLL,
    }

    public State playerState;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.LeftClickAction -= OnLeftClicked;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.LeftClickAction += OnLeftClicked;

        hitBox = GetComponent<BoxCollider>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerState = State.IDLE;
        animator.SetInteger("state", 0);
    }

    private void Update()
    {
        if (_isDie)
            return;
        switch (playerState)
        {
            case State.IDLE:
                break;
            case State.WALK:
                if (!_walk)
                {
                    changeState(State.IDLE);
                }
                break;
            case State.RUN:
                if (!_run)
                {
                    changeState(State.IDLE);
                }
                break;
            case State.ATTACK:
                if (!_attack)
                {
                    changeState(State.IDLE);
                }
                break;
            case State.ROLL:
                hitBox.enabled = false;
                if (!_roll)
                {
                    changeState(State.IDLE);
                }
                break;
        }
        _run = false;
        _walk = false;
    }

    protected void changeState(State state)
    {
        switch (state)
        {
            case State.IDLE:
                hitBox.enabled = true;
                playerState = State.IDLE;
                animator.SetInteger("state", 0);
                break;
            case State.WALK:
                playerState = State.WALK;
                animator.SetInteger("state", 1);
                _walk = true;
                break;
            case State.RUN:
                playerState = State.RUN;
                animator.SetInteger("state", 2);
                _run = true;
                break;
            case State.ATTACK:
                weapon.enabled = true;
                playerState = State.ATTACK;
                animator.SetInteger("state", 3);
                _attack = true;
                Invoke("stopCounter", 1.45f);
                break;
            case State.ROLL:
                playerState = State.ROLL;
                animator.SetInteger("state", 4);
                _roll = true;
                canRoll = false;
                Invoke("stopCounter", 0.75f);
                Invoke("rollCooltime", _rollCooltime);
                break;
        }
    }

    protected void stopCounter()
    {
        _attack = false;
        _roll = false;
        weapon.enabled = false;
    }

    protected void rollCooltime()
    {
        canRoll = true;
        Debug.Log("구르기 준비");
    }

    void OnLeftClicked(bool clicked)
    {
        if (clicked && playerState != State.ATTACK && !_isDie)
        {
            changeState(State.ATTACK);
        }
    }

    void OnKeyboard(KeyCode key)
    {

        if (playerState != State.ATTACK)
        {
            if (playerState != State.ROLL)
            {
                if (key == KeyCode.Space && canRoll)
                {
                    changeState(State.ROLL);
                }
                if (key == KeyCode.LeftShift)
                {
                    changeState(State.RUN);
                }

            }
            if (playerState == State.IDLE || playerState == State.WALK)
            {
                changeState(State.WALK);
            }

            Move(key);
        }
    }
    void Move(KeyCode key)
    {
        float speed = _walkSpeed;
        var offset = Camera.transform.forward;
        offset.y = 0;


        if (playerState == State.RUN)
        {
            speed = _runSpeed;
        }

        if (playerState == State.ROLL)
        {
            speed = 1f;
        }
        if (!_isDie)
        {
            if (key == KeyCode.W)
            {
                var dir = offset;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
                transform.position += dir * Time.deltaTime * speed;
            }
            if (key == KeyCode.S)
            {
                var dir = -offset;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
                transform.position += dir * Time.deltaTime * speed;
            }
            if (key == KeyCode.D)
            {
                var dir = new Vector3(offset.z, offset.y, -offset.x);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
                transform.position += dir * Time.deltaTime * speed;
            }
            if (key == KeyCode.A)
            {
                var dir = new Vector3(-offset.z, offset.y, offset.x);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
                transform.position += dir * Time.deltaTime * speed;
            }
        }



    }



}
