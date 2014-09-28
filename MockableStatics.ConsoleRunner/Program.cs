namespace MockableStatics.ConsoleRunner
{
    using System;
    using System.Configuration;

    using MockableStatics.Engine.Concrete;
    using MockableStatics.Engine.Interfaces;

    class Program
    {
        static void Main(string[] args)
        {
            Run(ConfigurationManager.AppSettings["InPath"], 
                ConfigurationManager.AppSettings["OutPath"]);
            Console.ReadKey();
        }

        static void Run(string path, string @out)
        {
            IEngine engine = new EngineService();
            engine.Create(path, @out);
        }
    }
}
