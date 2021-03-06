using System.Collections.Generic;
using System.Collections.Immutable;

namespace Pidgin
{
    public static partial class Parser<TToken>
    {
        /// <summary>
        /// Creates a parser which parses the end of the input stream
        /// </summary>
        /// <returns>A parser which parses the end of the input stream and returns <see cref="Unit.Value"/></returns>
        // todo: make me a property, not a method
        public static Parser<TToken, Unit> End() => new EndParser();

        private sealed class EndParser : Parser<TToken, Unit>
        {
            private static readonly ImmutableSortedSet<Expected<TToken>> _expected
                = ImmutableSortedSet.Create(new Expected<TToken>());

            public EndParser() : base(_expected)
            {
            }

            internal sealed override InternalResult<Unit> Parse(ref ParseState<TToken> state)
            {
                var result = state.Peek();
                if (result.HasValue)
                {
                    state.Error = new ParseError<TToken>(
                        result,
                        false,
                        Expected,
                        state.SourcePos,
                        null
                    );
                    return InternalResult.Failure<Unit>(false);
                }
                return InternalResult.Success(Unit.Value, false);
            }
        }
    }
}