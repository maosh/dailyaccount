using System;
using System.Data;
using ADODB;
using System.Reflection;

namespace dailyAccount
{
    public sealed class ADONETtoADO
    {
        /// <summary>
        /// 将DataTable对象转换为Recordeset对象
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <returns>转换后得到的Recordeset对象</returns>
        public static Recordset ConvertDataTableToRecordset(DataTable table)
        {
            Recordset rs = new Recordset();
            //DataRow dr;
            foreach (DataColumn dc in table.Columns)
            {
                rs.Fields.Append(dc.ColumnName, GetDataType(dc.DataType), -1, FieldAttributeEnum.adFldIsNullable);
            }
            rs.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified,
                 LockTypeEnum.adLockUnspecified, -1);

            foreach (DataRow dr in table.Rows)
            {
                rs.AddNew(Missing.Value, Missing.Value); object o;
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    rs.Fields[i].Value = dr[i];
                    o = rs.Fields[i].Value;
                }
            }

            return rs;
        }

        /// <summary>

        /// 将ADO.NET的数据类型转换为ADO的数据类型

        /// </summary>

        /// <param name="dataType">ADO.NET的数据类型</param>

        /// <returns>ADO的数据类型</returns>

        public static DataTypeEnum GetDataType(Type dataType)
        {
            switch (dataType.ToString())
            {
                case "System.Boolean": return DataTypeEnum.adBoolean;
                case "System.Byte": return DataTypeEnum.adUnsignedTinyInt;
                case "System.Char": return DataTypeEnum.adChar;
                case "System.DateTime": return DataTypeEnum.adDate;
                case "System.Decimal": return DataTypeEnum.adDecimal;
                case "System.Double": return DataTypeEnum.adDouble;
                case "System.Int16": return DataTypeEnum.adSmallInt;
                case "System.Int32": return DataTypeEnum.adInteger;
                case "System.Int64": return DataTypeEnum.adBigInt;
                case "System.SByte": return DataTypeEnum.adTinyInt;
                case "System.Single": return DataTypeEnum.adSingle;
                case "System.String": return DataTypeEnum.adVarChar;
                //case "TimeSpan":return DataTypeEnum.
                case "System.UInt16": return DataTypeEnum.adUnsignedSmallInt;
                case "System.UInt32": return DataTypeEnum.adUnsignedInt;
                case "System.UInt64": return DataTypeEnum.adUnsignedBigInt;
                default: throw (new Exception("没有对应的数据类型"));
            }
        }
    }
}
