﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Imagine.Uwp.Cognitive
{
    /// <summary>
    /// HttpHelperResponse instance to hold data from Http Response.
    /// </summary>
    internal class HttpHelperResponse : IDisposable
    {
        private HttpResponseMessage _responseMessage = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperResponse"/> class.
        /// </summary>
        /// <param name="response">Http Response <see cref="HttpResponseMessage"/> being wrapped.</param>
        public HttpHelperResponse(HttpResponseMessage response)
        {
            _responseMessage = response;
        }

        /// <summary>
        /// Gets the HTTP response StatusCode.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return _responseMessage.StatusCode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the HTTP response was successful.
        /// </summary>
        public bool Success
        {
            get
            {
                return _responseMessage.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets the HTTP response header collection.
        /// </summary>
        public HttpResponseHeaderCollection Headers
        {
            get
            {
                return _responseMessage.Headers;
            }
        }

        /// <summary>
        /// Gets content from HTTP response.
        /// </summary>
        public IHttpContent Content
        {
            get
            {
                return _responseMessage.Content;
            }
        }

        /// <summary>
        /// Reads the Content as string and returns it to the caller.
        /// </summary>
        /// <returns>string content</returns>
        public Task<string> GetTextResultAsync()
        {
            if (this.Content == null)
            {
                return Task.FromResult<string>(null);
            }

            return Content.ReadAsStringAsync().AsTask();
        }

        /// <summary>
        /// Dispose underlying content
        /// </summary>
        public void Dispose()
        {
            try
            {
                _responseMessage.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }
        }
    }
}
