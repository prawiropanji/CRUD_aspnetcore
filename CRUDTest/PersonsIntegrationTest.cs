using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest
{
    public class PersonsIntegrationTest : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _client;
         public PersonsIntegrationTest(CustomWebAppFactory customWebAppFactory)
        {
            _client =  customWebAppFactory.CreateClient();
        }

        [Fact]
        public async Task Persons_IndexPageReturnViewWithTableElement()
        {
            HttpResponseMessage response = await _client.GetAsync("persons/index");

            //Assert status code
            response.Should().BeSuccessful();

            //Assert the DOM response
            string responseString = await  response.Content.ReadAsStringAsync();

            //parse HTML content
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseString);
            HtmlNode document= htmlDocument.DocumentNode;

            //query DOM using fizzler
            var table = document.QuerySelector("table");
            table.Should().NotBeNull();

        }
    }
}
