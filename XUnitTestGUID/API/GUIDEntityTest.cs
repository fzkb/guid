using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using GUIDCRUD;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using GUIDCRUD.Models;
using GUIDCRUD.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit.Extensions;
using Newtonsoft.Json;
using System.Text;

namespace XUnitTestGUID.API
{    
    /// <summary>
    /// UNIT Test Class to validate the CRUD functationality of WEB API
    /// </summary>
    public class GUIDEntityTest
    {
        private readonly HttpClient _httpClient;

        private readonly string _connectionString = "server=CBSDEVSVR06;database=GUIDDB;integrated security=yes;";

        public GUIDEntityTest()
        {          

            WebHostBuilder webHostBuilder = new WebHostBuilder();

            
            webHostBuilder.UseStartup<Startup>();

            var server = new TestServer(new WebHostBuilder().UseEnvironment("Dev")
                                            .UseSetting("connectionString", _connectionString)                                            
                                            .UseStartup<Startup>());


            _httpClient = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task GUIDGetAllAsync(string method)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/GUIDEntities");

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "42441E8BEC3D488E8B8E6C1D861B5AA7")]
        public async Task GUIDGetEntityByGuidAsync(string method, string id)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/GUIDEntities/{id}");

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [JsonFileData("data.json", "UPDATE")]
        public async Task GUIDUpdateEntityByGuidAsync(string method, string id, GUIDEntity gUIDEntity)
        {
            String serializedString = JsonConvert.SerializeObject(gUIDEntity);

            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/GUIDEntities/{id}");

            var httpContent = new StringContent(serializedString, Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PutAsync($"api/GUIDEntities/{id}", httpContent);
            
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", "42441E8BEC3D488E8B8E6C1D861B5AA7")]
        public async Task GUIDDeleteEntityByGuidAsync(string method, string id = "")
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/GUIDEntities/{id}");

            //Act
            var response = await _httpClient.DeleteAsync(request.RequestUri);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [JsonFileData("data.json", "ADDBYGUID")]
        public async Task GUIDAddbyGUIDAsync(string method, GUIDEntity gUIDEntity)
        {
            gUIDEntity.GuidValue = Guid.NewGuid().ToString("N").ToUpper();

            String serializedString = JsonConvert.SerializeObject(gUIDEntity);

            string id = gUIDEntity.GuidValue;

            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/GUIDEntities/{id}");

            var httpContent = new StringContent(serializedString, Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync($"api/GUIDEntities/{id}", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [JsonFileData("data.json", "ADDBYUSER")]
        public async Task GUIDAddbyUserAsync(string method, GUIDEntity gUIDEntity)
        {
            String serializedString = JsonConvert.SerializeObject(gUIDEntity);            

            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/GUIDEntities");

            var httpContent = new StringContent(serializedString, Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync($"api/GUIDEntities", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }


    }
}
