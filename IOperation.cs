namespace OperationSystem
{
    public interface IOperation
    {
        void Execute();
        void Rollback();
    }
}
