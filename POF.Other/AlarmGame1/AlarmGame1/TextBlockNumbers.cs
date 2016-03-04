using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmGame1
{
    public static class TextBlockNumbers
    {
        public static string getRandomNumber()
        {
            Random random = new Random();
            string text = "";

            string[] numbers = new string[4];

            foreach (var item in numbers)
            {
                text += random.Next(0, 9).ToString();
            }

            return text;
        }


        public static bool compare(string valueTextbox, string valueInput)
        {
            return valueTextbox == valueInput ? true : false;
        }
    }
}
