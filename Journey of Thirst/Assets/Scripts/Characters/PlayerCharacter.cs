using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour
{
    #region Variables
    private CharacterController controller;
    [SerializeField]
    private LayerMask groundLayerMask;

    private NavMeshAgent nav;
    private Camera camera;
    [SerializeField]
    private Animator anim;

    readonly int moveHash = Animator.StringToHash("Move");
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        nav = GetComponent<NavMeshAgent>();
        nav.updatePosition = false;
        nav.updateRotation = true;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                nav.SetDestination(hit.point);
            }
        }

        if(nav.remainingDistance > nav.stoppingDistance)
        {
            controller.Move(nav.velocity * Time.deltaTime);
            anim.SetBool(moveHash, true);
        }
        else
        {
            controller.Move(Vector3.zero);
            anim.SetBool(moveHash, false);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 position = nav.nextPosition;
        anim.rootPosition = nav.nextPosition;
        transform.position = position;
    }
}
