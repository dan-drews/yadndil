using System;
using System.Collections.Generic;
using System.Text;

namespace Yadndil.Demo
{
    class DemoManager : IDemoManager
    {
        private IDemoEngine _demoEngine;
        public DemoManager(IDemoEngine demoEngine)
        {
            _demoEngine = demoEngine;
        }

        public void DoStuffInManager()
        {
            _demoEngine.DoStuffInEngine();
        }
    }
}
