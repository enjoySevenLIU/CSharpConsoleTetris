using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块.输入模块
{
    internal class InputThread
    {
        //线程成员变量
        Thread inputThread;

        //输入检测事件
        public event Action inputEvent;

        private static InputThread instance = new InputThread();

        public static InputThread Instance
        {
            get
            {
                return instance;
            }
        }
        //创建并开启输入检测线程
        private InputThread()
        {
            //线程成员变量
            inputThread = new Thread(InputCheck);
            inputThread.IsBackground = true;
            inputThread.Start();
        }
        //输入检测线程
        private void InputCheck()
        {
            while (true)
            {
                //为空时，不会执行
                inputEvent?.Invoke();
            }
        }
    }
}
