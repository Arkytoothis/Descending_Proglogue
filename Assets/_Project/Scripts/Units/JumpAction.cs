using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Units;
using UnityEngine;

namespace Descending
{
    public class JumpAction : BaseAction
    {
        private float _jumpDelay = 0f;
        
        private void Update()
        {
            if (_isActive == false) return;

            float spinAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAmount, 0);
            _jumpDelay += spinAmount;

            if (_jumpDelay >= 360f)
            {
                ActionComplete();
            }
        }

        public override void PerformAction(MapPosition mapPosition, Action onSpinComplete)
        {
            _jumpDelay = 0f;
            ActionStart(onSpinComplete);
        }

        public override string GetName()
        {
            return "J";
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            return new List<MapPosition>
            {
                _unit.CurrentMapPosition
            };
        }

        public override int GetActionPointCost()
        {
            return 2;
        }

        public override EnemyAction GetEnemyAction(MapPosition mapPosition)
        {
            return new EnemyAction
            {
                _mapPosition = mapPosition,
                ActionValue = 0,
            };
        }
    }
}