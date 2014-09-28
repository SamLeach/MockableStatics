namespace MockableStatics.Engine.Tests
{
    using System.IO;
    using System.Linq;

    using MockableStatics.Engine.Concrete;
    using MockableStatics.Engine.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    public class EngineIntegrationTests
    {
        [Test]
        public void Engine_Create_SourceFilesAndAssemblyCreatedOnDisk()
        {
            // Arrange
            const string PathInput = "C:\\PathInput";
            const string PathOutput = "C:\\PathOutput";
            IEngine engine = new EngineService();

            // Make sure the input path has some test assemblies before we start
            Assert.True(Directory.GetFiles(PathInput).Any());

            // Act
            engine.Create(PathInput, PathOutput);

            // Assert
            var files = Directory.GetFiles(PathOutput).ToList();
            Assert.True(files.Count == 4);
            Assert.True(files.Contains(Path.Combine(PathOutput, "Engine.cs")));
            Assert.True(files.Contains(Path.Combine(PathOutput, "MockableStatics.Engine.dll")));
            Assert.True(files.Contains(Path.Combine(PathOutput, "MockableStatics.StaticMethod.dll")));
            Assert.True(files.Contains(Path.Combine(PathOutput, "StaticClass.cs")));

            //
            // Clean-up
            this.DeleteIfExists(Path.Combine(PathOutput, "Engine.cs"));
            this.DeleteIfExists(Path.Combine(PathOutput, "MockableStatics.Engine.dll"));
            this.DeleteIfExists(Path.Combine(PathOutput, "MockableStatics.StaticMethod.dll"));
            this.DeleteIfExists(Path.Combine(PathOutput, "StaticClass.cs"));
            Assert.False(Directory.GetFiles(PathOutput).Any());
        }

        private void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
