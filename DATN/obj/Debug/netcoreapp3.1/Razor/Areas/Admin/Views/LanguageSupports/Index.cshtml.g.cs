#pragma checksum "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "973bf9e3de1c63209c27a225e9d56b568d9faf9f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_LanguageSupports_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/LanguageSupports/Index.cshtml")]
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
#line 1 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\_ViewImports.cshtml"
using DATN;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\_ViewImports.cshtml"
using DATN.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
using PagedList.Core.Mvc;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"973bf9e3de1c63209c27a225e9d56b568d9faf9f", @"/Areas/Admin/Views/LanguageSupports/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9e39dcc3d8d70168c8c7d99b2942a95a3e873006", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_LanguageSupports_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PagedList.Core.IPagedList<DATN.Models.LanguageSupport>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-inverse-primary btn-md"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-inverse-success btn-md"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-inverse-info btn-md"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-inverse-danger btn-md"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Quản lý danh sách ngôn ngữ được hỗ trợ với mỗi bài tập</h1>\r\n<br />\r\n<p>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "973bf9e3de1c63209c27a225e9d56b568d9faf9f6567", async() => {
                WriteLiteral("Create New");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-ExerciseId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 12 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                                                            WriteLiteral(ViewBag.CurrentExerciseId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["ExerciseId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-ExerciseId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["ExerciseId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
</p>
<br />

<div class=""row"">
    <div class=""col-3"">
        <input type=""text"" class=""form-control"" id=""nameLanguage"" name=""nameEmail"" placeholder=""Tìm kiếm ngôn ngữ theo tên"">
    </div>
</div>
<br />

<div class=""row"">
    <div class=""col-lg-12 stretch-card"">
        <div class=""card"">
            <div class=""card-body"">
                <div class=""table-responsive"">
                    <table class=""table"" style=""max-width: 1160px;"">
                        <thead>
                            <tr>
                                <th>
                                    ID
                                </th>
                                <th>
                                    Challenge
                                </th>
                                <th>
                                    Tên bài tập
                                </th>
                                <th>
                                    Tên ngôn ngữ
                                </th>
 ");
            WriteLiteral(@"                               <th>
                                    File Main
                                </th>
                                <th>
                                    File Function
                                </th>
                                <th>
                                    Mô tả
                                </th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody id=""tb-exercise-index"">
");
#nullable restore
#line 56 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                             if (Model != null)
                            {
                                

#line default
#line hidden
#nullable disable
#nullable restore
#line 58 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                 foreach (var item in Model)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 62 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.LanguageSupId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 65 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.Exercise.Challenge.ChallengeName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 68 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.Exercise.ExcerciseName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 71 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.Language.LanguageName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td style=\"white-space: normal;\">\r\n                                            ");
#nullable restore
#line 74 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.FileMain);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td style=\"white-space: normal;\">\r\n                                            ");
#nullable restore
#line 77 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                       Write(item.FileFunction);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "973bf9e3de1c63209c27a225e9d56b568d9faf9f13962", async() => {
                WriteLiteral("Edit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 80 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                                                                                          WriteLiteral(item.LanguageSupId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\r\n                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "973bf9e3de1c63209c27a225e9d56b568d9faf9f16343", async() => {
                WriteLiteral("Details");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 81 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                                                                                          WriteLiteral(item.LanguageSupId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\r\n                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "973bf9e3de1c63209c27a225e9d56b568d9faf9f18727", async() => {
                WriteLiteral("Delete");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 82 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                                                                                           WriteLiteral(item.LanguageSupId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 85 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 85 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                                 
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tbody>\r\n                    </table>\r\n\r\n                </div>\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<br />\r\n\r\n\r\n\r\n");
            DefineSection("Script", async() => {
                WriteLiteral(@"
    <script>
        $(document).ready(function () {

            ////Lọc khi thay đổi thử thách
            //jQuery('#idchallenge').change(function () {
            //    var challengeId = jQuery(this).children("":selected"").attr(""value"");
            //    challengeId = parseFloat(challengeId);

            //    $('#idchallenge option')
            //        .removeAttr('selected');
            //    $(""#idchallenge > [value="" + challengeId + ""]"").attr(""selected"", ""true"");

            //    $.ajax({
            //        url: '/Admin/LanguageSupports/Filtter',
            //        datatype: ""json"",
            //        type: ""GET"",
            //        data: {
            //            ChallengeId: challengeId
            //        },
            //        async: true,
            //        success: function (results) {
            //            if (results.status == ""success"") {
            //                window.location.href = results.redirectUrl;
            //           ");
                WriteLiteral(@" }
            //        },
            //        error: function (xhr) {
            //            alert(""error"");
            //        }
            //    });
            //});


            ////Tìm kiếm theo tên bài tập
            //$(""#nameExercise"").keyup(function () {
            //    var strnameLangugae = $('#nameLanguage').val();
            //    var strnameExercise = $('#nameExercise').val();
            //    var challengeId = jQuery('#idchallenge').children("":selected"").attr(""value"");
            //    challengeId = parseFloat(challengeId);
            //    $.ajax({
            //        url: '/Admin/Search/FindLanguageSupport/',
            //        datatype: ""json"",
            //        type: ""POST"",
            //        data: { nameLangugae: strnameLangugae, nameExercise: strnameExercise, ChallengeId: challengeId },
            //        async: true,
            //        success: function (results) {
            //            $(""#tb-exercise-index"").html("""");
    ");
                WriteLiteral(@"        //            $(""#tb-exercise-index"").html(results);
            //        },
            //        error: function (xhr) {
            //            alert('error');
            //        }
            //    });
            //});

            //Tìm kiếm theo tên ngôn ngữ
            $(""#nameLanguage"").keyup(function () {
                var strnameLangugae = $('#nameLanguage').val();
                var exerciseId = ");
#nullable restore
#line 159 "D:\End-term\Đồ án\Hệ thống học lập trình online\DATN\DATN\Areas\Admin\Views\LanguageSupports\Index.cshtml"
                            Write(Html.Raw(Json.Serialize(ViewBag.CurrentExerciseId)));

#line default
#line hidden
#nullable disable
                WriteLiteral(@";
                exerciseId = parseFloat(exerciseId);
                $.ajax({
                    url: '/Admin/Search/FindLanguageSupport/',
                    datatype: ""json"",
                    type: ""POST"",
                    data: { nameLangugae: strnameLangugae, ExerciseId: exerciseId },
                    async: true,
                    success: function (results) {
                        $(""#tb-exercise-index"").html("""");
                        $(""#tb-exercise-index"").html(results);
                    },
                    error: function (xhr) {
                        alert('error');
                    }
                });
            });
        });
    </script>
");
            }
            );
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PagedList.Core.IPagedList<DATN.Models.LanguageSupport>> Html { get; private set; }
    }
}
#pragma warning restore 1591
