# Coding Standards

Here you will find some documentation on the coding standards to follow for this specific project.

## No harcoded values
We are actively trying to avoid the use of hardcoded values spread all around the code, instead, we have used the approach of a [Constants class](src/FairPlayCombinedSln/FairPlayCombined.Common/Constants.cs), 
in the Common project, any constant value needs to have a constant defined in some of the subclasses.

## No member variables
As part of generating a differentiated code signature we are purposely avoiding having member variables prefixed with "_", instead class will have private properties named using [PascalCase](https://www.theserverside.com/definition/Pascal-case).

All class properties must be private.

## Variables and properties names
The names of variables and properties must specify what the variable contents are: e.g. videosList, userVideos, etc.

## API Endpoints
* The methods must be named using a verb, e.g. "GetUserVideos"
* The methods must always have the parameter CancellationToken cancellationToken, as the last parameter.
* For small endpoints prfer using Minimal APIs.
* The methods must always have the Http Verb Attribute, with "[action]", so that the endpoint is generated with the same name of the method:
  * [HttpPost("[action]")]
  * [HttpPut("[action]")]
  * [HttpDelete("[action]")]
  * [HttpGet("[action]")]

## Accessing API Endpoints
* use a ClientService class in the "{MainProject}.ClientServices" project, use the same prefix than the one used in the Controller for a name
* Generate the client using Kiota.

## Blazor Pages & components
* Components that load data must have the "Loading" component so that the spinners are shown while data is being load.
* Razor pages will have the following order
  * @page
  * @attribute
  * @implements
  * @using
  * @inject (of interfaces)
  * @inject (of classes)
  * Main html code
  * @code section
 
    Check FairPlayTube's [Home Page](https://github.com/pticostaricags/FairPlayCombined/blob/main/src/FairPlayCombinedSln/FairPlayTube.SharedUI/Components/Pages/Home.razor)
    
    ![image](https://github.com/user-attachments/assets/90ec002a-c77d-4116-8cf8-8a4f3808cb89)


## SQL Tables
* Tables must not contain more than 20 columns, if a table is about to reach that many columns, chances are many columns can be converted into an "AdditionalAttributes" kind of table.

## Async Methods naming
* All async methods( except the endpoints ) must be suffixed with Async
* All aynsc methods must received a mandatory "CancellationToken cancellationToken" as the last parameter

## Methods invocation
* When invoking a method, used named parameters.
