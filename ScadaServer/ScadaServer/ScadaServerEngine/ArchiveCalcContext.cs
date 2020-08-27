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
 * Summary  : Implements the calculator context interface for calculating data in archives
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Server.Archives;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements the calculator context interface for calculating data in archives.
    /// <para>Реализует интерфейс контекста калькулятора для расчета данных в архивах.</para>
    /// </summary>
    internal class ArchiveCalcContext : ICalcContext
    {
        private readonly HistoricalArchiveLogic archiveLogic; // the historical archive logic
        private readonly DateTime timestamp;                  // the calculated data timestamp


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveCalcContext(HistoricalArchiveLogic archiveLogic, DateTime timestamp)
        {
            this.archiveLogic = archiveLogic ?? throw new ArgumentNullException("archiveLogic");
            this.timestamp = timestamp;
        }


        /// <summary>
        /// Gets the actual data of the input channel.
        /// </summary>
        CnlData ICalcContext.GetCnlData(int cnlNum)
        {
            return archiveLogic.GetCnlData(cnlNum, timestamp);
        }

        /// <summary>
        /// Gets the previous data of the input channel, if applicable.
        /// </summary>
        CnlData ICalcContext.GetPrevCnlData(int cnlNum)
        {
            return CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual timestamp of the input channel.
        /// </summary>
        DateTime ICalcContext.GetCnlTime(int cnlNum)
        {
            return timestamp;
        }

        /// <summary>
        /// Gets the previous timestamp of the input channel, if applicable.
        /// </summary>
        DateTime ICalcContext.GetPrevCnlTime(int cnlNum)
        {
            return DateTime.MinValue;
        }

        /// <summary>
        /// Sets the input channel data.
        /// </summary>
        void ICalcContext.SetCnlData(int cnlNum, CnlData cnlData)
        {
            archiveLogic.WriteCnlData(cnlNum, timestamp, cnlData);
        }
    }
}