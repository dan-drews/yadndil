using System;
using System.Collections.Generic;
using System.Text;

namespace Yadndil.Demo
{
    class DemoManager : IDemoManager
    {
        private IDemoHandler _demoHandler;
        private IDemoEngine _demoEngine;
        public DemoManager(IDemoEngine demoEngine, IDemoHandler demoHandler)
        {
            _demoEngine = demoEngine;
            _demoHandler = demoHandler;
        }

        public void DoStuffInManager()
        {
            _demoEngine.DoStuffInEngine();
            _demoHandler.DoWork();
        }
    }
}
