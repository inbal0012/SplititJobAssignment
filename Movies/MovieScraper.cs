using System;
using System.Net.Http;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using SplititJobAssignment.Movies.dto;


namespace SplititJobAssignment.Movies
{
    public class MovieScraper
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<Movie>> ScrapeMovies(string url)
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var htmlBody = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlBody);

                var movieNodes = htmlDocument.DocumentNode.SelectNodes("//li[contains(@class,'ipc-metadata-list-summary-item')]");

                if (movieNodes != null)
                {
                    var MoviesList = new List<Movie>();
                    foreach (var node in movieNodes)
                    {
                        var details = node.SelectSingleNode(".//div[contains(@class,'cli-children')]");

                        var name = details.SelectSingleNode(".//h3[contains(@class,'ipc-title__text')]").InnerText.Split(".")[1].Trim();

                        var moreDetails = details.SelectNodes(".//span[contains(@class,'cli-title-metadata-item')]");
                        var year = int.Parse(moreDetails[0].InnerText);
                        var length = moreDetails[1].InnerText;

                        var mpa = "";
                        if (moreDetails.Count > 2)
                        {
                            mpa = moreDetails[2].InnerText;
                        }

                        var rateNode = details.SelectSingleNode(".//span[contains(@class,'ipc-rating-star--imdb')]").InnerText.Split("(");
                        var rating = double.Parse(rateNode[0].Trim());

                        var movie = new Movie
                        {
                            Year = year,
                            Length = length,
                            MPA = mpa,
                            Rating = rating,
                            Name = name,
                        };

                        Console.WriteLine(movie.Name);
                        MoviesList.Add(movie);
                    }

                    return MoviesList;
                }

            }

            return null;
        }
    }
}
