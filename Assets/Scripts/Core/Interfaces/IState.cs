public interface IState
{
    void OnStateEnter();
    void OnStateExit();
    void Tick();
    void FixedTick();
}