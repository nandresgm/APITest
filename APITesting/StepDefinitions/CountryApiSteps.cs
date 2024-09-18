using Newtonsoft.Json.Linq;
using NUnit.Framework.Legacy;
using RestSharp;
using TechTalk.SpecFlow;

namespace APITesting.StepDefinitions
{
    [Binding]
    public class CountryApiSteps
    {
        private RestResponse _response;
        private readonly RestClient _client = new RestClient("https://restcountries.com/v3.1");

        [When(@"I call the API with country code ""(.*)""")]
        public void WhenICallTheAPIWithCountryCode(string countryCode)
        {
            var request = new RestRequest($"/alpha/{countryCode}");
            _response = _client.Execute<RestResponse>(request);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            ClassicAssert.AreEqual(statusCode, (int)_response.StatusCode);
        }

        [Then(@"the response should contain the country name ""(.*)""")]
        public void ThenTheResponseShouldContainTheCountryName(string expectedCountryName)
        {
            var content = _response.Content;
            var jsonArray = JArray.Parse(content);
            var jsonObject = (JObject)jsonArray[0];
            var actualCountryName = jsonObject["name"]?["common"]?.ToString();
            ClassicAssert.AreEqual(expectedCountryName, actualCountryName);
        }

        [Then(@"the response structure should include the following keys")]
        public void ThenTheResponseStructureShouldIncludeTheFollowingKeys(Table table)
        {
            var expectedKeys = table.Rows.Select(row => row["Key"].Trim()).ToArray();

            var content = _response.Content;
            var jsonArray = JArray.Parse(content);
            var jsonObject = (JObject)jsonArray[0];

            foreach (var key in expectedKeys)
            {
                ClassicAssert.IsTrue(jsonObject.ContainsKey(key), $"Key '{key}' is missing in the response.");
            }
        }

        [Then(@"the capital should be ""(.*)""")]
        public void ThenTheCapitalShouldBe(string expectedCapital)
        {
            var content = _response.Content;
            var jsonArray = JArray.Parse(content);
            var jsonObject = (JObject)jsonArray[0];
            var actualCapital = jsonObject["capital"]?[0]?.ToString();
            ClassicAssert.AreEqual(expectedCapital, actualCapital);
        }

        [Then(@"the response should contain an error message ""(.*)""")]
        public void ThenTheResponseShouldContainAnErrorMessage(string expectedErrorMessage)
        {
            var content = _response.Content;
            ClassicAssert.IsTrue(content.Contains(expectedErrorMessage));
        }
    }
}
