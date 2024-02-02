using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebComponentsWithTagHelpers;

public class WebComponentOptimizer : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var children = await output.GetChildContentAsync();
        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(children.GetContent());
        
        output.Content.Clear();
        output.TagName = "";

        var template = (IHtmlTemplateElement)document.GetElementsByTagName("template")[0];
        var webComponentName = template.GetAttribute("name");
        
        if (webComponentName is { })
        {
            var outputBuilder = new StringWriter();
            var formatter = new HtmlMarkupFormatter();
            var webComponents = document.GetElementsByTagName(webComponentName);
            
            foreach (var webComponent in webComponents)
            {
                var clone = template.Content.Clone();
                var slot = clone.FindDescendant<IHtmlSlotElement>()!;
                // replace the slot with web component contents
                slot.OuterHtml = webComponent.InnerHtml;
                slot.RemoveFromParent();
                webComponent.InnerHtml = clone.ToHtml(); 
                webComponent.ToHtml(outputBuilder,formatter); ;
            }

            output.Content.SetHtmlContent(outputBuilder.ToString());
        }
    }
}