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
 * Summary  : Represents a device as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a device as an entity of the configuration database.
    /// <para>Представляет КП как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class Device
    {
        public int DeviceNum { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? DevTypeID { get; set; }

        public int? NumAddress { get; set; }

        public string StrAddress { get; set; }

        public int? CommLineNum { get; set; }

        public string Descr { get; set; }
    }
}