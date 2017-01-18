﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class MovementManager : MonoBehaviour
    {
        [HideInInspector]
        public RaycastHit HitInfo;
        private GameObject _lastHit;
        public float MaxDistance = 2.5f;
        [HideInInspector]
        public bool Hit;
        [HideInInspector]
        public Vector3 CameraPos;

        // Update is called once per frame
        void Update()
        {
            CameraPos = Camera.main.transform.position;
            Hit = Physics.Raycast(CameraPos, Vector3.down, out HitInfo,
                MaxDistance);
            if (Hit)
            {
                HandleTargets();
            }
            else
            {
                if (_lastHit != null)
                {
                    //Possible Exit Event here
                    _lastHit = null;
                }
            }
        }

        void HandleTargets()
        {
            GameObject go = HitInfo.collider.gameObject;
            if (go != _lastHit)
            {
                _lastHit = go;
                Interactable_Pos target = _lastHit.GetComponent<Interactable_Pos>();
                if (target != null)
                {
                    target.Enter();
                }
            }
        }
    }
}