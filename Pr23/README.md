## Пример окружения Файл: jsonplaceholder-environment.json
{
  "id": "jsonplaceholder-env",
  "name": "JSONPlaceholder Environment",
  "values": [
    {
      "key": "baseUrl",
      "value": "https://jsonplaceholder.typicode.com",
      "type": "default",
      "enabled": true
    },
    {
      "key": "postId",
      "value": "",
      "type": "default",
      "enabled": true
    }
  ]
}
## Структура коллекции
Коллекция содержит 5 запросов:
GET {{baseUrl}}/posts
GET {{baseUrl}}/posts/1
POST {{baseUrl}}/posts
PUT {{baseUrl}}/posts/{{postId}}
DELETE {{baseUrl}}/posts/{{postId}}
## GET /posts
URL:
{{baseUrl}}/posts
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Response is an array", function () {
    var json = pm.response.json();
    pm.expect(json).to.be.an('array');
});

pm.test("First post has correct structure", function () {
    var json = pm.response.json();
    pm.expect(json[0]).to.have.keys(['userId', 'id', 'title', 'body']);
});

pm.test("Response time < 500ms", function () {
    pm.expect(pm.response.responseTime).to.be.below(500);
});
## GET /posts/1
URL:
{{baseUrl}}/posts/1
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Post has valid structure", function () {
    var json = pm.response.json();
    pm.expect(json).to.have.property("id", 1);
    pm.expect(json).to.have.keys(['userId', 'id', 'title', 'body']);
});
## POST /posts
URL:
{{baseUrl}}/posts
{
    "title": "Test Post",
    "body": "This is a test post content",
    "userId": 1
}
pm.test("Status code is 201", function () {
    pm.response.to.have.status(201);
});

pm.test("Response contains posted data", function () {
    var json = pm.response.json();
    pm.expect(json.title).to.eql("Test Post");
    pm.expect(json.body).to.eql("This is a test post content");
    pm.expect(json.userId).to.eql(1);
});

// Save returned ID for PUT & DELETE
pm.environment.set("postId", pm.response.json().id);
## PUT /posts/{{postId}}
URL:
{{baseUrl}}/posts/{{postId}}
{
    "id": {{postId}},
    "title": "Updated title",
    "body": "Updated body",
    "userId": 1
}
pm.test("Status is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Post updated", function () {
    var json = pm.response.json();
    pm.expect(json.title).to.eql("Updated title");
    pm.expect(json.body).to.eql("Updated body");
});
## DELETE /posts/{{postId}}
URL:
{{baseUrl}}/posts/{{postId}}
pm.test("Status is 200 or 204", function () {
    pm.expect([200, 204]).to.include(pm.response.code);
});
# Практическая работа №23
## Анализ API с помощью Postman
### Вариант 1: JSONPlaceholder — Posts API

---

## Цель
Протестировать API для работы с постами:
- GET /posts
- GET /posts/1
- POST /posts
- PUT /posts/1
- DELETE /posts/1

---

## Используемые возможности Postman
- Коллекции
- Переменные окружений
- JavaScript-тесты
- Автоматизация API-проверок
- Выполнение коллекции через Runner

