﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Prima.Templates;

namespace Prima.Services
{
    public class TemplateProvider
    {
        private readonly IReadOnlyDictionary<string, string> _templates;

        public TemplateProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _templates = assembly.GetManifestResourceNames()
                .Where(resourceName => resourceName.StartsWith("Prima.Templates"))
                .Select(resourceName =>
                {
                    using var s = assembly.GetManifestResourceStream(resourceName);
                    if (s == null)
                    {
                        throw new InvalidOperationException($"Resource {resourceName} failed to be loaded from the assembly.");
                    }

                    var resourceKey = resourceName
                        .Replace("Prima.Templates.", "")
                        .Replace(".md", "")
                        .Replace(".", "/")
                        .ToLowerInvariant();
                    resourceKey += ".md";

                    using var sr = new StreamReader(s);
                    return new { Name = resourceKey, Data = sr.ReadToEnd() };
                })
                .ToImmutableDictionary(resource => resource.Name, resource => resource.Data);
        }

        public IEnumerable<string> GetNames() => _templates.Keys;

        public ResolvedTemplate Execute<T>(string templateName, T templateData) where T : class
        {
            var template = _templates[templateName];
            var replaceableTokens = GetReplaceableTokens(template);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var token in replaceableTokens)
            {
                template = template.Replace("{{." + token + "}}", templateData.GetPropertyValue(token)?.ToString());
            }

            template = template.Trim();
            return new ResolvedTemplate(template);
        }

        private static readonly Regex TokenRegex = new(@"\{\{.(?<Token>.+)\}\}", RegexOptions.Compiled);
        private static IEnumerable<string> GetReplaceableTokens(string template)
        {
            return TokenRegex.Matches(template)
                .Where(match => match.Success)
                .Select(match => match.Groups["Token"].Value);
        }
    }
}