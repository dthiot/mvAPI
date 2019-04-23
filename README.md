# README #

Web API 2.0 Web Service allowing call to Mutli-Value database subroutines (Stored Procedures). This version only supports Bluefinity's mv.NET.  This is a Visual Studio 2017 project.

After you have downloaded the files extract the zip file to a directory open the Visual Studio solution.  There are several configuration items to set in the web.config file:

### How do I get set up? ###

* Download and extract the zip file to a directory
* Open the solution with Visual Studio
+ Modify the following parameters in web.config
  * mvNetLogin - The mv.NET login profile
  * UserName - the username used in the authentication header of the request
  * Password - the password used in teh authentication header of the request
  * mvNetSubPrefix - the string to prepend to the name of the subroutine in your MultiValue system for each call through mvAPI
  * TlsRequest - true/false to enable TLS for the API.  If you don't have a security certificate this will be false
* Database configuration
* How to run tests
* Deployment instructions
