using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController3D : MonoBehaviour
{
    [SerializeField] private Transform _mustBeAlwaysInsideWater = null;

    [SerializeField] private Transform _trWater   = null;
    [SerializeField] private float     _waterSize = 100f;

    private int curChunkOfCamera = 0;
    private int curChunkOfWater  = 0;


    private void Update()
    {
        tryMoveWaterToCorrectPlace();
    }

    private void tryMoveWaterToCorrectPlace()
    {
        curChunkOfCamera = (int)((_mustBeAlwaysInsideWater.transform.position.x) / _waterSize * 2);
        curChunkOfWater  = (int)((_trWater.transform.position.x)  / _waterSize * 2);
        if ( curChunkOfWater != curChunkOfCamera )
        {
            Vector3 newWaterPos = _trWater.position;
            newWaterPos.x = curChunkOfCamera * _waterSize / 2;
            _trWater.position = newWaterPos;
        }
    }
}
