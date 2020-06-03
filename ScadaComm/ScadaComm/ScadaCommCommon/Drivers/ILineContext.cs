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
 * Summary  : Defines functionality to access the communication line features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Data.Models;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Defines functionality to access the communication line features.
    /// <para>Определяет функциональность для доступа к функциям линии связи.</para>
    /// </summary>
    public interface ILineContext
    {
        /// <summary>
        /// Gets the communication line configuration.
        /// </summary>
        LineConfig LineConfig { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the shared data of the communication line.
        /// </summary>
        IDictionary<string, object> SharedData { get; }


        /// <summary>
        /// Sends the telecontrol command to the current communication line.
        /// </summary>
        void SendCommand(TeleCommand cmd);
    }
}