namespace MockableStatics.Engine.Tests
{
    using MockableStatics.Engine.Concrete;
    using MockableStatics.StaticMethod;

    using NUnit.Framework;

    [TestFixture]
    public class EngineTests
    {
   
        [Test]
        public void Go()
        {
            var engine = new EngineService();
            var f = engine.GenerateImplementation(typeof(StaticClass));
        }
    }
}
