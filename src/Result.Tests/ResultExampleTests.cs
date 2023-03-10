using Newtonsoft.Json;

namespace Result.Tests
{
    public class ResultExampleTests
    {
        ExampleUsage _exampleUsage = new ExampleUsage();
        
        [Test]
        public void SuccessResult()
        {
           var result = _exampleUsage.Success();

            Console.Write(JsonConvert.SerializeObject(result));
        }

        [Test]
        public void ValidationResult()
        {
            var result = _exampleUsage.Validation();

            Console.Write(JsonConvert.SerializeObject(result));
        }

        [Test]
        public void ExceptionResult()
        {
            var result = _exampleUsage.Exception();

            Console.Write(JsonConvert.SerializeObject(result,new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }

        [Test]
        public void ErrorResult()
        {
            var result = _exampleUsage.ErrorResult();

            Console.Write(JsonConvert.SerializeObject(result, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

    }

    public class ExampleUsage
    {
        public Result<TestResponseObject> Success()
        {
            var result = new TestResponseObject("1");
            return result;
        }

        public Result<TestResponseObject> Validation()
        {
            var result = Result<TestResponseObject>.Validation("Id", "Must be greater than 1");
            return result;
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
        }

        public Result<TestResponseObject> ErrorResult()
        {
            return Result<TestResponseObject>.Failure("MyBusinessError","You cannot do this");
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

        //public Result<TestResponseObject> RealUsage(int number)
        //{
        //    var result = RealResponse(1);

        //    if(result.IsSuccess)
        //    {
        //       // return OK();
        //    }
        //    else if(result.ErrorType == FailureType.Validation)
        //    {
        //        //Show validation
        //       // return Problem(result.ValidationErrors);
        //    }
        //    else
        //    {
        //        //Log Error
        //        //_logger.log(result.Error)
        //      //  return InternalServerError();
        //    }
        //}
    }
   
       
    public class TestResponseObject
    {
        public TestResponseObject(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

}