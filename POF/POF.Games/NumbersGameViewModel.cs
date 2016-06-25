using System;
using GalaSoft.MvvmLight.Command;

namespace POF.Games
{
    public class NumbersGameViewModel
    {
        public RelayCommand CreateNewNumbers { get; set; }
        public RelayCommand FinishGameCommand { get; set; }
        public RelayCommand SubmitAnswerCommand { get; set; }

        private string givenNum;
        public string GivenNum
        {
            get { return givenNum; }
            set { givenNum = value; }
        }

        private string submittedNum;
        public string SubmittedNum
        {
            get { return submittedNum; }
            set { submittedNum = value; }
        }

        public NumbersGameViewModel()
        {
            CreateNewNumbers = new RelayCommand(generateNumbers);
            FinishGameCommand = new RelayCommand(finishGame, canFinishGame);
            SubmitAnswerCommand = new RelayCommand(submitAnswer);
        }

        private void submitAnswer()
        {
            throw new NotImplementedException();
        }

        private bool canFinishGame()
        {
            throw new NotImplementedException();
        }

        private void finishGame()
        {
            throw new NotImplementedException();
        }

        private void generateNumbers()
        {
            throw new NotImplementedException();
        }
    }
}