using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.游戏帮助;
using 俄罗斯方块.绘制对象基类和枚举信息;
using 俄罗斯方块.计分板;

namespace 俄罗斯方块.地图相关
{
    internal class Map : IDraw
    {
        //w是动态墙壁宽的容量
        public int w;
        public int h;
        ScoreBoard scoreboard;
        HelpInfo helpInfo;
        //记录每一行有多少个方块的容器
        //索引对应的就是行号
        private int[] recordInfo;

        private GameScene nowGameScene;

        //固定墙壁
        private List<DrawObject> walls = new List<DrawObject>();
        //动态墙壁
        public List<DrawObject> dynamicWalls = new List<DrawObject>();

        //重载一层无参构造，初始化固定墙壁
        public Map( GameScene scene ) 
        {
            this.nowGameScene = scene;
            scoreboard = new ScoreBoard();
            helpInfo = new HelpInfo();

            h = Game.h - 6;

            //这个就代表对应每行计数初始化 0 ~ (Game.h - 7)
            recordInfo = new int[h];

            w = 0;
            //绘制横向的固定墙壁
            for (int i = 0; i < Game.w;  i += 2)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, i, Game.h - 6));
                ++w;
            }
            w -= 2;

            for (int i = 0; i < Game.h - 6; i++)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, 0, i));
                walls.Add(new DrawObject(E_DrawType.Wall, Game.w - 2, i));
            }
        }

        public void Draw()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Draw();
            }
            //动态墙壁绘制，有才绘制
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].Draw();
            }
        }

        public void CleanDraw()
        {
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].ClearDraw();
            }
        }

        /// <summary>
        /// 提供给外部添加动态方块的函数
        /// </summary>
        /// <param name="walls"></param>
        public void AddWalls( List<DrawObject> walls )
        {
            for (int i = 0; i < walls.Count; i++)
            {
                //传递方块进来时 把其类型改成 墙壁类型
                walls[i].ChangeType(E_DrawType.Wall);
                dynamicWalls.Add(walls[i]);

                //在动态墙壁添加处 发现位置顶满 游戏就结束了
                if (walls[i].pos.y <= 0)
                {
                    //关闭结束线程
                    this.nowGameScene.StopThread();
                    //场景切换 切换到结束界面
                    Game.ChangeScene(E_SceneType.End);
                    return;
                }

                //添加动态墙壁的计数
                //根据索引来得到行 h 是 Game.h - 6
                //y最大为 Game.h - 7
                recordInfo[h - 1 - walls[i].pos.y] += 1;
            }
            CheckClear();
        }
        /// <summary>
        /// 检测是否填满一行
        /// </summary>
        public void CheckClear()
        {
            List<DrawObject> delList = new List<DrawObject>();
            for (int i = 0; i < recordInfo.Length; i++)
            {
                //必须满足条件才能证明满了
                //小方块-2
                if (recordInfo[i] == w )
                {
                    CleanDraw();

                    //这一行的所有小方块移除
                    for (int j = 0; j < dynamicWalls.Count; j++)
                    {
                        //当前通过动态方块的y计算它在哪一行 如果行号
                        //和当前记录索引一致 就证明 应该 移除
                        if(i == h - 1 - dynamicWalls[j].pos.y) 
                        {
                            //为了安全移除 添加一个要移除的方块的列表
                            delList.Add(dynamicWalls[j]);
                        }
                        //消除行上面的行下移一行
                        else if(h - 1 - dynamicWalls[j].pos.y > i)
                        {
                            ++dynamicWalls[j].pos.y;
                        }
                    //移除待删除的小方块
                    }
                    for (int j = 0; j < delList.Count; j++)
                    {
                        dynamicWalls.Remove(delList[j]);
                    }

                    //记录小方块的数量的数组从上到下迁移
                    for (int j = 0; j < recordInfo.Length - 1; j++)
                    {
                        recordInfo[j] = recordInfo[j + 1];
                    }
                    //置空最顶的计数
                    recordInfo[recordInfo.Length - 1] = 0;
                    scoreboard.ScorePlus();
                    Draw();
                    
                    //去掉一行后 再次重头检测是否还有满行
                    CheckClear();
                    break;
                }
            }
        }
    }
}
