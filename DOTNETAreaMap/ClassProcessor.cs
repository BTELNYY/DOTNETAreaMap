using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNETAreaMap
{
    public class ClassProcessor : IProcessor
    {
        public TypeLink[] GetReferencedTypes()
        {
            throw new NotImplementedException();
        }

        public void Process(Type input, StreamWriter writer)
        {
            string sanitze = input.Name.SanitizeForUML();
            writer.Write($"class {sanitze}{{\n");
            foreach (var property in input.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static))
            {

            }
            foreach (var field in input.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static))
            {
                writer.Write($"\t\t{field.ToUMLAccessor()}{field.Name.SanitizeForUML()}: {field.FieldType.Name.SanitizeForUML()}\n");
            }
            foreach (var method in input.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static))
            {
                string methodAddin = $"\t\t{method.ToUMLAccessor()}{method.Name.SanitizeForUML()}({string.Join(", ", method.GetParameters().Select(x => $"{x.Name.SanitizeForUML()}:{x.ParameterType.Name.SanitizeForUML()}"))}){method.ReturnType.Name.SanitizeForUML()}\n";
                writer.Write(methodAddin);
            }
            writer.Write("}\n");
        }
    }
}
