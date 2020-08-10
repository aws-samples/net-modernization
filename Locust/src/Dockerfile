# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: MIT-0

FROM locustio/locust

WORKDIR /mnt/locust

COPY . .

EXPOSE 8089 5557

ENTRYPOINT ["locust","-f","/mnt/locust/locustfile.py"]