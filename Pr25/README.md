## Практическая работа 25. Написание автотестов для POST, PUT, DELETE запросов
Тема:

Разработка автоматизированных тестов для модифицирующих операций REST API (POST, PUT, DELETE) с использованием RestSharp в .NET
## Цель работы
Освоить навыки тестирования модифицирующих операций REST API (CRUD) на примере сервиса JSONPlaceholder.
## Задачи работы
Изучить особенности тестирования POST / PUT / PATCH / DELETE запросов
Научиться отправлять тела запросов с данными
Проверять статус-коды и возвращаемые структуры
Реализовать полный CRUD-цикл
Добавить негативные тесты и проверки ошибок
Использовать генератор тестовых данных (Bogus)
Разработать удобный API-клиент для повторного использования
Подготовить README.md и структуру тестового проекта
## Models/CreatePostRequest.cs

using System.Text.Json.Serialization;

public class CreatePostRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}
## Models/UpdatePostRequest.cs
using System.Text.Json.Serialization;

public class UpdatePostRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}
## Models/PostResponse.cs
public class PostResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}
## TestData/TestDataGenerator.cs
using Bogus;

public class TestDataGenerator
{
    public CreatePostRequest GeneratePost()
    {
        return new Faker<CreatePostRequest>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Body, f => f.Lorem.Paragraph())
            .RuleFor(x => x.UserId, 1)
            .Generate();
    }

    public UpdatePostRequest GenerateUpdatedPost()
    {
        return new Faker<UpdatePostRequest>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Body, f => f.Lorem.Paragraph())
            .RuleFor(x => x.UserId, 1)
            .Generate();
    }
}
## Helpers/ApiClient.cs
using RestSharp;

public class ApiClient : IDisposable
{
    private readonly RestClient _client;

    public ApiClient(string baseUrl)
    {
        _client = new RestClient(new RestClientOptions(baseUrl)
        {
            MaxTimeout = 5000,
            ThrowOnAnyError = false
        });
    }

    public RestResponse<T> ExecuteRequest<T>(string endpoint, Method method, object body = null)
    {
        var request = new RestRequest(endpoint, method);

        if (body != null)
            request.AddJsonBody(body);

        return _client.Execute<T>(request);
    }

    public RestResponse ExecuteRequest(string endpoint, Method method, object body = null)
    {
        var request = new RestRequest(endpoint, method);

        if (body != null)
            request.AddJsonBody(body);

        return _client.Execute(request);
    }

    public void Dispose() => _client?.Dispose();
}
## Tests/BaseTest.cs
using NUnit.Framework;

public class BaseTest
{
    protected ApiClient apiClient;
    protected TestDataGenerator testData;

    [SetUp]
    public virtual void Setup()
    {
        apiClient = new ApiClient("https://jsonplaceholder.typicode.com");
        testData = new TestDataGenerator();
    }

    [TearDown]
    public virtual void TearDown()
    {
        apiClient?.Dispose();
    }
}
## Tests/PostCRUDTests.cs 
using NUnit.Framework;
using RestSharp;
using System.Net;

[TestFixture]
public class PostCRUDTests : BaseTest
{
    [Test]
    public void CreatePost_ShouldReturnCreated()
    {
        var newPost = testData.GeneratePost();

        var response = apiClient.ExecuteRequest<PostResponse>(
            "posts", Method.Post, newPost);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(response.Data.Title, Is.EqualTo(newPost.Title));
    }

    [Test]
    public void UpdatePost_ShouldReturnOk()
    {
        var updateData = testData.GenerateUpdatedPost();

        var response = apiClient.ExecuteRequest<PostResponse>(
            "posts/1", Method.Put, updateData);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Data.Title, Is.EqualTo(updateData.Title));
    }

    [Test]
    public void PatchPost_ShouldReturnOk()
    {
        var partialUpdate = new { title = "Patched Title" };

        var response = apiClient.ExecuteRequest<PostResponse>(
            "posts/1", Method.Patch, partialUpdate);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Data.Title, Is.EqualTo(partialUpdate.title));
    }

    [Test]
    public void DeletePost_ShouldReturnOk()
    {
        var response = apiClient.ExecuteRequest("posts/1", Method.Delete);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public void FullPostLifecycle_ShouldWorkCorrectly()
    {
        var newPost = testData.GeneratePost();
        var create = apiClient.ExecuteRequest<PostResponse>("posts", Method.Post, newPost);

        Assert.That(create.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var id = create.Data.Id;

        var update = apiClient.ExecuteRequest<PostResponse>(
            $"posts/{id}", Method.Put, testData.GenerateUpdatedPost());

        Assert.That(update.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var patch = apiClient.ExecuteRequest<PostResponse>(
            $"posts/{id}", Method.Patch, new { body = "New patched body" });

        Assert.That(patch.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var delete = apiClient.ExecuteRequest($"posts/{id}", Method.Delete);

        Assert.That(delete.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}

## Вариант 1: JSONPlaceholder — Posts CRUD

### Endpoints
- POST /posts
- PUT /posts/{id}
- PATCH /posts/{id}
- DELETE /posts/{id}

### Используемые технологии
- .NET 7
- RestSharp 110+
- NUnit
- Bogus (генерация тестовых данных)

### Команда запуска
dotnet test

