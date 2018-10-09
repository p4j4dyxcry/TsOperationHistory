using System;

namespace OperationSystem
{
    /// <summary>
    /// 識別子とタイムスタンプからマージ可能か判断する
    /// </summary>
    public class OperationMergeJudger<T> : IOperationMergeJudger
    {
        public T Key { get; }

        public TimeSpan Permission { get; set; } = TimeSpan.MaxValue;

        private DateTime TimeStamp { get; } = DateTime.Now;

        public bool CanMerge(IOperationMergeJudger operationMergeJudger)
        {
            if (operationMergeJudger is OperationMergeJudger<T> timeStampMergeInfo)
            {
                return Equals(Key, timeStampMergeInfo.Key) &&
                       TimeStamp - timeStampMergeInfo.TimeStamp < Permission;
            }
            return false;
        }

        public OperationMergeJudger(T key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
