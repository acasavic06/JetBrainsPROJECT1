using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Console;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (!File.Exists("input.cs"))
        {
            WriteLine("Error: input.cs not found");
            return;
        }

        // read the input file
        var code = File.ReadAllText("input.cs");
        
        // parse the syntax tree and get the root 
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();
        
        var newMembers = new List<MemberDeclarationSyntax>();
        
        // we will go through namespaces->classes->methods
        foreach (var member in root.Members)
        {
            if (member is NamespaceDeclarationSyntax ns)
            {
                var newNamespaceMembers = new List<MemberDeclarationSyntax>();
                
                // loop through all class declarations inside the namespace
                foreach (var classMember in ns.Members)
                {
                    if (classMember is ClassDeclarationSyntax cd)
                    {
                        var newClassMembers = new List<MemberDeclarationSyntax>();

                        // loop through all members of the class
                        foreach (var subMember in cd.Members)
                        {
                            if (subMember is MethodDeclarationSyntax method)
                            {
                                // if method has exactly one parameter
                                // add 1 new parameter with same type as parameter that already is in method
                                if (method.ParameterList.Parameters.Count == 1)
                                {
                                    var param= method.ParameterList.Parameters.First();
                                    //name of new parameter is the name of existing one + "1" at the end
                                    var newParam= param.WithIdentifier(SyntaxFactory.Identifier(param.Identifier.Text + "1"));
                                
                                    var newParams = method.ParameterList.AddParameters(newParam);
                                    method = method.WithParameterList(newParams);
                                }

                                // check if there are any local functions with one parameter inside a method
                                var localFuncs = method.DescendantNodes()
                                    .OfType<LocalFunctionStatementSyntax>()
                                    .Where(f => f.ParameterList.Parameters.Count == 1)
                                    .ToList();
                                var newBody = method.Body;
                                
                                // loop through all local functions and duplicate their parameter
                                foreach (var func in localFuncs)
                                {
                                    var param = func.ParameterList.Parameters.First();
                                    var newParam = param.WithIdentifier(
                                        SyntaxFactory.Identifier(param.Identifier.Text + "1"));
                                    var newParams = func.ParameterList.AddParameters(newParam);
                                    var newFunc = func.WithParameterList(newParams);
                                    newBody = newBody.ReplaceNode(func, newFunc);
                                }

                                // replace method body if any local functions were changed
                                if (newBody != method.Body)
                                    method = method.WithBody(newBody);

                                // add the modified method to the new class
                                newClassMembers.Add(method);
                            }
                            else
                            {
                                // copy non-method members
                                newClassMembers.Add(subMember);
                            }
                        }
                        // replace old class with the updated one
                        var newClass=cd.WithMembers(SyntaxFactory.List(newClassMembers));
                        newNamespaceMembers.Add(newClass);
                    }
                    else
                    {
                        // copy non-class members 
                        newNamespaceMembers.Add(classMember);
                    }
                }

                // replace old namespace with the updated one
                var newNamespace = ns.WithMembers(SyntaxFactory.List(newNamespaceMembers));
                newMembers.Add(newNamespace);
            }
            else
            {
                newMembers.Add(member);
            }
        }
        // write the transformed syntax tree in the output file
        var newRoot = root.WithMembers(SyntaxFactory.List(newMembers));
        File.WriteAllText("output.cs",newRoot.ToFullString());
    }
}



/*
class Program
{
    static void Main(string[] args)
    {
        var code = File.ReadAllText("input.cs");
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot();

        var methods = from method in root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
            where method.ParameterList.Parameters.Count == 1
            select method;

        var newRoot = root;

        foreach (var method in methods)
        {
            var currMethod = newRoot.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First(m => m.Identifier.Text == method.Identifier.Text);

            var original = currMethod.ParameterList.Parameters.First();
            var newName = SyntaxFactory.Identifier(original.Identifier.Text + "1");
            var newParameter = original.WithIdentifier(newName);
            
            var newList = currMethod.ParameterList.AddParameters(newParameter);
            var newMethod = currMethod.WithParameterList(newList);
            
            newRoot = newRoot.ReplaceNode(currMethod, newMethod);
        }
        
        File.WriteAllText("output.cs",newRoot.ToFullString());
    }
}
*/
/*
const string programText =
    @"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program2
    {
        static void Main(string[] args)
        {
            int a=5;
            Console.WriteLine(""Hello, World!"");
            Console.Writeline(a);
        }
    }
    int x=5;  

    class Program3
    {
        static void Main(string [] args){
            int x=3;Console.Writeline(x);
        }
    }
}";

SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
//var root = tree.GetRoot();

WriteLine($"The tree is a {root.Kind()} node.");
WriteLine($"The tree has {root.Members.Count} elements in it.");
WriteLine($"The tree has {root.Usings.Count} using directives. They are:");
foreach (UsingDirectiveSyntax element in root.Usings)
    WriteLine($"\t{element.Name}");

MemberDeclarationSyntax firstMember = root.Members[0];
WriteLine($"The first member is a {firstMember.Kind()}.");
var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
WriteLine($"There are {programDeclaration.Members.Count} members declared in the {programDeclaration.Identifier} class.");
WriteLine($"The first member is a {programDeclaration.Members[0].Kind()}.");
WriteLine($"The second member is a {programDeclaration.Members[1].Kind()}.");


var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
WriteLine($"The return type of the {mainDeclaration.Identifier} method is {mainDeclaration.ReturnType}.");
WriteLine($"The method has {mainDeclaration.ParameterList.Parameters.Count} parameters.");
foreach (ParameterSyntax item in mainDeclaration.ParameterList.Parameters)
    WriteLine($"The type of the {item.Identifier} parameter is {item.Type}.");
WriteLine($"The body text of the {mainDeclaration.Identifier} method follows:");
WriteLine(mainDeclaration.Body?.ToFullString());
WriteLine();
WriteLine();
WriteLine();

// Jako korisno za nalazenje svih metoda
var firstParameters = from methodDeclaration in root.DescendantNodes()
        .OfType<MethodDeclarationSyntax>()
    where methodDeclaration.Identifier.ValueText == "Main"
    select methodDeclaration.ParameterList.Parameters.First();
 
var argsParameter2 = firstParameters.FirstOrDefault();
WriteLine(argsParameter2);
*/