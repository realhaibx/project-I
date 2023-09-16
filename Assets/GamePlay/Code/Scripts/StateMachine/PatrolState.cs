using System;
using UnityEngine;

namespace GamePlay.Code.Scripts.StateMachine
{
    public class PatrolState : IState
    {
        private float randomTimer;
        private float timer;
        public void OnEnter(Enemy enemy)
        {
            enemy.StopMoving();
            timer = 0;
            randomTimer = UnityEngine.Random.Range(3f, 6f);
        }

        public void OnExcute(Enemy enemy)
        {
            timer += Time.deltaTime;
            
            if (enemy.Target != null)
            {
                enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
                if (enemy.IsTargetInRange())
                {
                    enemy.ChangeState(new AttackState());
                }
                else
                {
                    enemy.Moving();
                }
            }
            else
            {
                if (timer < randomTimer)
                {
                    enemy.Moving();
                } else 
                {
                    enemy.ChangeState(new IdleState());
                }
            }
        }
        

        public void OnExit(Enemy enemy)
        {
            
        }
    }
}