namespace Model.Agents.States
{
    public interface IState
    {
        void action();
        IState next();
    }
}