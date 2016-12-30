﻿using System;

namespace Monads {
    public class Maybe<T> {
        public static Maybe<T> Nothing => new Maybe<T>();
        public bool HasValue { get; }
        public T Value { get; }

        public Maybe() {
            HasValue = false;
        }

        public Maybe(T value) {
            Value = value;
            HasValue = value != null;
        }

        public override string ToString() => HasValue ? Value.ToString() : "Nothing";
    }

    public static class Maybe {
        public static Maybe<T> ToMaybe<T>(this T t) => new Maybe<T>(t);

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> source, Func<T, TResult> func) => source.HasValue
            ? new Maybe<TResult>(func(source.Value))
            : Maybe<TResult>.Nothing;

        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> source,
            Func<T, Maybe<TResult>> func) => source.HasValue ? func(source.Value) : Maybe<TResult>.Nothing;

        public static Maybe<TResult> SelectMany<T, T2, TResult>(this Maybe<T> source, Func<T, Maybe<T2>> func,
            Func<T, T2, TResult> func2) {
            if (!source.HasValue) return Maybe<TResult>.Nothing;
            var maybe = func(source.Value);
            return maybe.HasValue ? new Maybe<TResult>(func2(source.Value, maybe.Value)) : Maybe<TResult>.Nothing;
        }
    }
}