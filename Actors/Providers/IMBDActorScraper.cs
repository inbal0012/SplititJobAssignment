using HtmlAgilityPack;

namespace SplititJobAssignment.Actors
{
    public class IMBDActorScraper : BaseActorScraper
    {
        private const string baseUrl = "https://www.imdb.com/";

        protected override HtmlNodeCollection GetActorNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes(".//div[@class='lister-item mode-detail']");
        }

        protected override string GetName(HtmlNode actorNode)
        {
            var res = actorNode.SelectSingleNode(".//h3[@class='lister-item-header']/a");
            if (res != null)
            {
                return cleanText(res.InnerText);
            }

            return "";
        }

        protected override string GetDetails(HtmlNode actorNode)
        {
            var res = actorNode.SelectSingleNode(".//div[@class='lister-item-content']").ChildNodes[5];
            if (res != null)
            {
                return cleanText(res.InnerText);
            }

            return "";
        }

        protected override string GetType(HtmlNode actorNode)
        {
            var res = actorNode.SelectSingleNode(".//h3[@class='lister-item-header']").NextSibling.NextSibling.ChildNodes[0];
            if (res != null)
            {
                return cleanText(res.InnerText);
            }

            return "";
        }

        protected override int GetRank(HtmlNode actorNode)
        {
            var res = actorNode.SelectSingleNode(".//h3[@class='lister-item-header']/span");
            if (res != null)
            {
                return int.Parse(cleanText(res.InnerText));
            }

            return 0;
        }

        protected override string GetSource(HtmlNode actorNode)
        {
            return "IMBD";
        }

        protected override bool GetHasNextPage(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'lister-page-next next-page')]") != null;
        }

        protected override string GetNextPageUrl(HtmlDocument htmlDocument)
        {
            var res = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'lister-page-next next-page')]");
            if (res != null)
            {
                var url = $"{baseUrl}{res.Attributes["href"].Value}";
                return url;
            }

            return "";
        }



    }
}