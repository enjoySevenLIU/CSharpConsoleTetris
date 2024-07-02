using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.地图相关;
using 俄罗斯方块.方块信息类;
using 俄罗斯方块.绘制对象基类和枚举信息;

namespace 俄罗斯方块.方块管理者
{
    enum E_ChangeType
    {
        Left,
        Right
    }

    internal class BlockWorker : IDraw
    {
        //方块们
        private List<DrawObject> blocks;
        //需要获取各个形状的方块信息是什么
        //选择一个容器来记录各个方块的形态信息
        private Dictionary<E_DrawType, BlockInfo> blockInfoDic;

        private BlockInfo nowBlockInfo;
        //当前砖块的形状
        private int nowInfoIndex;

        public BlockWorker()
        {
            //初始化 砖块信息
            blockInfoDic = new Dictionary<E_DrawType, BlockInfo>()
            {
                {E_DrawType.Cube, new BlockInfo(E_DrawType.Cube) },
                {E_DrawType.Line, new BlockInfo(E_DrawType.Line) },
                {E_DrawType.Tank, new BlockInfo(E_DrawType.Tank) },
                {E_DrawType.Left_Ladder, new BlockInfo(E_DrawType.Left_Ladder) },
                {E_DrawType.Right_Ladder, new BlockInfo(E_DrawType.Right_Ladder) },
                {E_DrawType.Left_Long_Ladder, new BlockInfo(E_DrawType.Left_Long_Ladder) },
                {E_DrawType.Right_Long_Ladder, new BlockInfo(E_DrawType.Right_Long_Ladder) },
            };
            RamdomCreateBlock();
        }

        public void Draw()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw();
            }
        }

        /// <summary>
        /// 随机创建一个方块
        /// </summary>
        public void RamdomCreateBlock()
        {
            //随机方块类型
            Random r = new Random();
            E_DrawType type = (E_DrawType)r.Next(1, 8);
            //每次新建一个 砖块 就是创建4个小方形
            blocks = new List<DrawObject>()
            {
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type)
            };

            //初始化方块位置
            //原点位置 我们随机 自己定义 方块list中第0个就是原点方块
            blocks[0].pos = new Position(24, -5);

            nowBlockInfo = blockInfoDic[type];
            //随机几种形态的一种来设置方块的消息
            nowInfoIndex = r.Next(0, nowBlockInfo.Count);
            //取出其中一种形态的坐标信息
            Position[] pos = nowBlockInfo[nowInfoIndex];
            for (int i = 0; i < pos.Length; i++)
            {
                //取出来的pos是相对原点方块的坐标，所以需要进行计算
                blocks[i + 1].pos = blocks[0].pos + pos[i];
            }

        }

        public void ClearDraw()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].ClearDraw();
            }
        }

        public void Charge(E_ChangeType type)
        {
            //变之前先擦除
            ClearDraw();

            switch (type)
            {
                case E_ChangeType.Left:
                    --nowInfoIndex;
                    if (nowInfoIndex < 0)
                    {
                        nowInfoIndex = nowBlockInfo.Count - 1;
                    }
                    break;
                case E_ChangeType.Right:
                    ++nowInfoIndex;
                    if (nowInfoIndex >= nowBlockInfo.Count)
                    {
                        nowInfoIndex = 0;
                    }
                    break;
            }
            //得到索引 是为了得到对应形态的 位置偏移信息
            Position[] pos = nowBlockInfo[nowInfoIndex];
            for (int i = 0; i < pos.Length; i++)
            {
                //取出来的pos是相对原点方块的坐标，所以需要进行计算
                blocks[i + 1].pos = blocks[0].pos + pos[i];
            }

            Draw();
        }

        public bool CanCharge(E_ChangeType type, Map map)
        {
            int nowIndex = nowInfoIndex;

            switch (type)
            {
                case E_ChangeType.Left:
                    --nowIndex;
                    if (nowIndex < 0)
                    {
                        nowIndex = nowBlockInfo.Count - 1;
                    }
                    break;
                case E_ChangeType.Right:
                    ++nowIndex;
                    if (nowIndex >= nowBlockInfo.Count)
                    {
                        nowIndex = 0;
                    }
                    break;
            }
            Position[] nowPos = nowBlockInfo[nowIndex];
            //首先判断是否超出边界
            Position tempPos;
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                if( tempPos.x < 2 ||
                    tempPos.x >= Game.w - 2 ||
                    tempPos.y >= map.h )
                {
                    return false;
                }
            }
            //再判断是否和动态砖块重合
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if(tempPos == map.dynamicWalls[j].pos)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void MoveRL(E_ChangeType type)
        {
            ClearDraw();
            Position movePos = new Position(type == E_ChangeType.Left ? -2 : 2, 0);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].pos += movePos;
            }
            Draw();
        }

        public bool CanMoveRL(E_ChangeType type, Map map)
        {
            Position movePos = new Position(type == E_ChangeType.Left ? -2 : 2, 0);
            //首先判断是否超出边界
            Position pos;
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                if( pos.x < 2 || pos.x >= Game.w - 2)
                {
                    return false;
                }
            }

            //再判断是否和动态砖块重合
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if( pos == map.dynamicWalls[j].pos)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void AutoMove()
        {
            ClearDraw();
            Position downMove = new Position(0, 1);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].pos += downMove;              
            }
            Draw();
        }

        public bool CanMove(Map map)
        {
            Position movePos = new Position(0, 1);
            Position pos;
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                if ( pos.y >= map.h)
                {
                    //不仅要停下来，还要变成动态方块
                    map.AddWalls(blocks);
                    RamdomCreateBlock();
                    return false;
                }
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (pos == map.dynamicWalls[j].pos)
                    {
                        //不仅要停下来，还要变成动态方块
                        map.AddWalls(blocks);
                        RamdomCreateBlock();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
