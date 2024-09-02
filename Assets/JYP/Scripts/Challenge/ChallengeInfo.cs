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
        
        public ChallengeInfo(string name, ChallengeType type, string reward, ChallengeStatus status)
        {
            this.name = name;
            this.type = type;
            this.reward = reward;
            this.status = status;
        }
    }