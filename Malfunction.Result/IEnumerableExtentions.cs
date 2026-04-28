
namespace MalFunction.Result
{
    public static class IEnumerableExtentions
    {
        /// <summary>
        /// Convert a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<List<TPass>, List<TFail>> Sequence<TPass, TFail>(this List<IResult<TPass, TFail>> enumerable) => 
            Traverse(enumerable.AsEnumerable(), x => x)
            .Map(x => x.ToList())
            .MapFail(x => x.ToList());

        /// <summary>
        /// Convert a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<TPass[], TFail[]> Sequence<TPass, TFail>(this IResult<TPass, TFail>[] enumerable) => 
            Traverse(enumerable.AsEnumerable(), x => x)
            .Map(x => x.ToArray())
            .MapFail(x => x.ToArray());

        /// <summary>
        /// Convert a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<IEnumerable<TPass>, IEnumerable<TFail>> Sequence<TPass, TFail>(this IEnumerable<IResult<TPass, TFail>> enumerable) =>
            Traverse(enumerable, x => x);

        /// <summary>
        /// Convert a collection of results into a single result containing a collection and aggregate the fails
        /// </summary>
        public static IResult<TPass[], TFail> Sequence<TPass, TFail>(this IResult<TPass, TFail>[] enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable.AsEnumerable(), x => x, failAggregator)
            .Map(x => x.ToArray());

        /// <summary>
        /// Convert a collection of results into a single result containing a collection and aggregate the fails
        /// </summary>
        public static IResult<List<TPass>, TFail> Sequence<TPass, TFail>(this List<IResult<TPass, TFail>> enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable.AsEnumerable(), x => x, failAggregator)
            .Map(x => x.ToList());

        /// <summary>
        /// Convert a collection of results into a single result containing a collection and aggregate the fails
        /// </summary>
        public static IResult<IEnumerable<TPass>, TFail> Sequence<TPass, TFail>(this IEnumerable<IResult<TPass, TFail>> enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable, x => x, failAggregator);

        /// <summary>
        /// Convert a collection of results into a single result containing a collection and aggregate the fails
        /// </summary>
        public static IResult<IEnumerable<TPass>, TAccumulate> Sequence<TPass, TFail, TAccumulate>(this IEnumerable<IResult<TPass, TFail>> enumerable, Func<TFail, TAccumulate> seed, Func<TAccumulate, TFail, TAccumulate> failAggregator) =>
            Traverse(enumerable, x => x, seed, failAggregator);

        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<List<TPass>, List<TFail>> Traverse<T, TPass, TFail>(this List<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc) => 
            Traverse(enumerable.AsEnumerable(), resultFunc)
            .Map(x => x.ToList())
            .MapFail(x => x.ToList());

        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<TPass[], TFail[]> Traverse<T, TPass, TFail>(this T[] enumerable, Func<T, IResult<TPass, TFail>> resultFunc) => 
            Traverse(enumerable.AsEnumerable(), resultFunc)
            .Map(x => x.ToArray())
            .MapFail(x => x.ToArray());

        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<IEnumerable<TPass>, IEnumerable<TFail>> Traverse<T, TPass, TFail>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc) =>
            Traverse(enumerable, resultFunc, x => new TFail[] { x }.AsEnumerable(), (x, y) => [.. x, y]);

        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<TPass[], TFail> Traverse<T, TPass, TFail>(this T[] enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable, resultFunc, failAggregator);
        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<List<TPass>, TFail> Traverse<T, TPass, TFail>(this List<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable, resultFunc, failAggregator);
        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<IEnumerable<TPass>, TFail> Traverse<T, TPass, TFail>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator) =>
            Traverse(enumerable, resultFunc, x => x, failAggregator);

        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<TPass[], TAccumulate> Traverse<T, TPass, TFail, TAccumulate>(this T[] enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TAccumulate> seed, Func<TAccumulate, TFail, TAccumulate> failAggregator) =>
            Traverse(enumerable.AsEnumerable(), resultFunc, seed, failAggregator)
            .Map(x => x.ToArray());
        
        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<List<TPass>, TAccumulate> Traverse<T, TPass, TFail, TAccumulate>(this List<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TAccumulate> seed, Func<TAccumulate, TFail, TAccumulate> failAggregator) => 
            Traverse(enumerable.AsEnumerable(), resultFunc, seed, failAggregator)
            .Map(x => x.ToList());
       
        /// <summary>
        /// Map a result producing function over a list to get a new result, and aggregate the fails
        /// </summary>
        public static IResult<IEnumerable<TPass>, TAccumulate> Traverse<T, TPass, TFail, TAccumulate>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TAccumulate> seed, Func<TAccumulate, TFail, TAccumulate> failAggregator)
        {
            return enumerable.Aggregate(new IResult<IEnumerable<TPass>, TAccumulate>.Pass([]) as IResult<IEnumerable<TPass>, TAccumulate>, (current, next) => (current, resultFunc(next)) switch
            {
                (IResult<IEnumerable<TPass>, TAccumulate>.Pass pass, IResult<TPass, TFail>.Pass newPass) =>
                    new IResult<IEnumerable<TPass>, TAccumulate>.Pass(pass.Value.Append(newPass.Value)),

                (IResult<IEnumerable<TPass>, TAccumulate>.Fail fail, IResult<TPass, TFail>.Pass) =>
                    fail,

                (IResult<IEnumerable<TPass>, TAccumulate>.Pass pass, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, TAccumulate>.Fail(seed(newFail.Value)),

                (IResult<IEnumerable<TPass>, TAccumulate>.Fail fail, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, TAccumulate>.Fail(failAggregator(fail, newFail)),

                _ => throw new Exception("Unexpected result")
            });
        }
    }
}