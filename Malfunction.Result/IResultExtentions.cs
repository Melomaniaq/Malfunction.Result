
namespace Malfunction.Result
{
    public static class IResultExtentions
    {
        /// <summary>
        /// Projects result's pass value into a new form
        /// </summary>
        public static IResult<TResult, TFail> Map<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TPass, TResult> func) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TResult, TFail>.Pass(func(pass.Value)),
            IResult<TPass, TFail>.Fail fail => new IResult<TResult, TFail>.Fail(fail.Value),
            _ => throw new Exception("Unexpected result")
        };

        /// <summary>
        /// Projects result's pass value into a new form
        /// </summary>
        public static IResult<TResult, TFail> Map<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TPass, IResult<TResult, TFail>> func) => result switch
        {
            IResult<TPass, TFail>.Pass pass => func(pass.Value),
            IResult<TPass, TFail>.Fail fail => new IResult<TResult, TFail>.Fail(fail.Value),
            _ => throw new Exception("Unexpected result")
        };

        /// <summary>
        /// Projects result's fail value into a new form
        /// </summary>
        public static IResult<TPass, TResult> MapFail<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TFail, TResult> func) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TPass, TResult>.Pass(pass.Value),
            IResult<TPass, TFail>.Fail fail => new IResult<TPass, TResult>.Fail(func(fail.Value)),
            _ => throw new Exception("Unexpected result")
        };

        /// <summary>
        /// Projects result's fail value into a new form
        /// </summary>
        public static IResult<TPass, TResult> MapFail<TPass, TFail, TResult>(this IResult<TPass, TFail> result, Func<TFail, IResult<TPass, TResult>> func) => result switch
        {
            IResult<TPass, TFail>.Pass pass => new IResult<TPass, TResult>.Pass(pass.Value),
            IResult<TPass, TFail>.Fail fail => func(fail.Value),
            _ => throw new Exception("Unexpected result")
        };
    }
}
