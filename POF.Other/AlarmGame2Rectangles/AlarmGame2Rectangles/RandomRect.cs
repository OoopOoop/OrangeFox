using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGame2Rectangles
{
    class RandomRect
    {
        public string RecBlueOne { get; set; }
        public string RecBlueTwo { get; set; }
        public string RecBlueThree { get; set; }


        public Dictionary<int, string> Rectangles = new Dictionary<int, string>()
        {
            {1, "rec1"},
            {2, "rec2"},
            {3, "rec3"},
            {4, "rec4"},
            {5, "rec5"},
            {6, "rec6"},
            {7, "rec7"},
            {8, "rec8"},
            {9, "rec9"},
        };


        public void chooseRect()
        {
            Random randomStoryBoard = new Random();

            int[] randomNumbers = new int[3];


            for (int i = 0; i < randomNumbers.Length; i++)
            {
                int randomNumber;

                do
                {
                    randomNumber = randomStoryBoard.Next(1, 9);
                }
                while (randomNumbers.Contains(randomNumber));


                randomNumbers[i] = randomNumber;
            }


            RecBlueOne = Rectangles[randomNumbers[0]];
            RecBlueTwo = Rectangles[randomNumbers[1]];
            RecBlueThree = Rectangles[randomNumbers[2]];
        }
    }
}
