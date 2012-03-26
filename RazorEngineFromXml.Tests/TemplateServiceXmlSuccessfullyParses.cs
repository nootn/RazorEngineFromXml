using System;
using System.IO;
using Bddify;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorEngineFromXml.Tests
{
    [TestClass]
    [Story(AsA = "As TemplateServiceXml",
        IWant = "I want to parse some input to give valid output",
        SoThat = "So that people can turn a Razor template and XML into HTML")]
    public class TemplateServiceXmlSuccessfullyParses
    {
        private Exception _exception;
        private string _razorTemplate;

        private string _result;
        private TemplateServiceXml _templateServiceXml;
        private string _xmlData;

        [TestInitialize]
        public void Init()
        {
            _templateServiceXml = new TemplateServiceXml();
        }

        [TestMethod]
        public void FlatStructuredObjectParsesSuccessfully()
        {
            this.Given(s => s.GivenIPassValidParametersOfAFlatStructuredObject())
                .When(s => s.WhenITryAndParseATemplateAndData())
                .Then(s => s.ThenNoExceptionIsThrown())
                .And(
                    s =>
                    s.ThenResultIsAsExpected(
                        " <h1>Root</h1><p>Attribute 'testAttribute' = ValueOfTestAttribute</p><p>Value of node = ValueOfSingleRootFlatNode</p>"))
                .Bddify();
        }

        [TestMethod]
        public void SingleLevelStructuredObjectParsesSuccessfully()
        {
            this.Given(s => s.GivenIPassValidParametersOfASingleLevelStructuredObject())
                .When(s => s.WhenITryAndParseATemplateAndData())
                .Then(s => s.ThenNoExceptionIsThrown())
                .And(
                    s =>
                    s.ThenResultIsAsExpected(
                        " <h1>Root</h1><p>Attribute 'testAttribute' = ValueOfTestAttribute</p><h2>Second Child</h2><p>Attribute 'testAttribute' = The Second Child Attribute</p><p>Value of node = The Second Child Value</p><h2>Second Child</h2><p>Attribute 'testAttribute' = The Second Child Attribute</p><p>Value of node = The Second Child Value</p>"))
                .Bddify();
        }

        [TestMethod]
        public void FancyLinqQueryOnDataStructureParsesSuccessfully()
        {
            this.Given(s => s.GivenIDoALinqQueryOnAStructuredObject())
                .When(s => s.WhenITryAndParseATemplateAndData())
                .Then(s => s.ThenNoExceptionIsThrown())
                .And(
                    s =>
                    s.ThenResultIsAsExpected(
                        @"<h1>
    Testing Linq to get the last element</h1>
<ul>
            <li>Value is an int: 1</li>
            <li>Value is an int: 2</li>
            <li>Value is an int: 3</li>
</ul>"))
                .Bddify();
        }

        #region Given...

        private void GivenIPassValidParametersOfAFlatStructuredObject()
        {
            _razorTemplate =
                "@using System <h1>Root</h1><p>Attribute 'testAttribute' = @Model.Attributes[\"testAttribute\"].InternalValue</p><p>Value of node = @Model.InternalValue</p>";
            _xmlData = "<Flat testAttribute=\"ValueOfTestAttribute\">ValueOfSingleRootFlatNode</Flat>";
        }

        private void GivenIPassValidParametersOfASingleLevelStructuredObject()
        {
            _razorTemplate =
                "@using System <h1>Root</h1><p>Attribute 'testAttribute' = @Model.Attributes[\"testAttribute\"].InternalValue</p><h2>Second Child</h2><p>Attribute 'testAttribute' = @Model.SecondChild.Attributes[\"testAttribute\"].InternalValue</p><p>Value of node = @Model.SecondChild.InternalValue</p><h2>Second Child</h2><p>Attribute 'testAttribute' = @Model.SecondChild.Attributes[\"testAttribute\"].InternalValue</p><p>Value of node = @Model.SecondChild.InternalValue</p>";
            _xmlData =
                "<Deep testAttribute=\"ValueOfTestAttribute\"><SecondChild testAttribute=\"The Second Child Attribute\">The Second Child Value</SecondChild><SecondChild testAttribute=\"The Second Child Attribute\">The Second Child Value</SecondChild></Deep>";
        }

        private void GivenIDoALinqQueryOnAStructuredObject()
        {
            _razorTemplate = TestRazorFiles.TestLinq;
            _xmlData =
                "<Deep><One>1</One><Two><InternalTwo>2</InternalTwo></Two><Three>3</Three></Deep>";
        }

        #endregion

        #region When...

        private void WhenITryAndParseATemplateAndData()
        {
            try
            {
                _result = _templateServiceXml.Parse(_razorTemplate, _xmlData);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        #endregion

        #region Then...

        private void ThenResultIsAsExpected(string expectedResult)
        {
            Assert.IsTrue(string.Equals(expectedResult, _result, StringComparison.CurrentCulture));
        }

        private void ThenNoExceptionIsThrown()
        {
            Assert.IsTrue(_exception == null);
        }

        #endregion
    }
}