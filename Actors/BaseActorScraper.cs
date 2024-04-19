using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SplititJobAssignment.Actors
{
    public class BaseActorScraper
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<Actor>> ScrapeActors(string url)
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var htmlBody = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlBody);

                var actorNodes = GetActorNodes(htmlDocument);

                var actors = new List<Actor>();

                if (actorNodes != null)
                {
                    foreach (var actorNode in actorNodes)
                    {
                        var actor = new Actor
                        {
                            Name = GetName(actorNode),
                            Details = GetDetails(actorNode),
                            Type = GetType(actorNode),
                            Rank = GetRank(actorNode),
                            Source = GetSource(actorNode),
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        actors.Add(actor);
                    }
                }

                var hasNextPage = GetHasNextPage(htmlDocument);
                if (hasNextPage)
                {
                    var nextPageUrl = GetNextPageUrl(htmlDocument);
                    var nextPageActors = await ScrapeActors(nextPageUrl);
                    actors.AddRange(nextPageActors);
                }

                return actors;
            }

            return null;
        }

        protected virtual HtmlNodeCollection GetActorNodes(HtmlDocument htmlDocument)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetName(HtmlNode actorNode)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetDetails(HtmlNode actorNode)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetType(HtmlNode actorNode)
        {
            throw new NotImplementedException();
        }

        protected virtual int GetRank(HtmlNode actorNode)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetSource(HtmlNode actorNode)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetNextPageUrl(HtmlDocument htmlDocument)
        {
            throw new NotImplementedException();
        }

        protected virtual bool GetHasNextPage(HtmlDocument htmlDocument)
        {
            throw new NotImplementedException();
        }

        protected string cleanText(string text)
        {
            var res = text.Replace("\n", "").Trim();
            return Regex.Replace(res, "[^0-9A-Za-z _'-]", "");
        }
    }
}