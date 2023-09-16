using UnityEngine;

namespace GamePlay.Code.Scripts.StateMachine
{
    public class AttackState : IState
    {
        private float timer;
        public void OnEnter(Enemy enemy)
        {
            if (enemy.Target != null)
            {
                enemy.ChangeDirection((enemy.Target.transform.position.x > enemy.transform.position.x && enemy.transform.rotation.y == 0) || (enemy.Target.transform.position.x < enemy.transform.position.x && enemy.transform.rotation.y != 0));
                enemy.StopMoving();
                enemy.Attack();
            }
        }

        public void OnExcute(Enemy enemy)
        {
            timer += Time.deltaTime;

            if (timer >= 1.5f)
            {
                enemy.ChangeState(new PatrolState());
            }
        }

        public void OnExit(Enemy enemy)
        {
        }
    }
}