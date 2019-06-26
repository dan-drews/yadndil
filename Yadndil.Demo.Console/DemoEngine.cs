using System;
using System.Collections.Generic;
using System.Text;

namespace Yadndil.Demo
{
    class DemoEngine : IDemoEngine
    {

        private IDemoHandler _demoHandler;
        public DemoEngine(IDemoHandler demoHandler)
        {
            _demoHandler = demoHandler;
        }

        public void DoStuffInEngine()
        {
            _demoHandler.DoWork2();
            Console.WriteLine("Hey! It worked!");
        }
    }
}
