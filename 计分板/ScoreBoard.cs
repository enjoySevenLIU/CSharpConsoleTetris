using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.绘制对象基类和枚举信息;

namespace 俄罗斯方块.计分板
{
    internal class ScoreBoard : IDraw
    {
        private int score;
        private Position pos;

        public ScoreBoard() 
        {
            score = 0;
            pos = new Position(0, Game.h - 5);
            Draw();
        }

        public void ScorePlus()
        {
            score++;
            Draw();
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("本次游戏你的分数是：{0}", score);
        }
    }
}
