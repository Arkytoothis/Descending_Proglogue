using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Descending.Units;
using UnityEngine;

namespace Descending.Combat
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject _actionCamera = null;

        private void Start()
        {
            BaseAction.OnActionStarted += BaseAction_OnActionStarted;
            BaseAction.OnActionCompleted += BaseAction_OnActionCompleted;
            
            HideActionCamera();
        }

        private void ShowActionCamera()
        {
            _actionCamera.SetActive(true);
        }
        
        private void HideActionCamera()
        {
            _actionCamera.SetActive(false);
        }

        private void BaseAction_OnActionStarted(object sender, EventArgs e)
        {
            if (TurnManager.Instance.IsPlayerTurn == false) return;
            
            switch (sender)
            {
                case ShootAction shootAction:
                    Unit shooter = shootAction.Unit;
                    Unit target = shootAction.TargetUnit;
                    Vector3 cameraHeight = Vector3.up * 1.7f;
                    Vector3 cameraBack = Vector3.back * 0.5f;
                    float shoulderOffsetAmount = 0.75f;
                    Vector3 shootDirection = (target.transform.position - shooter.transform.position).normalized;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;
                    Vector3 actionCameraPosition = shooter.transform.position + cameraHeight + cameraBack + shoulderOffset + (shootDirection * -1);

                    _actionCamera.transform.position = actionCameraPosition;
                    _actionCamera.transform.LookAt(target.CameraTarget);
                    
                    ShowActionCamera();
                    break;
            }
        }

        private void BaseAction_OnActionCompleted(object sender, EventArgs e)
        {
            if (TurnManager.Instance.IsPlayerTurn == false) return;
            
            switch (sender)
            {
                case ShootAction shootAction:
                    HideActionCamera();
                    break;
            }
        }
    }
}