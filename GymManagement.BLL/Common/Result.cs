using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result (bool Success , string? error = null , ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new(true);
        public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFound);
        public static Result Fail (string error , ResultKind kind = ResultKind.Confilict) => new(false, error, kind);
        public static Result Validation(string error) => new(false, error, ResultKind.ValidationFailed);
    }

    public sealed record Result<T>(bool Success , T? Value , string? error = null , ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok (T Value) => new(true , Value);
        public static Result<T> NotFound(string error = "Not Found") => new(false, default, error, ResultKind.NotFound);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Confilict) => new(false, default, error, kind);
        public static Result<T> Validation(string error) => new(false, default, error, ResultKind.ValidationFailed);
    }
}
