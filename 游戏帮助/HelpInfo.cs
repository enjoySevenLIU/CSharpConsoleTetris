using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.绘制对象基类和枚举信息;

namespace 俄罗斯方块.游戏帮助
{
    internal class HelpInfo
    {
        public HelpInfo() 
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, Game.h - 4);
            Console.Write("操作方法：");
            Console.SetCursorPosition(0, Game.h - 3);
            Console.Write("A：向左移动，D：向右移动，S:加速向下");
            Console.SetCursorPosition(0, Game.h - 2);
            Console.Write("←：逆时针旋转，→：顺时针旋转");
        }
    }
}
