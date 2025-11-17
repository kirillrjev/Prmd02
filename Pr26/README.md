## Практическая работа 26. Десериализация JSON-ответов в объекты C#
Тема:
Десериализация JSON-ответов REST API в объекты C# с использованием System.Text.Json и Newtonsoft.Json

Цель работы
Получить практические навыки десериализации JSON-данных, создания моделей, обработки вложенных структур, применения кастомных конвертеров и сравнения двух JSON-библиотек .NET.

Задачи
Изучить основы работы с JSON в .NET.
Научиться десериализовать JSON в объекты C# с помощью System.Text.Json.
Освоить десериализацию через Json.NET (Newtonsoft.Json).
Разработать модели данных для простых и вложенных JSON-структур.
Реализовать тесты для проверки корректности десериализации.
Добавить кастомные JSON-конвертеры.
Выполнить валидацию данных и обработку ошибок.
## Основы JSON-десериализации
System.Text.Json
var user = JsonSerializer.Deserialize<User>(jsonString);

Newtonsoft.Json
var user = JsonConvert.DeserializeObject<User>(jsonString);
## Атрибуты управления десериализацией
public class User
{
    [JsonPropertyName("id")]     // System.Text.Json
    [JsonProperty("id")]         // Newtonsoft.Json
    public int Id { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
}
## Модели (Вариант 1: JSONPlaceholder Users)

Полностью соответствуют JSONPlaceholder API.

## Models/Simple/User.cs
using System.Text.Json.Serialization;

namespace JsonDeserializationTests.Models.Simple
{
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

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
## Models/Complex/ComplexUser.cs 
using System.Text.Json.Serialization;

namespace JsonDeserializationTests.Models.Complex
{
    public class ComplexUser
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
}
## Address.cs 
public class Address
{
    public string Street { get; set; }
    public string Suite { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
    public Geo Geo { get; set; }
}
## Geo.cs
public class Geo
{
    public string Lat { get; set; }
    public string Lng { get; set; }
}
## Company.cs
public class Company
{
    public string Name { get; set; }
    public string CatchPhrase { get; set; }
    public string Bs { get; set; }
}
## System.Text.Json
public class SystemTextJsonService : IJsonDeserializer
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonService()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, _options);
}
## Newtonsoft.Json
public class NewtonsoftJsonService : IJsonDeserializer
{
    private readonly JsonSerializerSettings _settings;

    public NewtonsoftJsonService()
    {
        _settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }

    public T Deserialize<T>(string json)
        => JsonConvert.DeserializeObject<T>(json, _settings);
}
## Tests/BasicDeserializationTests.cs
[TestFixture]
public class BasicDeserializationTests
{
    private SystemTextJsonService _system;
    private NewtonsoftJsonService _newtonsoft;

    [SetUp]
    public void Setup()
    {
        _system = new SystemTextJsonService();
        _newtonsoft = new NewtonsoftJsonService();
    }

    [Test]
    public void DeserializeUser_SystemTextJson_ReturnsValidModel()
    {
        var json = JsonSamples.SingleUserJson;

        var user = _system.Deserialize<User>(json);

        Assert.That(user.Id, Is.EqualTo(1));
        Assert.That(user.Name, Is.EqualTo("Leanne Graham"));
    }

    [Test]
    public void DeserializeUsers_Array_ShouldReturnList()
    {
        var users = _system.Deserialize<List<User>>(JsonSamples.UsersArrayJson);

        Assert.That(users.Count, Is.EqualTo(2));
        Assert.That(users[0].Username, Is.EqualTo("Bret"));
    }
}
## Tests/ComplexDeserializationTests.cs
[TestFixture]
public class ComplexDeserializationTests
{
    private SystemTextJsonService _service;

    [SetUp]
    public void Setup()
    {
        _service = new SystemTextJsonService();
    }

    [Test]
    public void Deserialize_ComplexUser_ReturnsNestedObjects()
    {
        var user = _service.Deserialize<ComplexUser>(JsonSamples.ComplexUserJson);

        Assert.That(user.Address.City, Is.EqualTo("Gwenborough"));
        Assert.That(user.Address.Geo.Lat, Is.EqualTo("-37.3159"));
        Assert.That(user.Company.Name, Is.EqualTo("Romaguera-Crona"));
    }
}
## Кастомный JSON-конвертер
Например, преобразование строки в дату.
public class CustomDateConverter : JsonConverter<DateTime>
{
    private readonly string format = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o)
        => DateTime.ParseExact(reader.GetString(), format, null);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions o)
        => writer.WriteStringValue(value.ToString(format));
}
## 