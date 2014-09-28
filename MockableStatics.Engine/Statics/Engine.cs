
namespace MockableStatics.Engine.Statics
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;

    using Microsoft.CSharp;

    using MockableStatics.Engine.Models;

    public static class Engine
    {
        public static IEnumerable<MockableType> GetStaticMethodsOfExecutingAssembly()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            var staticMethods = new List<MockableType>();

            foreach (var type in types)
            {
                staticMethods.Add(new MockableType 
                { 
                    Name = "MockableStatics." + type.FullName,
                    StaticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList() 
                });
            }

            return staticMethods;
        }

        public static IEnumerable<MockableType> GetStaticMembers(Assembly assembly)
        {
            var types = assembly.GetTypes().ToList();

            foreach (var type in types)
            {
                var staticMembers = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

                if (!staticMembers.Any())
                {
                    continue;
                }

                var mockableType = new MockableType { Name = type.Name, Namespace = "Ms." + type.Namespace, StaticMethods = staticMembers };

                yield return mockableType;
            }
        }

        public static void CreateSourceFile(string name, string sourceText, string path)
        {
            File.WriteAllText(Path.Combine(path + @"\" + sourceText), sourceText);            
        }

        public static IList<string> CreateAllSourceFiles(IList<string> name, IList<string> sourceTexts, string path)
        {
            var sourceFiles = new List<string>();

            for (int i = 0; i < name.Count; i++)
            {
                string fullPath = Path.Combine(path, name[i]);
                File.WriteAllText(fullPath, sourceTexts[i]);
                sourceFiles.Add(fullPath);
            }
            return sourceFiles;
        }

        public static IList<Assembly> LoadBaseDirectoryAssemblies(string path)
        {
            if (path == null)
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            var assemblies = new List<Assembly>();

            foreach (string dll in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFile(dll);
                    assemblies.Add(loadedAssembly);
                }
                catch (FileLoadException loadEx)
                { } // The Assembly has already been loaded.
                catch (BadImageFormatException imgEx)
                { } // The file is not an assembly.

            }

            return assemblies;
        }

        public static IEnumerable<string> BuildSourceFiles(IEnumerable<MockableType> mockableTypes)
        {
            foreach (var mockableType in mockableTypes)
            {
                // Namespace
                var compileUnit = new CodeCompileUnit();
                var @namespace = new CodeNamespace(mockableType.Namespace);
                compileUnit.Namespaces.Add(@namespace);

                // Class
                var @interface = new CodeTypeDeclaration(mockableType.Name) { IsInterface = true };

                // Members
                foreach (var m in mockableType.StaticMethods)
                {
                    var method = new CodeMemberMethod { Name = m.Name };
                    @interface.Members.Add(method);
                }

                @namespace.Types.Add(@interface);

                var codeProvider = new CSharpCodeProvider();

                var sb = new StringBuilder();
                var writer = new StringWriter(sb);

                codeProvider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());

                string sourceText = sb.ToString();
                yield return sourceText;
            }
        }

        public static bool CompileGeneratedSourceToAssembly(string [] sourceFiles, string outputAssembly)
        {
            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters { GenerateExecutable = false, OutputAssembly = outputAssembly };
            var results = codeProvider.CompileAssemblyFromFile(parameters, sourceFiles);
            return results.Errors.Count == 0;
        }
    }
}
