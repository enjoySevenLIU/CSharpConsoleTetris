using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块
{
    /// <summary>
    /// 场景类型枚举
    /// </summary>
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景
        /// </summary>
        Begin,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 结束场景
        /// </summary>
        End,
    }

    internal class Game
    {
        public const int w = 50;
        public const int h = 35;
        //游戏场景
        public static ISceneUpdate nowScene;

        public Game()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(w, h);
            Console.SetBufferSize(w, h);
            ChangeScene(E_SceneType.Begin);
        }

        public void Start()
        {
            //游戏主循环 主要负责游戏场景的更新
            while (true)
            {
                
                //判断当前游戏场景不为空 就更新
                if (nowScene != null)
                {
                    nowScene.Update();
                }
            }
        }

        public static void ChangeScene(E_SceneType type)
        {
            //切场景之前 应该把上一个场景的绘制内容去掉
            Console.Clear();

            switch(type)
            {
                case E_SceneType.Begin:
                    nowScene = new BeginScene();
                    break;
                case E_SceneType.Game:
                    nowScene = new GameScene();
                    break;
                case E_SceneType.End:
                    nowScene = new EndScene();
                    break;
            }
        }
    }
}
