using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Bddify;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorEngine.Templating;

namespace RazorEngineFromXml.Tests
{
    [TestClass]
    [Story(AsA = "As TemplateServiceXml",
        IWant = "I want to disallow invalid operations",
        SoThat = "So that people know when they have tried to use me incorrectly")]
    public class TemplateServiceXmlDoesNotAllowInvalidOperations
    {
        private TemplateServiceXml _templateServiceXml;
        private string _razorTemplate;
        private string _xmlData;

        private string _result;
        private Exception _exception;

        [TestInitialize]
        public void Init()
        {
            _templateServiceXml = new TemplateServiceXml();
        }

        [TestMethod]
        public void NullTemplateAndNullXmlDataDisallowsUse()
        {
            this.Given(s => s.GivenIPassNullParameters())
                .When(s => s.WhenITryAndParseATemplateAndData())
                .Then(s => s.ThenAnExceptionIsThrown())
                .And(s=>s.ThenResultIsNotSet())
                .Bddify();
        }

        #region Given...

        private void GivenIPassNullParameters()
        {
            _razorTemplate = null;
            _xmlData = null;
        }

        #endregion


        #region When...

        private void WhenITryAndParseATemplateAndData()
        {
            try
            {
                _result = _templateServiceXml.Parse(_razorTemplate, _xmlData);
            }
            catch(Exception ex)
            {
                _exception = ex;
            }
        }

        #endregion


        #region Then...

        private void ThenResultIsNotSet()
        {
            Assert.IsTrue(string.IsNullOrWhiteSpace(_result));
        }

        private void ThenAnExceptionIsThrown()
        {
            Assert.IsTrue(_exception != null);
        }

        #endregion

    }
}
