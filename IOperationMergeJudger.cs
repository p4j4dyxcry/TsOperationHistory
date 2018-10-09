namespace OperationSystem
{
    public interface IOperationMergeJudger
    {
        /// <summary>
        /// マージ可能か判断します。
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        bool CanMerge(IOperationMergeJudger operation);
    }
}
