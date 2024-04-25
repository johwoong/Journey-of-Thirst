using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    #region Variables
    public Transform target;
    #endregion
    #region MainMethod

    private void Start()
    {
        HandleCamera();
    }

    private void LateUpdate()
    {
        HandleCamera();
    }
    #endregion

    #region Helper Method
    public virtual void HandleCamera()
    {

    }
    #endregion
}
