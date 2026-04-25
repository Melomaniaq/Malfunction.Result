namespace Malfunction.Result
{
    public interface IResult<TPass, TFail>
    {
        public sealed record Pass(TPass Value) : IResult<TPass, TFail>
        {
            public static implicit operator TPass(Pass pass) => pass.Value;
        }

        public sealed record Fail(TFail Value) : IResult<TPass, TFail>
        {
            public static implicit operator TFail(Fail fail) => fail.Value;

        }
    }
}

