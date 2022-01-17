using System;

namespace TaskCommon.Helpers
{
    public sealed class LogHelper
    {
        public static string GetErrorMessage(string args, Exception ex)
        {
            string innerMsg = ex.InnerException == null ? string.Empty : ex.InnerException.Message;

            string result = string.Format("#Параметры метода: {0}# Исключение: {1}# Внутреннее исключение: {2}# StackTrace: {3}"
                , args
                , ex.Message
                , innerMsg
                , ex.StackTrace);

            return result;
        }
    }
}
