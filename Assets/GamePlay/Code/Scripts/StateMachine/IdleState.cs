using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Code.Scripts.StateMachine
{
    public class IdleState : IState
    {
        private float randomTimer;
        private float timer;
        public void OnEnter(Enemy enemy)
        {
            enemy.StopMoving();
            timer = 0;
            randomTimer = Random.Range(2f, 4f);
        }

        public void OnExcute(Enemy enemy)
        {
            if (timer > randomTimer)
            {
                enemy.ChangeState(new PatrolState());
            }

            timer += Time.deltaTime;

        }

        public void OnExit(Enemy enemy)
        {
            
        }
    }
}