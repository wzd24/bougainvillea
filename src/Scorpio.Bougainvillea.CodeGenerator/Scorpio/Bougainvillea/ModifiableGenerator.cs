using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public class ModifiableGenerator : ISourceGenerator
    {
        private const string _attributeText = @"
using System;
namespace Scorpio.Bougainvillea
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class ModifiableAttribute : Attribute
    {
        public ModifiableAttribute()
        {
        }
    }
}
";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Execute(GeneratorExecutionContext context) => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Created on demand before each generation pass
    /// </summary>
    class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<ITypeSymbol> Fields { get; } = new List<ITypeSymbol>();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if(context.Node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count>0)
            {
                classDeclarationSyntax
            }
            // any field with at least one attribute is a candidate for property generation
            if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
                && fieldDeclarationSyntax.AttributeLists.Count > 0)
            {
                foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                {
                    // Get the symbol being declared by the field, and keep it if its annotated
                    IFieldSymbol fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IPropertySymbol;
                    if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "Scorpio.Bougainvillea.ModifiableAttribute"))
                    {
                        Fields.Add(fieldSymbol);
                    }
                }
            }
        }
    }
}
