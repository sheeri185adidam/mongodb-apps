namespace Tests
{
        public abstract class Transaction
        {
            protected Transaction(long transactionId, long logSequence, 
                int gameId, int denomId, string gameName)
            {
                TransactionId = transactionId;
                LogSequence = logSequence;
                GameId = gameId;
                DenomId = denomId;
                GameName = gameName;
            }

            public long TransactionId {get; set;}

            public long LogSequence {get;private set;}

            public int GameId {get;private set;}

            public int DenomId{get;private set;}

            public string GameName {get;private set;}

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                
                // TODO: write your implementation of Equals() here
                var compare = (Transaction) obj;
                return TransactionId == compare.TransactionId
                        && LogSequence == compare.LogSequence
                        && GameId == compare.GameId
                        && DenomId == compare.DenomId
                        && GameName == compare.GameName;
            }
            
            // override object.GetHashCode
            public override int GetHashCode()
            {
                // TODO: write your implementation of GetHashCode() here
                throw new System.NotImplementedException();
            }
            public override string ToString()
            {
                return $"TransactionId: {TransactionId}\n" + 
                        $"LogSequence: {LogSequence}\n" +
                        $"GameId: {GameId}\n" +
                        $"DenomId: {DenomId}\n" +
                        $"GameName: {GameName}";
            }   
        }
}