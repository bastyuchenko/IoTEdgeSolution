using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CSharpFunction.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSharpFunction
{
    public static class CSharpFunction
    {
        [FunctionName("CSharpFunction")]
        public static async Task FilterMessageAndSendMessage(
            [EdgeHubTrigger("inputFromSensor")] Message messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output,
            ILogger logger
        )
        {
            const int temperatureThreshold = 20;
            byte[] messageBytes = messageReceived.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);

            if (!string.IsNullOrEmpty(messageString))
            {
                logger.LogInformation("Info: Received one non-empty message");
                // Get the body of the message and deserialize it.
                var messageBody = JsonConvert.DeserializeObject<MessageBody>(messageString);

                if (messageBody != null && messageBody.machine.temperature > temperatureThreshold)
                {
                    using (var pipeMessage = new Message(messageBytes))
                    {
                        foreach (KeyValuePair<string, string> prop in messageReceived.Properties)
                        {
                            pipeMessage.Properties.Add(prop.Key, prop.Value);
                        }
                        await output.AddAsync(pipeMessage);
                        logger.LogInformation("Info: Piped out the message");
                    }
                }
            }
        }
    }
}
