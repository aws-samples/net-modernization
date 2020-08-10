"""
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
"""

import random, uuid
from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    wait_time = between(5, 9)

    @task
    def get_unicorns(self):
        """
        This function will get all the unicorns.
        It will test /api/unicorn with GET method.
        """
        self.client.get("/api/unicorn", name="/api/unicorn")

    @task
    def get_unicorn(self):
        """
        This function will get a specific unicorn using its id.
        It will test /api/unicorn/id with GET method.
        """
        unicorn_id = random.choice(self.unicorn_ids)
        self.client.get(f"/api/unicorn/{unicorn_id}", name="/api/unicorn/id")


    def on_start(self):
        """
        This function prepare stuff for the above 4 tests. 
        """
        # get unicorn ids
        unicorns = self.client.get("/api/unicorn", name="/api/unicorn").json()
        self.unicorn_ids = [unicorn["unicorn_id"] for unicorn in unicorns]


