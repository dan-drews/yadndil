using System;

namespace Yadndil.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Container();
            c.Register<IDemoManager, DemoManager>();
            c.Register<IDemoEngine, DemoEngine>();
            c.Register<IDemoHandler, DemoHandler>();

            var manager = c.Get<IDemoManager>();
            manager.DoStuffInManager();
            
            Console.ReadLine();
        }
    }
}
