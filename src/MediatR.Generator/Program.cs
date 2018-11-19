using DDDEastAnglia.Api;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediatR.Generator {
    internal class Program {
        private static void Main(string[] args) {

            var assembly = typeof(Startup).Assembly;

            var output = $@"C:\Users\RussellSeamer\Source\repos\github\reyno\dddeastanglia\src\ui\src\types.ts";

            MediatRInterfaceGenerator.Generate(assembly, output);

            Console.WriteLine("Interface generated");

            
        }

    }
}