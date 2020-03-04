using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Library
{
    public class Utils
    {
        /// <summary>
        /// Lấy danh sách tham số của store để truyền dữ liệu
        /// </summary>
        /// <param name="cmd">Đối tượng command cần truyền tham số</param>
        /// <param name="dataType">Loại dữ liệu</param>
        /// <param name="data">data của đối tượng</param>
        /// created by: dtthao 08.12.2018
        public static void MappingParameter(SqlCommand cmd, object procParam)
        {
            /*
             * 1. Lấy danh sách các tham số của store
             * 2. Duyệt qua danh sách tham số, kiểm tra xem kiểu dataType có tham số trùng tên với tham số hay không
             * 3. Nếu có thì lấy dữ liệu của property đó của data để truyền vào cmd
             * 4. Nếu không có thì throw exeption báo không đủ tham số
             */
            SqlCommandBuilder.DeriveParameters(cmd);

            string paramName = string.Empty;
            for (int i = 1; i < cmd.Parameters.Count; ++i)
            {
                var param = cmd.Parameters[i];
                paramName = param.ParameterName.Substring(1);
                var propInfo = procParam.GetType().GetProperty(paramName);
                if (propInfo != null)
                {
                    var value = propInfo.GetValue(procParam);

                    if (value != null)
                    {
                        param.Value = value;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                }
                else
                {
                    throw new Exception("Không đủ tham số");
                }
            }
        }

        public static void AutoMapingObject<T>(SqlDataReader reader, ref T oObject)
        {
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                string fieldName = reader.GetName(i);
                reader.GetValue(i);
                if (oObject.GetType().GetProperty(fieldName) != null && reader[fieldName] != DBNull.Value)
                {
                    oObject.GetType().GetProperty(fieldName).SetValue(oObject, reader[fieldName]);
                }
            }
        }
    }
}