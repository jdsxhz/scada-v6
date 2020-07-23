﻿/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Represents a mechanism to read and write the configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Tables;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using static Scada.BinaryConverter;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write the configuration database tables.
    /// <para>Представляет механизм для чтения и записи таблиц базы конфигурации.</para>
    /// </summary>
    public class BaseTableAdapter : Adapter
    {
        /// <summary>
        /// Represents a field definition.
        /// </summary>
        protected class FieldDef
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public FieldDef(string name, byte dataType, bool allowNull)
            {
                Name = name ?? throw new ArgumentNullException("name");
                DataType = dataType;
                AllowNull = allowNull;

                switch (dataType)
                {
                    case DataTypeID.Integer:
                        DataSize = sizeof(int);
                        DefaultValue = 0;
                        break;
                    case DataTypeID.Double:
                        DataSize = sizeof(double);
                        DefaultValue = 0.0;
                        break;
                    case DataTypeID.Boolean:
                        DataSize = 1;
                        DefaultValue = false;
                        break;
                    case DataTypeID.DateTime:
                        DataSize = sizeof(long);
                        DefaultValue = (long)0;
                        break;
                    case DataTypeID.String:
                        DataSize = 0;
                        DefaultValue = "";
                        break;
                    default:
                        throw new ArgumentException("Data type is not supported.");
                }
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public FieldDef(string name, Type type, bool allowNull)
            {
                if (name == null)
                    throw new ArgumentNullException("name");
                if (name.Length > MaxFieldNameLength)
                    throw new ArgumentException("Name length exceeded.");

                Name = name;
                AllowNull = allowNull;

                if (type == typeof(int))
                {
                    DataType = DataTypeID.Integer;
                    DataSize = sizeof(int);
                    DefaultValue = 0;
                }
                else if (type == typeof(double))
                {
                    DataType = DataTypeID.Double;
                    DataSize = sizeof(double);
                    DefaultValue = 0.0;
                }
                else if (type == typeof(bool))
                {
                    DataType = DataTypeID.Boolean;
                    DataSize = 1;
                    DefaultValue = false;
                }
                else if (type == typeof(DateTime))
                {
                    DataType = DataTypeID.DateTime;
                    DataSize = sizeof(long);
                    DefaultValue = (long)0;
                }
                else if (type == typeof(string))
                {
                    DataType = DataTypeID.String;
                    DataSize = 0;
                    DefaultValue = "";
                }
                else
                {
                    throw new ArgumentException("Data type is not supported.");
                }
            }

            /// <summary>
            /// Gets or sets the field name.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the data type.
            /// </summary>
            public byte DataType { get; set; }
            /// <summary>
            /// Gets or sets the data size if it is fixed.
            /// </summary>
            public int DataSize { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether null values are possible.
            /// </summary>
            public bool AllowNull { get; set; }
            /// <summary>
            /// Gets the default field value.
            /// </summary>
            public object DefaultValue { get; protected set; }
        }

        /// <summary>
        /// Specifies the data type IDs.
        /// </summary>
        protected static class DataTypeID
        {
            /// <summary>
            /// Integer data type.
            /// </summary>
            public const byte Integer = 0;
            /// <summary>
            /// Floating point data type.
            /// </summary>
            public const byte Double = 1;
            /// <summary>
            /// Logical data type.
            /// </summary>
            public const byte Boolean = 2;
            /// <summary>
            /// Date and time.
            /// </summary>
            public const byte DateTime = 3;
            /// <summary>
            /// String data type.
            /// </summary>
            public const byte String = 4;
        }


        /// <summary>
        /// The table type.
        /// </summary>
        protected const ushort TableType = 2;
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 4;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 20;
        /// <summary>
        /// The field definition size in a file.
        /// </summary>
        protected const int FieldDefSize = 60;
        /// <summary>
        /// The maximum length allowed for a field name.
        /// </summary>
        protected const int MaxFieldNameLength = 50;
        /// <summary>
        /// The maximum length allowed for a field value.
        /// </summary>
        protected const int MaxFieldLenght = ushort.MaxValue;
        /// <summary>
        /// Indicates the beginning of a row.
        /// </summary>
        protected const ushort RowMarker = 0xAAFF;


        /// <summary>
        /// Writes the field definintion using the specified writer.
        /// </summary>
        protected void WriteFieldDef(FieldDef fieldDef, BinaryWriter writer, byte[] buffer)
        {
            Array.Clear(buffer, 0, FieldDefSize);
            int nameLength = fieldDef.Name.Length;
            buffer[0] = (byte)nameLength;
            Encoding.ASCII.GetBytes(fieldDef.Name).CopyTo(buffer, 1);
            buffer[nameLength + 2] = fieldDef.DataType;
            buffer[nameLength + 3] = (byte)(fieldDef.AllowNull ? 1 : 0);
            ushort crc = CalcCRC16(buffer, 0, FieldDefSize - 2);
            CopyUInt16(crc, buffer, FieldDefSize - 2);
            writer.Write(buffer);
        }

        /// <summary>
        /// Gets the field data to write.
        /// </summary>
        protected byte[] GetFieldData(FieldDef fieldDef, object value, byte[] buffer)
        {
            if (fieldDef.DataSize > 0 && (buffer == null || buffer.Length != fieldDef.DataSize))
                buffer = new byte[fieldDef.DataSize];

            switch (fieldDef.DataType)
            {
                case DataTypeID.Integer:
                    CopyInt32((int)value, buffer, 0);
                    break;
                case DataTypeID.Double:
                    CopyDouble((double)value, buffer, 0);
                    break;
                case DataTypeID.Boolean:
                    buffer[0] = (byte)value;
                    break;
                case DataTypeID.DateTime:
                    CopyTime((DateTime)value, buffer, 0);
                    break;
                case DataTypeID.String:
                    buffer = Encoding.UTF8.GetBytes((string)value);
                    if (buffer.Length > MaxFieldLenght)
                        throw new ArgumentException("String length exceeded.");
                    break;
                default:
                    buffer = EmptyBuffer;
                    break;
            }

            return buffer;
        }

        /// <summary>
        /// Gets the field value from the buffer.
        /// </summary>
        protected object GetFieldValue(int dataType, int dataSize, byte[] buffer, ref int index)
        {
            switch (dataType)
            {
                case DataTypeID.Integer:
                    return GetInt32(buffer, ref index);
                case DataTypeID.Double:
                    return GetDouble(buffer, ref index);
                case DataTypeID.Boolean:
                    return GetByte(buffer, ref index);
                case DataTypeID.DateTime:
                    return GetTime(buffer, ref index);
                case DataTypeID.String:
                    string s = Encoding.UTF8.GetString(buffer, index, dataSize);
                    index += dataSize;
                    return s;
                default:
                    return null;
            }
        }


        /// <summary>
        /// Fills the specified table by reading data from the configuration database.
        /// </summary>
        public void Fill(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = null;
            BinaryReader reader = null;

            baseTable.ClearItems();
            baseTable.IndexesEnabled = false;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // read and check header
                byte[] buffer = new byte[Math.Max(HeaderSize, FieldDefSize)];
                int bytesRead = reader.Read(buffer, 0, HeaderSize);

                if (bytesRead == 0)
                    return; // table is empty
                if (bytesRead < HeaderSize)
                    throw new ScadaException("Unexpected end of stream.");

                int index = 0;
                if (GetUInt16(buffer, ref index) != TableType)
                    throw new ScadaException("Invalid table type.");

                if (GetUInt16(buffer, ref index) != MajorVersion)
                    throw new ScadaException("Incompatible format version.");

                index += 2; // skip minor version
                int fieldCount = GetUInt16(buffer, ref index);

                // read field definitions
                FieldDef[] fieldDefs = new FieldDef[fieldCount];
                PropertyDescriptorCollection allProps = TypeDescriptor.GetProperties(baseTable.ItemType);
                PropertyDescriptor[] props = new PropertyDescriptor[fieldCount];

                for (int i = 0; i < fieldCount; i++)
                {
                    bytesRead = reader.Read(buffer, 0, FieldDefSize);

                    if (bytesRead < FieldDefSize)
                        throw new ScadaException("Unexpected end of stream.");

                    if (CalcCRC16(buffer, 0, FieldDefSize - 2) != BitConverter.ToUInt16(buffer, FieldDefSize - 2))
                        throw new ScadaException("Field definition CRC error.");

                    index = 0;
                    int nameLength = buffer[index++];
                    string fieldName = Encoding.ASCII.GetString(buffer, index, nameLength);
                    index += nameLength;

                    fieldDefs[i] = new FieldDef(
                        fieldName, 
                        GetByte(buffer, ref index), 
                        GetBool(buffer, ref index));

                    props[i] = allProps[fieldName]; // get a property that match the field definition
                }

                // read rows
                while (true)
                {
                    if (reader.ReadUInt16() != RowMarker)
                        throw new ScadaException("Row marker not found.");

                    int rowDataSize = reader.ReadInt32();
                    int fullRowSize = rowDataSize + 8;

                    if (buffer.Length < fullRowSize)
                        buffer = new byte[fullRowSize * 2];

                    bytesRead = reader.Read(buffer, 6, rowDataSize);

                    if (bytesRead < rowDataSize)
                        throw new ScadaException("Unexpected end of stream.");

                    if (CalcCRC16(buffer, 0, fullRowSize - 2) != BitConverter.ToUInt16(buffer, fullRowSize - 2))
                        throw new ScadaException("Row CRC error.");

                    // read fields
                    object item = baseTable.NewItem();
                    index = 6;

                    for (int i = 0; i < fieldCount; i++)
                    {
                        FieldDef fieldDef = fieldDefs[i];
                        PropertyDescriptor prop = props[i];
                        bool isNull = GetBool(buffer, ref index);
                        int dataSize = fieldDef.DataSize > 0 ? fieldDef.DataSize : GetUInt16(buffer, ref index);
                        object value = isNull ? null : GetFieldValue(fieldDef.DataType, dataSize, buffer, ref index);

                        if (prop != null)
                            prop.SetValue(item, value ?? (fieldDef.AllowNull ? null : fieldDef.DefaultValue));
                    }

                    baseTable.AddObject(item);
                }
            }
            catch (EndOfStreamException)
            {
                // normal file ending case
            }
            finally
            {
                reader?.Close();
                baseTable.IndexesEnabled = true;
            }
        }

        /// <summary>
        /// Updates the configuration database by writing data of the specified table.
        /// </summary>
        public void Update(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // write header
                writer.Write(TableType);
                writer.Write(MajorVersion);
                writer.Write(MinorVersion);

                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                ushort fieldCount = (ushort)Math.Min(props.Count, ushort.MaxValue);
                writer.Write(fieldCount);
                writer.Write(ReserveBuffer, 0, 12);

                if (fieldCount > 0)
                {
                    // create and write field definitions
                    FieldDef[] fieldDefs = new FieldDef[fieldCount];
                    byte[] buffer = new byte[FieldDefSize];

                    for (int i = 0; i < fieldCount; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        bool isNullable = prop.PropertyType.IsNullable();

                        FieldDef fieldDef = new FieldDef(
                            prop.Name,
                            isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                            isNullable || prop.PropertyType.IsClass);

                        fieldDefs[i] = fieldDef;
                        WriteFieldDef(fieldDef, writer, buffer);
                    }

                    // write rows
                    byte[][] rowData = new byte[fieldCount][];
                    bool[] isNullArr = new bool[fieldCount];

                    foreach (object item in baseTable.EnumerateItems())
                    {
                        // get row data and size
                        int rowDataSize = 0;

                        for (int i = 0; i < fieldCount; i++)
                        {
                            object value = props[i].GetValue(item);

                            if (value == null)
                            {
                                isNullArr[i] = true;
                            }
                            else
                            {
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = GetFieldData(fieldDef, value, rowData[i]);
                                rowData[i] = fieldData;
                                isNullArr[i] = false;
                                rowDataSize += (fieldDef.DataSize <= 0 ? 2 : 0) + fieldData.Length;
                            }

                            rowDataSize++; // null flag
                        }

                        // copy row data to the buffer
                        int fullRowSize = rowDataSize + 8;

                        if (buffer.Length < fullRowSize)
                            buffer = new byte[fullRowSize * 2];

                        int copyIndex = 0;
                        CopyUInt16(RowMarker, buffer, ref copyIndex);
                        CopyInt32(rowDataSize, buffer, ref copyIndex);

                        for (int i = 0; i < fieldCount; i++)
                        {
                            if (isNullArr[i])
                            {
                                buffer[copyIndex++] = 1;
                            }
                            else
                            {
                                buffer[copyIndex++] = 0;
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = rowData[i];

                                if (fieldDef.DataSize <= 0)
                                    CopyUInt16((ushort)fieldData.Length, buffer, ref copyIndex);

                                fieldData.CopyTo(buffer, copyIndex);
                                copyIndex += fieldData.Length;
                            }
                        }

                        ushort crc = CalcCRC16(buffer, 0, fullRowSize - 2);
                        CopyUInt16(crc, buffer, fullRowSize - 2);

                        // write row data
                        writer.Write(buffer, 0, fullRowSize);
                    }
                }
            }
            finally
            {
                writer?.Close();
            }
        }
    }
}