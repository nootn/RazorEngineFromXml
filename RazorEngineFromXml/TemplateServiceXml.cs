using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AmazedSaint.Elastic;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace RazorEngineFromXml
{
    public class TemplateServiceXml
    {
        private readonly TemplateService _templateService;

        public TemplateServiceXml()
        {
            _templateService = new TemplateService();
        }

        public TemplateServiceXml(ITemplateServiceConfiguration config)
        {
            _templateService = new TemplateService(config);
        }

        public string Parse(string razorTemplate, string xmlData)
        {
            var dataAsXml = GetDataAsXml(xmlData);
            return Parse(razorTemplate, dataAsXml);
        }

        public string Parse(string razorTemplate, XElement xmlData)
        {
            var obj = xmlData.ToElastic();
            return _templateService.Parse(razorTemplate, obj);
        }

        public string ParseMany(IEnumerable<string> razorTemplates, string xmlData, bool parallel = false)
        {
            var dataAsXml = GetDataAsXml(xmlData);
            var obj = dataAsXml.ToElastic();
            return _templateService.ParseMany(razorTemplates, obj, parallel);
        }

        public IEnumerable<string> ParseMany(IEnumerable<string> razorTemplates, IEnumerable<string> xmlDatas,
                                             bool parallel = false)
        {
            var objs = xmlDatas.Select(GetDataAsXml).Select(dataAsXml => dataAsXml.ToElastic()).ToList();
            return _templateService.ParseMany(razorTemplates, objs, parallel);
        }


        private static XElement GetDataAsXml(string xmlData)
        {
            XElement dataAsXml;
            try
            {
                dataAsXml = XElement.Parse(xmlData);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse xmlData into XML - please supply valid XML", "xmlData", ex);
            }
            return dataAsXml;
        }
    }
}