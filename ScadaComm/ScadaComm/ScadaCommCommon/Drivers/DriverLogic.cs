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
 * Summary  : Represents the base class for driver logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Log;
using System;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for driver logic.
    /// <para>Представляет базовый класс логики драйвера.</para>
    /// </summary>
    public abstract class DriverLogic
    {
        private ICommContext commContext; // the Communicator context


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverLogic()
        {
            commContext = null;
            Log = null;
        }


        /// <summary>
        /// Gets or sets the driver log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets or sets the Communicator context.
        /// </summary>
        public ICommContext CommContext
        {
            get
            {
                return commContext;
            }
            set
            {
                commContext = value ?? throw new ArgumentNullException();
                Log = commContext.Log;
            }
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public abstract string Code { get; }


        /// <summary>
        /// Creates a new communication channel.
        /// </summary>
        public virtual ChannelLogic CreateChannel()
        {
            return null;
        }

        /// <summary>
        /// Creates a new device.
        /// </summary>
        public virtual DeviceLogic CreateDevice(int deviceNum)
        {
            return null;
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public virtual void OnServiceStart()
        {
        }

        /// <summary>
        /// Performs actions when the service stops.
        /// </summary>
        public virtual void OnServiceStop()
        {
        }
    }
}