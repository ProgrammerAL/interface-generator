using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;
public static class CommentsBlockParser
{
    public static string ParseCommentsBlock(ISymbol symbol)
    {
        var commentsXml = symbol.GetDocumentationCommentXml();
        var comments = DetermineMethodComments(commentsXml);

        return comments;
    }

    private static string DetermineMethodComments(string? commentsXml)
    {
        if (string.IsNullOrWhiteSpace(commentsXml))
        {
            return string.Empty;
        }

        try
        {
            var element = XElement.Parse(commentsXml);

            var builder = new StringBuilder();
            foreach (var node in element.Nodes())
            {
                var nodeTextLines = node.ToString().Trim().Split('\n');

                foreach (var line in nodeTextLines)
                {
                    _ = builder.AppendLine($"/// {line.Trim()}");
                }
            }

            return builder.ToString().Trim();
        }
        catch
        {
            return string.Empty;
        }
    }
}
