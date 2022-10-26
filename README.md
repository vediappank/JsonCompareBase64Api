1.JsonBase64Diff
===================================================================

Provide 2 Api Rest that accepts JSON base64 encoded binary data on both endpoints and a third to return the differences between them.

2.How it works
===================================================================

Provide 2 Rest Api endpoints that receive JSON base64 encoded binary data on both endpoints;

3.Test Client Url
=====================================================================
http://localhost:10872/swagger/index.html

4.1st End Point for left post method
=====================================================================
[POST]
http://localhost:10872/Compare/1/left

{
  "data": "YXNkZmFzZGZhc2RmYXNkZmFzZGY="
}

[Result] 201
=====================================================================
{
  "message":"ok"
}

5.2nd End Point for right post method
=====================================================================
[POST] 
http://localhost:10872/Compare/1/right

{
  "data": "YXNkZmFzZGZhc2RmYXNkZmFzZGY="
}

[Result] 201
=====================================================================
{
  "message":"ok"
}

5.3rd Endpoint for Json Compare Result 
=====================================================================
Provide a endpoint for diff comparison between them.

Get: http://localhost:10872/Compare/1/Difference

The results provide the following info in JSON format:

Jsons are equal

Jsons are same size

Jsons are different size

Jsons are same size but with differences

[GET] 
http://localhost:10872/Compare/1/Difference

[Result] 200
=====================================================================
{
  "message":"The data is the same",
}

6.Technologies
========================
Aspnet Core Api 3.1

WebApi

MemoryCache

Swagger for documentation

7.Suggestion to improve
==============================
Change InMemory database for a relational or MS SQL Server database

Put a cache in the Api layer

Distribute the application in containers Docker

Use an Api Gateway or create an Oauth server for Authentication and Authentication


