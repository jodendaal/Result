namespace OperationResult.Tests
{
    public class OperationResultTests
    {
        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void Value_IsSet_WithImplicitOperatorInitialisation()
        {
            var testService = new TestService();
            var result = testService.InitialiseAsReturnType_WithImplicitOperator();

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.That(result.Value.Id, Is.EqualTo("1"));
        }

        [Test]
        public void Result_IsSetAndSucessIsTrue_WithImplicitOperator()
        {
            var testService = new TestService();
            var result = testService.GetData();

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.Id);
        }

        [Test]
        public void ErrorType_IsNoneWhenSuccess()
        {
            var testService = new TestService();
            var result = testService.GetData();

            Assert.True(result.ErrorType == ErrorType.None);
        }

        [Test]
        public void ErrorType_IsExceptionWhenExceptionIsSet()
        {
            var testService = new TestService();
            var result = testService.GetDataWithException();

            Assert.True(result.ErrorType == ErrorType.Exception);
        }

        [Test]
        public void ErrorType_IsValidationWhenValidationIsSet()
        {
            var testService = new TestService();
            var result = testService.GetDataWithValidationErrorDictionary();

            Assert.True(result.ErrorType == ErrorType.Validation);
        }

        [Test]
        public void Exception_IsSetAndSucessIsFalse_WhenError()
        {
            var testService = new TestService();
            var result = testService.GetDataWithException();

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Value);
            Assert.NotNull(result.Exception);
        }

        [Test]
        public void Validation_IsSetWhenUsingDictionary()
        {
            var testService = new TestService();
            var result = testService.GetDataWithValidationErrorDictionary();

            Assert.True(result.ErrorType == ErrorType.Validation);
            Assert.That(result.ValidationErrors.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Validation_IsSetWhenUsingSingleFieldArrayOfMesssges()
        {
            var testService = new TestService();
            var result = testService.GetDataWithValidationErrorSingleFieldArrayOfMesssges();

            Assert.True(result.ErrorType == ErrorType.Validation);
            Assert.That(result.ValidationErrors.Count(), Is.EqualTo(1));
            Assert.That(result.ValidationErrors["Id"].Count(), Is.EqualTo(2));
        }

        [Test]
        public void Validation_IsSetWhenUsingSingleFieldAndSingleMessage()
        {
            var testService = new TestService();
            var result = testService.GetDataWithValidationErrorSingleFieldAndSingleMessage();

            Assert.True(result.ErrorType == ErrorType.Validation);
            Assert.That(result.ValidationErrors.Count(), Is.EqualTo(1));
        }
    }

    public class TestService
    {
        public Result<TestResult> GetData()
        {
            return new TestResult() { Id = "1"};
        }

        public Result<TestResult> GetDataWithException()
        {
            return Result<TestResult>.Failure(new Exception("An error occured"));
        }

        public Result<TestResult> GetDataWithValidationErrorDictionary()
        {
            var validationErorrs = new Dictionary<string, string[]>();
            validationErorrs.Add("Id",new string[] {"Id is required"});
            return Result<TestResult>.Validation(validationErorrs);
        }

        public Result<TestResult> GetDataWithValidationErrorSingleFieldArrayOfMesssges()
        {
            return Result<TestResult>.Validation("Id",new string[] { "Id is required", "Id must be greater than 0" });
        }

        public Result<TestResult> GetDataWithValidationErrorSingleFieldAndSingleMessage()
        {
            return Result<TestResult>.Validation("Id", new string[] { "Id is required", "Id must be greater than 0" });
        }


        public Result<TestResult> InitialiseAsReturnType_WithImplicitOperator()
        {
            Result<TestResult> test = new TestResult() { Id="1"};
            return test;
        }

    }
    public class TestResult
    {
        public string Id { get; set; }

    }

}