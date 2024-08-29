    public enum ChallengeType
    {
        A,
        B,
    }
    
    public enum ChallengeStatus
    {
        NotStarted,
        InProgress,
        Completed,
    }
    

    public class ChallengeInfo
    {
        public string name;
        public ChallengeType type;
        public string reward;
        public ChallengeStatus status;
    }