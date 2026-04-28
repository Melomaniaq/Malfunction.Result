# Malfunction.Result
[![NuGet Version](https://img.shields.io/nuget/v/Malfunction.Result?style=flat)](https://www.nuget.org/packages/MalFunction.Result)

Functional result type for C#

# Getting Started
The result type is an interface that contains two records, Pass and Fail
```cs
  public interface IResult<TPass, TFail>
  {
      public sealed record Pass(TPass Value) : IResult<TPass, TFail>;

      public sealed record Fail(TFail Value) : IResult<TPass, TFail>;
  }
```

To define a result type, make a type alias. If using it across mulpiple files, define it globally in a seperate file
```cs
global using ValidationResult = MalFunction.Result.IResult<int, string>;
```

Set the return value of your method to your result and return a new pass or fail state
```cs
public ValidationResult IsGreaterThan5(int number)
{
    if (number > 5)
        return new ValidationResult.Pass(number);
    else
        return new ValidationResult.Fail("Number was not greater than 5");
}
```

To handle a result, make a type check and handle the success and fail states
```cs
string message = IsGreaterThan5(6) switch
{
    ValidationResult.Pass pass -> $"Number {pass.Value} was validated",
    ValidationResult.Fail fail -> $"Number failed validation with error: {fail.Value}"
};

Console.Writeline(message);
```

# Manipulating Results
## Map
Projects result's value into a new form
```cs
IResult<int, string> intResult = new IResult<int, string>.Pass(20);
IResult<string, string> stringResult = intResult.Map(x => x.ToString());
```
## Bind
Projects result's value into a new result
```cs
IResult<int, string> intResult = new IResult<int, string>.Pass(20);
IResult<string, string> stringResult = intResult.Map(x => new IResult<string, string>.Pass(x.ToString()));
```
## Traverse
Map a result producing function over a list to get a new result
```cs
int[] values = [1, 2, 3];

IResult<int, string> NumberMustBeEven()
{
    if (x % 2 = 0)
        return new IResult<int[], string[]>.Pass(number);
    else
        return new IResult<int[], string[]>.Fail("Number was odd")
}

IResult<int[], string[]> result = values.Traverse(NumberMustBeEven);

//using fail aggregator
IResult<int[], string> result = values.Traverse(NumberMustBeEven, (error1, error2) => error1 + error2);

//using fail aggregator with projection
record Error(string[] Errors)
IResult<int[], Error> result = results.Traverse(
    NumberMustBeEven,
    message => new Error([message]),
    (error, message) => error.Errors.Append(message)
);
```
## Sequence
Converts a collection of results into a single result containing a collection
```cs
IResult<int, string>[] results =
[
    new IResult<int, string>.Pass(12),
    new IResult<int, string>.Pass(20),
    new IResult<int, string>.Fail("error")
];

IResult<int[], string[]> result = results.Sequence();

//using fail aggregator
IResult<int[], string> result = results.Sequence((error1, error2) => error1 + error2);

//using fail aggregator with projection
record Error(string[] Errors)
IResult<int[], Error> result = results.Sequence(
    message => new Error([message]),
    (error, message) => error.Errors.Append(message)
);
```

For a detailed explenation on how to use elevated worlds like this result type i recommend you read:
https://fsharpforfunandprofit.com/posts/elevated-world/
