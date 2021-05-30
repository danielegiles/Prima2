﻿using NUnit.Framework;
using Prima.Services;
using System.Linq;

namespace Prima.Tests
{
    [TestFixture]
    public class TemplateProviderTests
    {
        private TemplateProvider _templates;

        [SetUp]
        public void Setup()
        {
            _templates = new TemplateProvider();
        }

        [Test]
        public void TestFile_Exists()
        {
            Assert.True(
                _templates.GetNames().Contains("test.md"),
                "Expected test.md, got [{0}]",
                string.Join(',', _templates.GetNames()));
        }

        [Test]
        public void TestFile_IsExecutable()
        {
            const string token = "cool token";
            var text = _templates.Execute("test.md", new
            {
                Token = token,
            });

            Assert.True(
                text.Contains($"Oh, wow, I have a {token}!"),
                "Expected to find \"Oh, wow, I have a {0}!, got\n{1}",
                token,
                text);
        }
    }
}