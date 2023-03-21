using System;

namespace TheatricalPlayersRefactoringKata
{
    public class Performance
    {
        private string _playID;
        private int _audience;
        private int _amount;


        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public string PlayID
        {
            get => _playID;
            set => _playID = value;
        }

        public int Audience
        {
            get => _audience;
            set => _audience = value;
        }

        public Performance(string playID, int audience)
        {
            this._playID = playID;
            this._audience = audience;
        }
    }
}