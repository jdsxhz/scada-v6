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
 * Module   : ScadaCommCommon
 * Summary  : Defines functionality to access the Communicator features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Data.Models;
using Scada.Log;
using System.Collections.Generic;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Defines functionality to access the Communicator features.
    /// <para>Определяет функциональность для доступа к функциям Коммуникатора.</para>
    /// </summary>
    public interface ICommContext
    {
        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        BaseDataSet BaseDataSet { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        IDictionary<string, object> SharedData { get; }


        /// <summary>
        /// Sends the telecontrol command to the current application.
        /// </summary>
        void SendCommand(TeleCommand cmd);
    }
}