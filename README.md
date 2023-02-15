[![Build](https://github.com/jodendaal/Result/actions/workflows/main.yml/badge.svg)](https://github.com/jodendaal/Result/actions/workflows/main.yml) [![NuGet Badge](https://buildstats.info/nuget/Result.Match)](https://www.nuget.org/packages/Result.Match)

# Result

Simple library to assist with handling operation results, success, validation and exceptions.
Also includes a match method for easier handling of response.

# Getting started

Install package [Nuget package](https://www.nuget.org/packages/Result.Match)

```powershell
Install-Package Result.Match
```

# Example Usage

## Match
```csharp
return result.Match<IActionResult>(
                (success) => OkObjectResult(success),
                (error) => BadRequest(),
                (validation) => ValidationProblemDetails(validation)
                );
```

```csharp

public class ExampleUsage
{
    public Result<TestResponseObject> RealUsage(int number)
    {
        var result = RealResponse(1);

        if(result.IsSuccess)
        {
            return OK();
        }
        else if(result.ErrorType == FailureType.Validation)
        {
            //Show validation
            return Problem(result.ValidationErrors);
        }
        else
        {
            //Log Error
            //_logger.log(result.Error)
            return InternalServerError();
        }
    }

    public Result<TestResponseObject> RealResponse(int number)
    {
        try
        {
            if(number > 0)
            {
                return new TestResponseObject("1");
            }
            else
            {
                return Result<TestResponseObject>.Validation("number", "must be greater than 0");
            }
        }
        catch(Exception ex)
        {
            return Result<TestResponseObject>.Failure(ex);
        }
    }

    public Result<TestResponseObject> Success()
    {
        var result = new TestResponseObject("1");
        return result;
         //{"ValidationErrors":null,"Value":{"Id":"1"},"Error":null,"IsSuccess":true,"ErrorType":0}
    }

    public Result<TestResponseObject> Validation()
    {
        var result = Result<TestResponseObject>.Validation("Id", "Must be greater than 1");
        return result;
        //{"ValidationErrors":{"Id":["Must be greater than 1"]},"Value":null,"Error":null,"IsSuccess":false,"ErrorType":2}
    }

    public Result<TestResponseObject> Exception()
    {
        try
        {
            throw new Exception("Test exception message");
        }
        catch(Exception ex)
        {
            return Result<TestResponseObject>.Failure(ex);
        }
        //{"ValidationErrors":null,"Value":null,"Error":{"StackTrace":"   at OperationResult.Tests.ExampleUsage.Exception() in OperationResultExampleTests.cs:line 61","Exception":{ExceptionObject},"Code":"System.Exception","Message":"Test exception message","Type":1},"IsSuccess":false,"ErrorType":1}
    }

    public Result<TestResponseObject> ErrorResult()
    {
        return Result<TestResponseObject>.Failure("MyBusinessError","You cannot do this");
        // {"ValidationErrors":null,"Value":null,"Error":{"StackTrace":"at ErrorResult in OperationResultExampleTests.cs:line 71","Exception":null,"Code":"MyBusinessError","Message":"You cannot do this","Type":0},"IsSuccess":false,"ErrorType":1}
    }
    
}

public class TestResponseObject
{
    public TestResponseObject(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}
```

# Buy me a coffee 
https://www.buymeacoffee.com/timdoestech?new=1
