using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class MeleeBehavior : EnemyBehavior
    {
        public override BaseAction ProcessAction(EnemyUnit enemyUnit, ref MapPosition targetPosition)
        {
            BaseAction bestAction;
            List<MapPosition> targetPositions = enemyUnit.GetAction<MeleeAttackAction>().GetValidActionGridPositions();

            if (targetPositions.Count > 0)
            {
                bestAction = enemyUnit.GetAction<MeleeAttackAction>();
                targetPosition = targetPositions[0];
            }
            else
            {
                bestAction = enemyUnit.GetAction<MoveAction>();
                targetPositions = enemyUnit.GetAction<MoveAction>().GetValidActionGridPositions();
                int highestTargetCount = 0;
                foreach (MapPosition mapPosition in targetPositions)
                {
                    int targetCount = enemyUnit.GetAction<MeleeAttackAction>().GetTargetCountAtPosition(mapPosition);

                    if (targetCount > highestTargetCount)
                    {
                        highestTargetCount = targetCount;
                        targetPosition = mapPosition;
                    }
                }
            }

            return bestAction;
        }
    }
}