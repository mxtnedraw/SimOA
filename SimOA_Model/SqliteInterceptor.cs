using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimOA_Model
{
    public partial class SqliteInterceptor : IDbCommandInterceptor
    {
        private static Regex replaceRegex = new Regex(@"\(\(CHARINDEX\((.*?), (.*?)\)\) > 0\)");
        private void ReplaceCharIndexFunc(DbCommand command)
        {
            var flag = false;
            var text = replaceRegex.Replace(command.CommandText, m =>
            {
                if (!m.Success) return m.Value;
                flag = true;
                var key = m.Groups[1].Value;
                var name = m.Groups[2].Value;
                //替换参数
                foreach (DbParameter commandParameter in command.Parameters)
                {
                    if (commandParameter.ParameterName == key.Substring(1))
                    {
                        commandParameter.Value = "%" + commandParameter.Value + "%";
                        break;
                    }
                }
                return name + " LIKE " + (key.Contains("'") ? "'%" + key.Substring(1,key.Length-2) + "%'" : key);
            });
            if (flag)
                command.CommandText = text;
        }
        public void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

        }

        public void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

        }

        public void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {

        }

        public void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            if (command.CommandText.Contains("CHARINDEX"))
            {
                ReplaceCharIndexFunc(command);
            }
        }

        public void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

        }

        public void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

        }
    }
}
