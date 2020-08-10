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
* Basket controller used to interact with the Basket API service.
*/

/**
 * Function used to fetch the user basket.
 */
let getBasket = () => {
    let $scope = angular.element($('#page-top')).scope()
    if ($scope.user === undefined) {
        return
    }
    $.ajax({
        type: "GET",
        url: $scope.host + "/api/basket/" + $scope.user,
        headers: { 'Access-Control-Allow-Origin': '*', 'Authorization': "Bearer " + $scope.idToken },
        success: function (response) {
            $scope.$apply(function () {
                $scope.cart_prods = [];
                if (response != null) {
                    console.log("Got the cart \n", response);
                    for (var i in response) {
                        $scope.cart_prods.push({ ...$scope.prods_key[response[i].unicorn_id], basket_id: response[i]['basket_id'] });
                    }
                }
                $scope.cart_prods_length = $scope.cart_prods.length;
            });
        },
        error: function (err) {
            if (err.responseText === '') { //If cart becomes empty
                $scope.$apply(function () {
                    $scope.cart_prods = [];
                    $scope.cart_prods_length = $scope.cart_prods.length;
                });
            } else {
                console.log(err.statusText, " - Get cart error");
            }
        }
    })
}

/**
 * Function used to add unicorn product to the user cart.
 * @param {uuid} puuid - Unique identifier for the Unicorn product.
 */
let addToBasket = ( puuid ) =>{
    let $scope = angular.element($('#page-top')).scope()
    let cuuid = $scope.user;
    $.ajax({
        type: "POST",
        url: $scope.host + "/api/basket",
        headers: { 'Access-Control-Allow-Origin': '*', 'Authorization': "Bearer " + $scope.idToken},
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            "user_id": cuuid,
            "unicorn_id": puuid,
        }),

        success: function (response) {
            console.log("Added to cart \n", response)
            $('.modal').modal('hide')
            $.notify("Added to cart!", { className: "success", globalPosition: 'top center' });
            getBasket()
        },
        error: function (err) {
            console.log(err, "Error: Add cart");
        }
    });
}

/**
 * Function used to remove a product from the cart.
 * @param {uuid} basket_id - Unique identifier for the basket product to be deleted. 
 */
let deleteBasket = ( basket_id ) => {
    let $scope = angular.element($('#page-top')).scope()
    $.ajax({
        type: "DELETE",
        url: $scope.host + "/api/basket/" + basket_id,
        headers: { 'Access-Control-Allow-Origin': '*', 'Authorization': "Bearer " + $scope.idToken},
        success: function (response) {
            console.log(response, "Removed from cart"); 
            $.notify("Removed from cart!", { className: "success", globalPosition: 'top center' });
            getBasket();
            $('.modal').modal('hide');
        },
        error: function (err) {
            console.log(err, "Error: Remove from cart");
            $.notify("Failed to remove from cart", { className: "error", globalPosition: 'top center' });
        }
    });
}