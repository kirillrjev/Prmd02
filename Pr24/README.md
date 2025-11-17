## Практическая работа №24
Написание автотестов для GET-запросов с использованием RestSharp
Вариант 1: JSONPlaceholder — Users API

## Models/User.cs
using System.Text.Json.Serialization;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }

    [JsonPropertyName("company")]
    public Company Company { get; set; }
}

public class Address
{
    [JsonPropertyName("street")] public string Street { get; set; }
    [JsonPropertyName("suite")] public string Suite { get; set; }
    [JsonPropertyName("city")] public string City { get; set; }
    [JsonPropertyName("zipcode")] public string Zipcode { get; set; }
    [JsonPropertyName("geo")] public Geo Geo { get; set; }
}

public class Geo
{
    [JsonPropertyName("lat")] public string Lat { get; set; }
    [JsonPropertyName("lng")] public string Lng { get; set; }
}

public class Company
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("catchPhrase")] public string CatchPhrase { get; set; }
    [JsonPropertyName("bs")] public string Bs { get; set; }
}
## Models/Post.cs
using System.Text.Json.Serialization;

public class Post
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }
}
## Models/Todo.cs
using System.Text.Json.Serialization;

public class Todo
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
}
## Helpers/ApiClient.cs
using RestSharp;

namespace RestSharpGetTests.Helpers
{
    public class ApiClient : IDisposable
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                MaxTimeout = 5000,
                ThrowOnAnyError = false
            };

            _client = new RestClient(options);
        }

        public RestResponse<T> ExecuteRequest<T>(string endpoint, Method method = Method.Get)
        {
            var request = new RestRequest(endpoint, method);
            return _client.Execute<T>(request);
        }

        public RestResponse ExecuteRequest(string endpoint, Method method = Method.Get)
        {
            var request = new RestRequest(endpoint, method);
            return _client.Execute(request);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
## Tests/UserApiTests.cs
using NUnit.Framework;
using RestSharp;
using System.Net;
using RestSharpGetTests.Helpers;

namespace RestSharpGetTests.Tests
{
    [TestFixture]
    public class UserApiTests
    {
        private ApiClient apiClient;

        [SetUp]
        public void Setup()
        {
            apiClient = new ApiClient("https://jsonplaceholder.typicode.com");
        }

        [Test]
        public void GetAllUsers_ReturnsListOfUsers()
        {
            var response = apiClient.ExecuteRequest<List<User>>("users");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data, Has.Count.GreaterThan(0));

            Assert.That(response.Data[0].Id, Is.GreaterThan(0));
            Assert.That(response.Data[0].Name, Is.Not.Empty);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetUserById_ValidId_ReturnsCorrectUser(int userId)
        {
            var response = apiClient.ExecuteRequest<User>($"users/{userId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Id, Is.EqualTo(userId));
            Assert.That(response.Data.Name, Is.Not.Empty);
        }

        [Test]
        public void GetUserPosts_ValidUserId_ReturnsPosts()
        {
            var response = apiClient.ExecuteRequest<List<Post>>("users/1/posts");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Has.Count.GreaterThan(0));

            Assert.That(response.Data[0].UserId, Is.EqualTo(1));
        }

        [Test]
        public void GetUserTodos_ValidUserId_ReturnsTodos()
        {
            var response = apiClient.ExecuteRequest<List<Todo>>("users/1/todos");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Has.Count.GreaterThan(0));

            Assert.That(response.Data[0].UserId, Is.EqualTo(1));
            Assert.That(response.Data[0].Title, Is.Not.Empty);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFound()
        {
            var response = apiClient.ExecuteRequest("users/99999");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TearDown]
        public void TearDown()
        {
            apiClient.Dispose();
        }
    }
}
# Практическая работа №24  
## Написание автотестов для GET-запросов с использованием RestSharp  
### Вариант 1: JSONPlaceholder — Users API

---

## Задание
Реализовать автотесты для следующих эндпоинтов:

- GET /users  
- GET /users/{id}  
- GET /users/{id}/posts  
- GET /users/{id}/todos  

Проверить:
- статус-коды  
- структуру JSON  
- корректность данных  
- негативный сценарий  

---

## Структура проекта

```
RestSharpGetTests/
├── Models/
├── Helpers/
├── Tests/
└── README.md
```

---

##  Пример теста

```csharp
[Test]
public void GetUserById_ValidId_ReturnsCorrectUser()
{
    var response = apiClient.ExecuteRequest<User>("users/1");

    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    Assert.That(response.Data.Id, Is.EqualTo(1));
}
```

---

##  Итог
Все запросы протестированы.  
Реализованы: базовые тесты, параметризованные тесты, негативный сценарий, собственный API-клиент, и модели.  

---

