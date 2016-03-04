using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;

namespace AlarmGame2Rectangles
{
    public static class ChangeRecProperty
    {
        public static void changeColor(Storyboard storyboard, Color color)
        {

            ColorAnimationUsingKeyFrames colorValue = storyboard.Children[1] as ColorAnimationUsingKeyFrames;

            ColorKeyFrameCollection collection = colorValue.KeyFrames;

            collection[1].Value = color;

        }
    }
}
