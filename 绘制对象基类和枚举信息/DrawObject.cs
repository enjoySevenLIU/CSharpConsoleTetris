﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块.绘制对象基类和枚举信息
{
    /// <summary>
    /// 绘制类型 根据不同类型 改变绘制的方块的颜色
    /// </summary>
    enum E_DrawType 
    {
        /// <summary>
        /// 墙壁
        /// </summary>
        Wall,
        /// <summary>
        /// 方块
        /// </summary>
        Cube,
        /// <summary>
        /// 直线
        /// </summary>
        Line,
        /// <summary>
        /// 坦克
        /// </summary>
        Tank,
        /// <summary>
        /// 左梯子
        /// </summary>
        Left_Ladder,
        /// <summary>
        /// 右梯子
        /// </summary>
        Right_Ladder,
        /// <summary>
        /// 左长梯子
        /// </summary>
        Left_Long_Ladder,
        /// <summary>
        /// 右长梯子
        /// </summary>
        Right_Long_Ladder,
    }

    internal class DrawObject : IDraw
    {
        public Position pos;
        public E_DrawType type;

        public DrawObject(E_DrawType type)
        {
            this.type = type;
        }

        public DrawObject(E_DrawType type, int x, int y):this(type)
        {
            this.pos = new Position(x, y);
        }

        public E_DrawType GetType()
        {
            return type;
        }

        public void Draw()
        {
            //不渲染屏幕外的方块
            if (pos.y < 0)
            {
                return;
            }

            Console.SetCursorPosition(pos.x, pos.y);
            switch (type)
            {
                case E_DrawType.Wall:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case E_DrawType.Cube:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case E_DrawType.Line:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case E_DrawType.Tank:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case E_DrawType.Left_Ladder:
                case E_DrawType.Right_Ladder:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case E_DrawType.Left_Long_Ladder:
                case E_DrawType.Right_Long_Ladder:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.Write("■");
        }

        public void ClearDraw()
        {
            //不渲染屏幕外的方块
            if (pos.y < 0)
            {
                return;
            }
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write("  ");
        }

        /// <summary>
        /// 这是切换方块类型 主要用于搬砖下落到地图时 把搬砖类型变成墙壁类型
        /// </summary>
        /// <param name="type"></param>
        public void ChangeType(E_DrawType type)
        {
            this.type = type;
        }
    }
}
