using System;
using System.IO;
using System.Linq;
using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Obs.Utils;

namespace LesoftWuye2.CodeGenerator
{
    class Program
    {
        const string NAMESPACE_NAME = "LesoftWuye2";

        static void Main(string[] args)
        {
            using (var bootstrapper = AbpBootstrapper.Create<LesoftWuye2CodeGeneratorModule>())
            {
                bootstrapper.IocManager.IocContainer
                    .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                        .WithConfig("log4net.config")
                    );

                bootstrapper.Initialize();

                

                using (var generator = bootstrapper.IocManager.ResolveAsDisposable<Generator>())
                {
                    string generatorDir = NAMESPACE_NAME + @".CodeGenerator\bin\Debug\";
                    string solutionPath = AppDomain.CurrentDomain.BaseDirectory.Replace(generatorDir, "");
                    string entityProjectPath = Path.Combine(solutionPath, NAMESPACE_NAME+@".Core\bin\Debug\", NAMESPACE_NAME+".Core.dll");
                    string appProjectPath = Path.Combine(solutionPath, NAMESPACE_NAME + @".Application");
                    string webProjectPath = Path.Combine(solutionPath, NAMESPACE_NAME + @".Web");
                    string entityAssemblyName = NAMESPACE_NAME + ".Core";

                    var entitys= generator.Object.GetEntities(entityAssemblyName, appProjectPath);
                    if (entitys.Any(t => t.IsExists==false))
                    {
                        Console.WriteLine("Whether to continue generating code(Y/N):");
                        if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Y")
                        {
                            generator.Object.GenerateCode(entitys.Where(t=>t.IsExists==false).ToList(), appProjectPath, webProjectPath,NAMESPACE_NAME);
                            Console.WriteLine("The code generation is complete.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No entities were found that required code generation");   
                    }
                }

                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
