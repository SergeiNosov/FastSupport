#pragma checksum "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "75ebcc4e06b60695a9c34a2f8e0f9ddd5c37fbee"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Messages_Message), @"mvc.1.0.view", @"/Views/Messages/Message.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/user/FastSupport/FastSupportFixed/Views/_ViewImports.cshtml"
using FastSupportFixed;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/user/FastSupport/FastSupportFixed/Views/_ViewImports.cshtml"
using FastSupportFixed.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"75ebcc4e06b60695a9c34a2f8e0f9ddd5c37fbee", @"/Views/Messages/Message.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"db501e97f68e4c4b8599fd5b7907d777fd0865bd", @"/Views/_ViewImports.cshtml")]
    public class Views_Messages_Message : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Dictionary<string, int>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/CSS/messages.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n\n");
            WriteLiteral("\n");
#nullable restore
#line 5 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
  
    ViewData["Title"] = "Диалог";

  

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n<html>\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "75ebcc4e06b60695a9c34a2f8e0f9ddd5c37fbee4636", async() => {
                WriteLiteral("\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "75ebcc4e06b60695a9c34a2f8e0f9ddd5c37fbee4894", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "75ebcc4e06b60695a9c34a2f8e0f9ddd5c37fbee6833", async() => {
                WriteLiteral("\n    <div class=\"container d-flex justify-content-center\">\n        <div class=\"card mt-5\">\n\n\n");
#nullable restore
#line 22 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
             foreach (var message in Model)
            {
                

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
                 if (message.Value == 1)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <div class=\"d-flex flex-row p-3\">\n                        <div class=\"bg-white mr-2 p-3\"><span class=\"text-muted\">");
#nullable restore
#line 27 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
                                                                           Write(Html.DisplayName(message.Key));

#line default
#line hidden
#nullable disable
                WriteLiteral("</span></div>\n                        <img src=\"https://img.icons8.com/color/48/000000/circled-user-male-skin-type-7.png\" width=\"40\" height=\"40\">\n                    </div>\n");
#nullable restore
#line 30 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
                }
                else
                {


#line default
#line hidden
#nullable disable
                WriteLiteral("                    <div class=\"d-flex flex-row p-2\">\n                        <img src=\"https://img.icons8.com/color/48/000000/circled-user-female-skin-type-7.png\" width=\"40\" height=\"40\">\n                        <div class=\"chat ml-2 p-3\">");
#nullable restore
#line 36 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
                                              Write(Html.DisplayName(message.Key));

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\n                    </div>\n");
#nullable restore
#line 38 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"

                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
                 


            }

#line default
#line hidden
#nullable disable
                WriteLiteral("\n\n\n            <div class=\"form-group px-3\"> <textarea class=\"form-control\" rows=\"3\" placeholder=\"Type your message\"></textarea> </div>\n\n\n\n\n            <button type=\"button\"");
                BeginWriteAttribute("onclick", " onclick=\"", 1296, "\"", 1361, 3);
                WriteAttributeValue("", 1306, "location.href=\'", 1306, 15, true);
#nullable restore
#line 51 "/Users/user/FastSupport/FastSupportFixed/Views/Messages/Message.cshtml"
WriteAttributeValue("", 1321, Url.Action("SendMessage?", "Messages"), 1321, 39, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 1360, "\'", 1360, 1, true);
                EndWriteAttribute();
                WriteLiteral(" class=\"btn btn-success\">Отправить</button>\n\n\n\n        </div>\n\n\n    </div>\n\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Dictionary<string, int>> Html { get; private set; }
    }
}
#pragma warning restore 1591
