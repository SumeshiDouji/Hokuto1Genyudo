using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayer : MonoBehaviour
{
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;
    public LayerMask Portallayer
    {
        get => portalLayer;
    }
    public LayerMask TriggerableLayers
    {
        get => grassLayer | fovLayer | portalLayer;
    }
}
