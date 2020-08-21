/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
// using Amazon.Rekognition; // Uncomment this line after installing the Rekognition NuGet Package for the AI Content Moderation extra credit lab
// using Amazon.Rekognition.Model; // Uncomment this line after installing the Rekognition NuGet Package for the AI Content Moderation extra credit lab
using InventoryService.Interface;
using Microsoft.AspNetCore.Identity;

namespace InventoryService.Service
{
    public class RekognitionService : IRekognitionService
    {
        /// <inheritdoc />
        public string GetContentModerationLabels(byte[] userUpload)
        {
            // Uncomment the code below for the AI Content Moderation extra credit lab
            /*
            Image uploadedImage = new Image();
            uploadedImage.Bytes = new System.IO.MemoryStream(userUpload);
            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectModerationLabelsRequest detectModerationLabelsRequest = new DetectModerationLabelsRequest()
            {
                Image = uploadedImage,
                MinConfidence = 70F,
            };

            DetectModerationLabelsResponse detectModerationLabelsResponse = rekognitionClient.DetectModerationLabelsAsync(detectModerationLabelsRequest).GetAwaiter().GetResult();
            if (detectModerationLabelsResponse.ModerationLabels.Count > 0)
            {
                return detectModerationLabelsResponse.ModerationLabels[0].Name;
            }
            */
            return null;
        }
    }
}