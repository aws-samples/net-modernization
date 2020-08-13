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

/*
* Unicorn controller used to interact with the Inventory API service.
*/

/**
 * Function used to fetch all the currently available Unicorn store inventory.
 */
let getUnicorn = ()=>{
    let $scope = angular.element($('#page-top')).scope()
    return new Promise((resolve,reject) => {
        $.ajax({
            type: "GET",
            url: $scope.host + "/api/unicorn",
            contentType: 'application/json',
            headers: { 'Access-Control-Allow-Origin': '*' },
            dataType: 'json',
            success: function (response) {
                $scope.products = response;
                $scope.prods_key = {};
    
                for (var i in $scope.products) {
                    $scope.prods_key[$scope.products[i].unicorn_id] = $scope.products[i];
                }
                $scope.$apply();
                resolve()
            },
            error: function (err) {
                $.notify("Failed to fetch Unicorns!", { globalPosition: "top center", className: "error" });
                console.log(err, "Error");
                reject(err)
            }
        });
    })
}

/**
 * Function used to upload unicorn to inventory
 */
let addUnicorn = ( imgData ) => {
    let $scope = angular.element($('#page-top')).scope();
    return new Promise((resolve,reject) => {
        $.ajax({
            type: "POST",
            url: $scope.host + "/api/unicorn",
            contentType: "application/json",
            headers: { 'Access-Control-Allow-Origin': '*', 'Authorization': "Bearer " + $scope.idToken },
            dataType: 'json',
            data: JSON.stringify({
                "Image": imgData,
                "Unicorn": {
                    "unicorn_id": uuidv1(),
                    "name": $scope.upload_uname,
                    "description": $scope.upload_udes,
                    "price": $scope.upload_uprice,
                    "image": $scope.upload_uimg.split('\\').pop().split('/').pop().split('.')[0] // get filename from path
                },
            }),
            success: function (response) {
                console.log("upload succeeded\n", response);
                $('.modal').modal('hide');
                $.notify("Uploaded to inventory!", { className: "success", globalPosition: 'top center' });
                resolve();
            },
            error: function (err) {
                console.log("upload failed",err);
                if(err.status == 400){
                    var unsafeTag = err.responseText;
                    $.notify("Failed to upload!\n The picture contains unsafe content: " + unsafeTag + ".", { className: "error", globalPosition: 'top center' });
                    reject()
                }else{
                    $.notify("Failed to upload!", { className: "error", globalPosition: 'top center' });
                    reject()
                }
                
            }
        });
    })
}

/**
 * Function used to delete unicorn from the inventory
 */
let deleteUnicorn = ( unicorn_id ) => {
    let $scope = angular.element($('#page-top')).scope();
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "DELETE",
            url: $scope.host + "/api/unicorn/" + unicorn_id,
            contentType: 'application/json',
            headers: { 'Access-Control-Allow-Origin': '*', 'Authorization': "Bearer " + $scope.idToken },
            dataType: 'json',
            success: function (response) {
                console.log("Unicorn deleted!");
                $.notify("Removed from inventory!", { className: "success", globalPosition: 'top center' });
                $('.modal').modal('hide');
                resolve();
            },
            error: function (err) {
                console.log(err, "Error: Fail to delete unicorn");
                $.notify("Failed to remove from inventory", { className: "error", globalPosition: 'top center' });
                reject(err);
            }
        });
    })
}
