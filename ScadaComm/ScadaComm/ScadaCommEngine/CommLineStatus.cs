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
 * Module   : ScadaCommEngine
 * Summary  : Specifies the communication line statuses
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Specifies the communication line statuses.
    /// <para>Определяет статусы линии связи.</para>
    /// </summary>
    internal enum CommLineStatus
    {
        Undefined,
        Normal,
        Error,
        Terminating,
        Terminated
    }
}
