namespace Malfunction.Result
{
    public interface IResult<TPass, TFail>
    {
        public sealed record Pass(TPass Value) : IResult<TPass, TFail>;
        public sealed record Fail(TFail Value) : IResult<TPass, TFail>;
    }
}

