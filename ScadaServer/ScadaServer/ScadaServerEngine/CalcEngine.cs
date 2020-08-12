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
 * Module   : ScadaServerEngine
 * Summary  : Represents a mechanism for running calculator scripts and formulas
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using System;
using System.Text;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a mechanism for running calculator scripts and formulas.
    /// <para>Представляет механизм для запуска скриптов и формул калькулятора.</para>
    /// </summary>
    public abstract class CalcEngine
    {
        /// <summary>
        /// The buffer for converting channel values
        /// </summary>
        protected readonly byte[] valueBuffer;
        /// <summary>
        /// The calculation context.
        /// </summary>
        protected ICalcContext calcContext;
        /// <summary>
        /// The channel number for which the formula is calculated.
        /// </summary>
        protected int cnlNum;
        /// <summary>
        /// The input channel entity for which the formula is calculated.
        /// </summary>
        protected InCnl inCnl;
        /// <summary>
        /// The input channel data transmitted to the server before the calculation.
        /// </summary>
        protected CnlData initialCnlData;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CalcEngine()
        {
            valueBuffer = new byte[8];
            EndCalcCnlData();
            EndCalculation();
        }


        /// <summary>
        /// Gets the calculation context.
        /// </summary>
        public ICalcContext Context
        {
            get
            {
                return calcContext;
            }
        }

        /// <summary>
        /// Gets the channel number for which the formula is calculated.
        /// </summary>
        public int CnlNum
        {
            get
            {
                return cnlNum;
            }
        }

        /// <summary>
        /// Gets the input channel entity for which the formula is calculated.
        /// </summary>
        public InCnl InCnl
        {
            get
            {
                return inCnl;
            }
        }

        /// <summary>
        /// Gets the input channel value transmitted to the server before the calculation.
        /// </summary>
        public double Cnl
        {
            get
            {
                return initialCnlData.Val;
            }
        }

        /// <summary>
        /// Gets the input channel value transmitted to the server before the calculation.
        /// </summary>
        public double CnlVal
        {
            get
            {
                return initialCnlData.Val;
            }
        }

        /// <summary>
        /// Gets the input channel status transmitted to the server before the calculation.
        /// </summary>
        public int CnlStat
        {
            get
            {
                return initialCnlData.Stat;
            }
        }

        /// <summary>
        /// Gets the input channel data transmitted to the server before the calculation.
        /// </summary>
        public CnlData CnlData
        {
            get
            {
                return initialCnlData;
            }
        }


        /// <summary>
        /// Converts the specifined object into a channel value.
        /// </summary>
        protected double ToCnlVal(object obj)
        {
            int dataTypeID = inCnl?.DataTypeID ?? DataTypeID.Double;

            if (dataTypeID == DataTypeID.Double)
            {
                return Convert.ToDouble(obj);
            }
            else
            {
                Array.Clear(valueBuffer, 0, valueBuffer.Length);

                switch (dataTypeID)
                {
                    case DataTypeID.Int64:
                        BinaryConverter.CopyInt64(Convert.ToInt64(obj), valueBuffer, 0);
                        break;
                    case DataTypeID.UInt64:
                        BinaryConverter.CopyInt64((long)Convert.ToUInt64(obj), valueBuffer, 0);
                        break;
                    case DataTypeID.ASCII:
                        string s1 = Convert.ToString(obj);
                        int len1 = Math.Min(8, s1.Length);
                        Encoding.ASCII.GetBytes(s1, 0, len1, valueBuffer, 0);
                        break;
                    case DataTypeID.Unicode:
                        string s2 = Convert.ToString(obj);
                        int len2 = Math.Min(4, s2.Length);
                        Encoding.Unicode.GetBytes(s2, 0, len2, valueBuffer, 0);
                        break;
                }

                return BitConverter.ToDouble(valueBuffer, 0);
            }
        }

        /// <summary>
        /// Converts the specifined object into a channel data instance.
        /// </summary>
        protected CnlData ToCnlData(object obj)
        {
            return obj is CnlData cnlData ? 
                cnlData : 
                new CnlData(ToCnlVal(obj), initialCnlData.Stat);
        }


        /// <summary>
        /// Initializes the scripts.
        /// </summary>
        public virtual void InitScripts()
        {
        }

        /// <summary>
        /// Finalizes the scripts.
        /// </summary>
        public virtual void FinalizeScripts()
        {
        }

        /// <summary>
        /// Performs the necessary actions before the calculation.
        /// </summary>
        public void BeginCalculation(ICalcContext calcContext)
        {
            this.calcContext = calcContext;
        }

        /// <summary>
        /// Performs the necessary actions after the calculation.
        /// </summary>
        public void EndCalculation()
        {
            calcContext = null;
        }

        /// <summary>
        /// Performs the necessary actions before the calculation of the input channel data.
        /// </summary>
        public void BeginCalcCnlData(int cnlNum, InCnl inCnl, CnlData initialCnlData)
        {
            this.cnlNum = cnlNum;
            this.inCnl = inCnl;
            this.initialCnlData = initialCnlData;
        }

        /// <summary>
        /// Performs the necessary actions after the calculation of the input channel data.
        /// </summary>
        public void EndCalcCnlData()
        {
            cnlNum = 0;
            inCnl = null;
            initialCnlData = CnlData.Empty;
        }


        /// <summary>
        /// Gets the specified channel number for updating numbers on cloning.
        /// </summary>
        public int N(int n)
        {
            return n;
        }

        /// <summary>
        /// Gets the actual value of the formula input channel.
        /// </summary>
        public double Val()
        {
            return Data(cnlNum).Val;
        }

        /// <summary>
        /// Gets the actual value of the input channel n.
        /// </summary>
        public double Val(int n)
        {
            return Data(n).Val;
        }

        /// <summary>
        /// Sets the value of the input channel n.
        /// </summary>
        public double SetVal(int n, double val)
        {
            if (calcContext == null)
            {
                return double.NaN;
            }
            else
            {
                calcContext.SetCnlData(n, new CnlData(val, Stat(n)));
                return val;
            }
        }

        /// <summary>
        /// Gets the actual status of the formula input channel.
        /// </summary>
        public int Stat()
        {
            return Data(cnlNum).Stat;
        }

        /// <summary>
        /// Gets the actual status of the input channel n.
        /// </summary>
        public int Stat(int n)
        {
            return Data(n).Stat;
        }

        /// <summary>
        /// Sets the status of the input channel n.
        /// </summary>
        public int SetStat(int n, int stat)
        {
            if (calcContext == null)
            {
                return 0;
            }
            else
            {
                calcContext.SetCnlData(n, new CnlData(Val(n), stat));
                return stat;
            }
        }

        /// <summary>
        /// Gets the actual data of the formula input channel.
        /// </summary>
        public CnlData Data()
        {
            return Data(cnlNum);
        }

        /// <summary>
        /// Gets the actual data of the input channel n.
        /// </summary>
        public CnlData Data(int n)
        {
            return calcContext == null ? CnlData.Empty : calcContext.GetCnlData(n);
        }

        /// <summary>
        /// Sets the data of the input channel n.
        /// </summary>
        public double SetData(int n, double val, int stat)
        {
            if (calcContext == null)
            {
                return double.NaN;
            }
            else
            {
                calcContext.SetCnlData(n, new CnlData(val, stat));
                return val;
            }
        }

        /// <summary>
        /// Sets the data of the channel n.
        /// </summary>
        public double SetData(int n, CnlData cnlData)
        {
            if (calcContext == null)
            {
                return double.NaN;
            }
            else
            {
                calcContext.SetCnlData(n, cnlData);
                return cnlData.Val;
            }
        }

        /// <summary>
        /// Creates a new channel data instance.
        /// </summary>
        public CnlData NewData(double val, int stat)
        {
            return new CnlData(val, stat);
        }

        /// <summary>
        /// Gets the previous value of the formula input channel.
        /// </summary>
        public double PrevVal()
        {
            return PrevData(cnlNum).Val;
        }

        /// <summary>
        /// Gets the previous value of the input channel n.
        /// </summary>
        public double PrevVal(int n)
        {
            return PrevData(n).Val;
        }

        /// <summary>
        /// Gets the previous status of the formula input channel.
        /// </summary>
        public int PrevStat()
        {
            return PrevData(cnlNum).Stat;
        }

        /// <summary>
        /// Gets the previous status of the input channel n.
        /// </summary>
        public int PrevStat(int n)
        {
            return PrevData(n).Stat;
        }

        /// <summary>
        /// Gets the previous data of the input channel n.
        /// </summary>
        public CnlData PrevData(int n)
        {
            return calcContext == null ? CnlData.Empty : calcContext.GetPrevCnlData(n);
        }

        /// <summary>
        /// Gets the actual timestamp of the formula input channel.
        /// </summary>
        public DateTime Time()
        {
            return Time(cnlNum);
        }

        /// <summary>
        /// Gets the input timestamp of the input channel n.
        /// </summary>
        public DateTime Time(int n)
        {
            return calcContext == null ? DateTime.MinValue : calcContext.GetCnlTime(n);
        }

        /// <summary>
        /// Gets the previous timestamp of the formula input channel.
        /// </summary>
        public DateTime PrevTime()
        {
            return PrevTime(cnlNum);
        }

        /// <summary>
        /// Gets the previous timestamp of the input channel n.
        /// </summary>
        public DateTime PrevTime(int n)
        {
            return calcContext == null ? DateTime.MinValue : calcContext.GetPrevCnlTime(n);
        }
    }
}