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
 * Summary  : Holds archives classified by archive kinds
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using Scada.Server.Archives;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Holds archives classified by archive kinds.
    /// <para>Содержит архивы, классифицированные по видам.</para>
    /// </summary>
    internal class ArchiveHolder
    {
        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            CurrentArchives = new List<CurrentArchiveLogic>();
            HistoricalArchives = new List<HistoricalArchiveLogic>();
            EventArchives = new List<EventArchiveLogic>();
        }


        /// <summary>
        /// Gets the current archives.
        /// </summary>
        public List<CurrentArchiveLogic> CurrentArchives { get; private set; }

        /// <summary>
        /// Gets the historical archives.
        /// </summary>
        public List<HistoricalArchiveLogic> HistoricalArchives { get; private set; }

        /// <summary>
        /// Gets the event archives.
        /// </summary>
        public List<EventArchiveLogic> EventArchives { get; private set; }
    }
}