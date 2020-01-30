using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var gennedCode = CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        ClassDeclaration("C")
                            .WithModifiers(
                                TokenList(
                                    Token(SyntaxKind.PublicKeyword)))
                            .BuildProperties()))
                .NormalizeWhitespace();

            var code = gennedCode.ToFullString();
        }
    }

    public static class ClassDeclarationSyntaxExtension
    {
        public static ClassDeclarationSyntax BuildProperties(this ClassDeclarationSyntax value)
        {
            var propertyList = new List<PropertyDeclarationSyntax>();


            for (int i = 0; i < 50; i++)
            {
                propertyList.Add(PropertyDeclaration(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword)),
                        Identifier(GetUniqueWord()))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithAccessorList(
                        AccessorList(
                            List<AccessorDeclarationSyntax>(
                                new AccessorDeclarationSyntax[]
                                {
                                    AccessorDeclaration(
                                            SyntaxKind.GetAccessorDeclaration)
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken)),
                                    AccessorDeclaration(
                                            SyntaxKind.SetAccessorDeclaration)
                                        .WithSemicolonToken(
                                            Token(SyntaxKind.SemicolonToken))
                                }))));
            }

            return value.WithMembers(List<MemberDeclarationSyntax>(propertyList.ToArray()));
        }

        private static List<string> _usedWords = new List<string>();
        private static string GetUniqueWord()
        {
            var rand = new Random();
            var faker = new Faker();
            var word = faker.Hacker.Noun();
            while (_usedWords.Contains(word))
            {
                int randomValue = rand.Next(3);
                if (randomValue == 0)
                    word = faker.Hacker.Noun();
                else if (randomValue == 1)
                    word = faker.Commerce.Product();
                else
                    word = faker.Finance.AccountName();
            }

            word = word.Replace(" ", "");
            _usedWords.Add(word);

            return word;
        }

    }
}
