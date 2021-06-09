using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.ViewModel
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public enum GameSpeedEnum
    {
        Easy = 300,
        Normal = 230,
        Hard = 180,
        Expert = 130,
    }

    public enum BoardSizeEnum
    {
        Small = 12,
        Medium = 16,
        Large = 24
    }

}
