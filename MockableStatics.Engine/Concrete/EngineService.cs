namespace MockableStatics.Engine.Concrete
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Microsoft.CSharp;

    using MockableStatics.Engine.Interfaces;
    using MockableStatics.Engine.Statics;

    public class EngineService : IEngine
    {
        public void Create(string path, string @out)
        {
            var assemblies = Engine.LoadBaseDirectoryAssemblies(path);

            foreach (var assembly in assemblies)
            {
                var staticMembers = Engine.GetStaticMembers(assembly).ToList();

                if (!staticMembers.Any())
                {
                    continue;
                }

                var sourceFiles = Engine.BuildSourceFiles(staticMembers).ToList();

                var files = Engine.CreateAllSourceFiles(staticMembers.Select(x => x.Name + ".cs").ToList(), sourceFiles, @out);

                string assemblyPath = Path.Combine(
                    @out,
                    assembly.GetName().Name + ".dll");

                Engine.CompileGeneratedSourceToAssembly(files.ToArray(), assemblyPath);
            }
        }

        public void GenerateImplementation<T>(T t) where T : class
        {
            // Get all static members
            Type type = t.GetType();
            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);

            // Create wrapper for static members

            // Generate Source files

            // Generate Assemblies

            // Namespace
            var compileUnit = new CodeCompileUnit();
            var @namespace = new CodeNamespace(type.Namespace);
            compileUnit.Namespaces.Add(@namespace);

            // Class
            var @class = new CodeTypeDeclaration();

            // Members
            foreach (var m in members)
            {
                var method = new CodeMemberMethod { Name = m.Name };
                m.
                @class.Members.Add();
            }

            @namespace.Types.Add(@class);

            var codeProvider = new CSharpCodeProvider();

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            codeProvider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());

            string sourceText = sb.ToString();
        }
    }
}
