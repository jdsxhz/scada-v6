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
 * Summary  : Represents a Communicator manager
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm
{
    /// <summary>
    /// The phrases used by Communicator and its modules.
    /// <para>Фразы, используемые Коммуникатором и его модулями.</para>
    /// </summary>
    public static class CommPhrases
    {
        // Drivers
        public static string ErrorInDriver { get; private set; }

        public static void Init()
        {
            // set phrases depending on locale, because the service logic supports only 2 languaages
            if (Locale.IsRussian)
            {
                ErrorInDriver = "Ошибка при вызове метода {0} драйвера {1}";
            }
            else
            {
                ErrorInDriver = "Error calling the {0} method of the {1} driver";
            }
        }
    }
}