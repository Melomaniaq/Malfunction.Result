
namespace MalFunction.Result
{
    public static class IEnumerableExtentions
    {
        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<List<TPass>, List<TFail>> Sequence<TPass, TFail>(this List<IResult<TPass, TFail>> enumerable) => 
            Traverse(enumerable.AsEnumerable(), x => x)
            .Map(x => x.ToList())
            .MapFail(x => x.ToList());

        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<TPass[], TFail[]> Sequence<TPass, TFail>(this IResult<TPass, TFail>[] enumerable) => 
            Traverse(enumerable.AsEnumerable(), x => x)
            .Map(x => x.ToArray())
            .MapFail(x => x.ToArray());

        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<IEnumerable<TPass>, IEnumerable<TFail>> Sequence<TPass, TFail>(this IEnumerable<IResult<TPass, TFail>> enumerable) =>
            Traverse(enumerable, x => x);

        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<TPass[], TFail> Sequence<TPass, TFail>(this IResult<TPass, TFail>[] enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable.AsEnumerable(), x => x, failAggregator)
            .Map(x => x.ToArray());

        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<List<TPass>, TFail> Sequence<TPass, TFail>(this List<IResult<TPass, TFail>> enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable.AsEnumerable(), x => x, failAggregator)
            .Map(x => x.ToList());

        /// <summary>
        /// Converts a collection of results into a single result containing a collection
        /// </summary>
        public static IResult<IEnumerable<TPass>, TFail> Sequence<TPass, TFail>(this IEnumerable<IResult<TPass, TFail>> enumerable, Func<TFail, TFail, TFail> failAggregator) => 
            Traverse(enumerable, x => x, failAggregator);

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
        public static IResult<IEnumerable<TPass>, IEnumerable<TFail>> Traverse<T, TPass, TFail>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc)
        {
            return enumerable.Aggregate(new IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Pass([]) as IResult<IEnumerable<TPass>, IEnumerable<TFail>>, (current, next) => (current, resultFunc(next)) switch
            {
                (IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Pass pass, IResult<TPass, TFail>.Pass newPass) =>
                    new IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Pass(pass.Value.Append(newPass.Value)),

                (IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Fail fail, IResult<TPass, TFail>.Pass) =>
                    fail,

                (IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Pass pass, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Fail([newFail.Value]),

                (IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Fail fail, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, IEnumerable<TFail>>.Fail([.. fail.Value, newFail.Value]),

                _ => throw new Exception("Unexpected result")

            });
        }

        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<TPass[], TFail> Traverse<T, TPass, TFail>(this T[] enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator) => Traverse(enumerable, resultFunc, failAggregator);
        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<List<TPass>, TFail> Traverse<T, TPass, TFail>(this List<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator) => Traverse(enumerable, resultFunc, failAggregator);
        /// <summary>
        /// Map a result producing function over a list to get a new result
        /// </summary>
        public static IResult<IEnumerable<TPass>, TFail> Traverse<T, TPass, TFail>(this IEnumerable<T> enumerable, Func<T, IResult<TPass, TFail>> resultFunc, Func<TFail, TFail, TFail> failAggregator)
        {
            return enumerable.Aggregate(new IResult<IEnumerable<TPass>, TFail>.Pass([]) as IResult<IEnumerable<TPass>, TFail>, (current, next) => (current, resultFunc(next)) switch
            {
                (IResult<IEnumerable<TPass>, TFail>.Pass pass, IResult<TPass, TFail>.Pass newPass) =>
                    new IResult<IEnumerable<TPass>, TFail>.Pass(pass.Value.Append(newPass.Value)),

                (IResult<IEnumerable<TPass>, TFail>.Fail fail, IResult<TPass, TFail>.Pass) =>
                    fail,

                (IResult<IEnumerable<TPass>, TFail>.Pass pass, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, TFail>.Fail(newFail.Value),

                (IResult<IEnumerable<TPass>, TFail>.Fail fail, IResult<TPass, TFail>.Fail newFail) =>
                    new IResult<IEnumerable<TPass>, TFail>.Fail(failAggregator(fail, newFail)),

                _ => throw new Exception("Unexpected result")
            });
        }
    }
}
