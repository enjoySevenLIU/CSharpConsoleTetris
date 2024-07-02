using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.地图相关;
using 俄罗斯方块.方块管理者;
using 俄罗斯方块.计分板;
using 俄罗斯方块.输入模块;

namespace 俄罗斯方块
{
    internal class GameScene : ISceneUpdate
    {
        Map map;
        BlockWorker blockworker;

        //bool isRunning;
        //Thread inputThread;

        public GameScene()
        {
            map = new Map(this);
            blockworker = new BlockWorker();

            InputThread.Instance.inputEvent += CheckInputThread;
            //旧输入方法
            //isRunning = true;
            //inputThread = new Thread(CheckInputThread);
            //inputThread.IsBackground = true;
            //inputThread.Start();
        }

        private void CheckInputThread()
        {
            //while (isRunning)
            //{
            //这只是 另一个输入线程 每帧会执行的逻辑 不需要自己来死循环
                if (Console.KeyAvailable)
                {
                    lock (blockworker)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.LeftArrow:
                                if (blockworker.CanCharge(E_ChangeType.Left, map))
                                {
                                    blockworker.Charge(E_ChangeType.Left);
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                if (blockworker.CanCharge(E_ChangeType.Right, map))
                                {
                                    blockworker.Charge(E_ChangeType.Right);
                                }
                                break;
                            case ConsoleKey.A:
                                if (blockworker.CanMoveRL(E_ChangeType.Left, map))
                                {
                                    blockworker.MoveRL(E_ChangeType.Left);
                                }
                                break;
                            case ConsoleKey.D:
                                if (blockworker.CanMoveRL(E_ChangeType.Right, map))
                                {
                                    blockworker.MoveRL(E_ChangeType.Right);
                                }
                                break;
                            case ConsoleKey.S:
                                if (blockworker.CanMove(map))
                                {
                                    blockworker.AutoMove();
                                }
                                break;
                        }
                    }
                }
            //}
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void StopThread()
        {
            //isRunning = false;
            //inputThread = null;

            InputThread.Instance.inputEvent -= CheckInputThread;
        }

        public void Update()
        {
            //锁里面不要包含线程休眠
            lock (blockworker)
            {
                map.Draw();

                blockworker.Draw();

                if (blockworker.CanMove(map))
                {
                    blockworker.AutoMove();
                }
            }
            Thread.Sleep(200);
        }
    }
}
