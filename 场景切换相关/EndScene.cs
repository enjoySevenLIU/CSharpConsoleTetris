using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块
{
    internal class EndScene : BeginOrEndBaseScene
    {
        public EndScene()
        {
            strTitle = "俄罗斯方块";
            strOne = "回到开始界面";
        }
        public override void EnterJDoSomething()
        {
            if (nowSelIndex == 0)
            {
                Game.ChangeScene(E_SceneType.Begin);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
