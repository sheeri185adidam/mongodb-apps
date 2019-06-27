using System;
using System.Linq;

namespace Tests
{
    public class JackpotTransaction : Transaction
    {
        public JackpotTransaction(long transactionId, long jackpotId, long logSequence,
            int gameId, int denomId, string gameName)
            : base(transactionId, logSequence, gameId, denomId, gameName)
        {
            JackpotId = jackpotId;
        }

        public long JackpotId { get; private set; }
        public long JackpotValue { get; set; }
        public string JackpotPrizeText { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var compared = (JackpotTransaction) obj;
            return base.Equals(obj)
                    && JackpotId == compared.JackpotId
                    && JackpotValue == compared.JackpotValue
                    && JackpotPrizeText == compared.JackpotPrizeText;
        }
        
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString() + "\n" + $"JackpotId: {JackpotId}\n" +
                    $"JackpotValue: {JackpotValue}\n" +
                    $"JackpotPrizeText: {JackpotPrizeText}";
        }

        public static JackpotTransaction Randomize(Random random)
        {
            return new JackpotTransaction(
                random.Next(1, 10000),
                random.Next(1, 10000),
                random.Next(1, 10000),
                random.Next(1, 1000),
                random.Next(1, 10000),
                RandomString(30, random)
            )
            {
                JackpotValue = random.Next(1, 10000),
                JackpotPrizeText = RandomString(100, random)
            };

        }

        private static string RandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}