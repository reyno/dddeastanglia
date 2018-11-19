using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Generator {
    public class MediatRInterfaceGenerator {

        private Assembly _assembly;
        private string _output;
        private FileStream _file;
        private StreamWriter _writer;

        public MediatRInterfaceGenerator(Assembly assembly, string output) {
            _assembly = assembly;
            _output = output;
        }

        public static void Generate(Assembly assembly, string output) {

            new MediatRInterfaceGenerator(assembly, output).GenerateInternal();

        }

        private void GenerateInternal() {

            _file = File.OpenWrite(_output);
            _writer = new StreamWriter(_file);

            try {

                    _writer.WriteLine("interface IMediator {");

                    foreach (var request in GetAllRequests())
                        ProcessRequest(request);

                _writer.WriteLine("}");

            } finally {
                _writer?.Close();
                _writer?.Dispose();
                _file?.Close();
                _file?.Dispose();
            }

        }

        private IEnumerable<Type> GetAllRequests()
            => from t in _assembly.DefinedTypes
               where typeof(IBaseRequest).IsAssignableFrom(t)
               select t;

        private void ProcessRequest(Type requestType) {

            var requestName = GetRequestName(requestType);

            var responseType = GetResponseType(requestType);

            _writer.WriteLine($"    send(command: '{requestName}', request: {GetTypescriptDefinitionForType(requestType)}): Promise<{GetTypescriptDefinitionForType(responseType)}>;");
            //_writer.WriteLine($"    subscribe(command: '{requestName}', callback: (request: {GetTypescriptDefinitionForType(requestType)}, response: {GetTypescriptDefinitionForType(responseType)}) => void): IMediatRSubscription;");

        }

        private string GetTypescriptDefinitionForType(Type requestType) {

            if (Type.GetTypeCode(requestType) != TypeCode.Object) return GetTypescriptType(requestType);

            var stringBuilder = new StringBuilder();
            var properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties) {

                var name = string.Concat(
                    property.Name.Substring(0, 1).ToLower(),
                    property.Name.Substring(1)
                    );
                var type = GetTypescriptType(property.PropertyType);


                stringBuilder.Append($"{name}: {type}; ");

            }
            return stringBuilder.Length == 0 ? "void" : $"{{{stringBuilder.ToString()}}}";
        }

        private string GetTypescriptType(Type type) {

            switch (Type.GetTypeCode(type)) {
                case TypeCode.String:
                    return "string";
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "number";
                default:
                    if (type == typeof(Guid)) return "string";
                    if (type == typeof(DateTime) || type == typeof(DateTimeOffset)) return "Date";
                    else return "any";
            }



        }

        private string GetRequestName(Type type) {

            var baseNamespace = "DDDEastAnglia.Api.MediatR.Requests";

            var strippedFullname = type.FullName.StartsWith(baseNamespace)
                ? type.FullName.Substring(baseNamespace.Length)
                : type.FullName;

            var truncatedFullName = strippedFullname.EndsWith("Request")
                ? strippedFullname.Substring(0, strippedFullname.Length - "Request".Length)
                : strippedFullname
                ;

            // camel case
            return string.Join(
                ".",
                truncatedFullName
                    .Split('.', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => string.Concat(s.Substring(0, 1).ToLower(), s.Substring(1))
                    )
                );


        }

        private Type GetResponseType(Type requestType) {
            
            return requestType
                .GetTypeInfo()
                .ImplementedInterfaces
                .First(x => x.IsGenericType)
                .GenericTypeArguments
                .Single()
                ;

        }



    }
}