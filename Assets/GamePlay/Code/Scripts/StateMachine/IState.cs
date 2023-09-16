namespace GamePlay.Code.Scripts.StateMachine
{
    public interface IState
    {
        void OnEnter(Enemy enemy);

        void OnExcute(Enemy enemy);

        void OnExit(Enemy enemy);

    }
}