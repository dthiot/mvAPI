# README

Web API 2.0 Web Service allowing calls to Mutli-Value database subroutines (Stored Procedures). This version only supports Bluefinity's mv.NET. This is a Visual Studio 2017 project. This API does not implent the full RESTful API specification. It is designed to only allow calling of subroutines written in MultiValue. The API only has one method and therefore does not require updating the website each time a new subroutine is added to what can be called via the API. The MultiValue subroutines require two parameters, ARGS and RESULTS which can be names anything you want and handled any way you want. ARGS are what is used to pass information to the subroutine and RESULTS are the ouput. The response will be a JSON object with the parent of ApiResponse. See samples below.

After you have downloaded the files extract the zip file to a directory open the Visual Studio solution. There are several configuration items to set in the web.config file:

### How do I get set up?

- Download and extract the zip file to a directory
- Open the solution with Visual Studio

* Modify the following parameters in web.config
  - mvNetLogin - The mv.NET login profile
  - UserName - the username used in the authentication header of the request
  - Password - the password used in teh authentication header of the request
  - mvNetSubPrefix - the string to prepend to the name of the subroutine in your MultiValue system for each call through mvAPI
  - TlsRequest - true/false to enable TLS for the API. If you don't have a security certificate this will be false

- Run the solution in Visual Studio or deploy as a website.

## Sample MultiValue HELLO.WORLD Subroutine

**NOTE: The subroutine must be named with the prefix mvNetSubPrefix but the API call does not use the prefix as that is added in code to control what routines can be called through the API.**

SUBROUTINE API.HELLO.WORLD (args, results) \*
results = '{"Response": "Hello ':args:'",'
results := '"Date": "':OCONV(DATE(),'D4-'):'"}'
RETURN
END

## Calling HELLO.WORLD via the API

Using a tool such as Postman (https://www.getpostman.com/) or Visual Studio Code extension "REST Client" set an authentication header using UserName and Password encoded for base64. If you are using Visual Studio Code another handy extension when dealing with RESTful API calls is vscode-base64 that will encode and decode base64 strings. Below is an example for Visual Studio Code wth the REST Client:

POST http://localhost:60843/api/ExecSP
Authorization: Basic bXZOZXRBcGk6QXBpUGFzc3dvcmQ=
Content-Type: application/json
Accept: application/json

{
"SubName": "HELLO.WORLD",
"Params": "John Smith"
}

## Response from the Sample API Call

{
"ApiResponse": {
"Response": "Hello John Smith",
"Date": "04-23-2019"
}
}
