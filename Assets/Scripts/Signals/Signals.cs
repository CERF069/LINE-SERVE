namespace Signals
{
    public class BallReachedTargetSignal
    {
        public TargetType TargetType { get; }

        public BallReachedTargetSignal(TargetType targetType)
        {
            TargetType = targetType;
        }
    }
} 
public enum TargetType
{
    Player,
    Enemy
}
