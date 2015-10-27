﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.Intune
{
    using System;
    using System.Linq;
    using System.Management.Automation;
    using RestClient;

    /// <summary>
    /// Cmdlet to get existing resources.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureRmIntuneAndroidMAMPolicyApp"), OutputType(typeof(PSObject))]
    public sealed class GetIntuneAndroidMAMPolicyAppCmdlet : IntuneBaseCmdlet
    {
        /// <summary>
        /// Gets the policy Name
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "The policy name for the apps to fetch.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Contains the cmdlet's execution logic.
        /// </summary>
        protected override void ProcessRecord()
        {
            Action action = () =>
            {
                var androidAppsForPolicy = this.IntuneClient.GetAppForAndroidMAMPolicy(this.AsuHostName, Name);
                if (androidAppsForPolicy != null && androidAppsForPolicy.Value.Count > 0)
                {
                    for (int start = 0; start < androidAppsForPolicy.Value.Count; start += IntuneConstants.BATCH_SIZE)
                    {
                        var batch = androidAppsForPolicy.Value.Skip(start).Take(IntuneConstants.BATCH_SIZE);
                        this.WriteObject(batch, enumerateCollection: true);
                    }
                }
                else
                {
                    this.WriteObject("0 items returned");
                }
            };

            base.SafeExecutor(action);
        }
    }
}
