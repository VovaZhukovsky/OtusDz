using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator
{
    
    [Generator]
    public class SerializerGenerator: IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
                "ZeroAllocCore.GenerateBinarySerializerAttribute",
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, _) => (INamedTypeSymbol)ctx.TargetSymbol);
            
            context.RegisterSourceOutput(candidates,Generate);
        }
        
        private static void Generate(
            SourceProductionContext context,
            INamedTypeSymbol symbol)
        {
            var source = $$$"""
                            using System.Text;
                            namespace {{{symbol.ContainingNamespace}}}
                            {
                                public partial class {{{symbol.Name}}}
                                {
                                    public byte[] SerializeToBinary()
                                    {
                                        using var stream =  new MemoryStream();
                                        using var writer = new BinaryWriter(stream);
                                        
                                        writer.Write(Id);
                                        writer.Write(Created.ToLongTimeString());
                                        if(Name is not null) 
                                            writer.Write(Name);
                                        writer.Flush();
                                        return stream.ToArray();
                                    }
                                    public static UserProfile DeserializeFromBinary(byte[]? value)
                                    {
                                        using var stream =  new MemoryStream(value);
                                        using var reader = new BinaryReader(stream);
                                        
                                        var userProfile = new UserProfile
                                        {
                                            Id = reader.ReadInt32(),
                                            Created = DateTime.Parse(reader.ReadString())
                                        };
                                        var name = reader.ReadString();
                                        
                                        if(!string.IsNullOrEmpty(name))
                                            userProfile.Name = name;
                                        
                                        return userProfile;
                                    }
                                }
                            }
                            """;

            context.AddSource($"{symbol.Name}.g.cs", source);
        }
    }
}

