using System;
using System.Collections.Generic;
using System.Text;

namespace Yadndil.Demo
{
    class DemoHandler : IDemoHandler
    {
        public void DoWork()
        {
            Console.WriteLine("Hi Aaron Deming!");
        }

        public void DoWork2()
        {
            Console.WriteLine("Hi Aaron Perkins!");
        }
    }
}
