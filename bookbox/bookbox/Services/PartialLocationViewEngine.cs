using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace BookBox.Services
{ 
    public class PartialLocationViewEngine : RazorViewEngine
    {

        private static readonly string[] NEW_PARTIAL_VIEW_FORMATS = new[] {
      "~/Views/Foo/{0}.cshtml",
      "~/Views/Shared/Bar/{0}.cshtml"
        };

        public PartialLocationViewEngine(IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, RazorProject razorProject, ILoggerFactory loggerFactory, DiagnosticSource diagnosticSource) : base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, razorProject, loggerFactory, diagnosticSource)
        {
        }

        //public PartialLocationViewEngine()
        //{
        //    // Keep existing locations in sync
        //    base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(NEW_PARTIAL_VIEW_FORMATS).ToArray();
        //}
    }
}
