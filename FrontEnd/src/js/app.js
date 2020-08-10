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

//Main controller code. Used to define the scope and app functionalities. 
var myApp = angular.module('unicorn_app', ['ngRoute', 'ngResource']).controller('unicornctrl', function ($scope, $rootScope, $timeout) {

    $scope.cart_prods_length = 0;
    //User scope variables
    $scope.login_email = "";
    $scope.login_password = "";
    $scope.login_name = "";
    $scope.signup_email = "";
    $scope.signup_password = "";
    $scope.signup_fname = "";
    $scope.signup_lname = "";
    //Basket scope variables
    $scope.carts = [];
    $scope.products = [];
    $scope.temp_cart = {};
    //Unicorn scope variables
    $scope.img_base64 = "";
    //Item color list
    $scope.colors = [
        '#007bff',
        '#6610f2',
        '#6f42c1',
        '#e83e8c',
        '#dc3545',
        '#fd7e14',
        '#ffc107',
        '#28a745',
        '#1abc9c',
        '#17a2b8',
        '#045fb4',
        '#6c757d',
        '#343a40',
        '#1abc9c',
        '#2c3e50',
        '#28a745',
        '#17a2b8',
        '#ffc107',
        '#dc3545',
        '#f8f9fa',

    ];
    

    //Initial function to load config and Init app
    $.getJSON("config.json", function (json) {

        console.log("API Endpoint : " + json['host']) 
        $scope.$apply(function () {
            $scope.host = json.host;
            $scope.poolData = {
                UserPoolId: json.userPoolId,
                ClientId: json.clientId
            };
            try {
                $scope.userPool = new AmazonCognitoIdentity.CognitoUserPool($scope.poolData)
            } catch (error) {
                $.notify("Cognito : " +error.message, { globalPosition: "top center", className: "info" });
            }
            $scope.init();
        });
    });

    //Init function to get unicorns and user info
    $scope.init = function () {
        
        getUnicorn().then((result) => {
            getAuthenticatedUser().then(function (result) {
                $scope.$apply(function () {
                    $scope.idToken = result['idToken'];
                    $scope.user = result['sub'];
                    $scope.user_email = result['email'];
                    $scope.first_name = (result['given_name'] == null ? "" : result['given_name']);
                    $scope.last_name = (result['family_name'] == null ? "" : result['family_name']);
                    $scope.getCart();
                    $.notify("Welcome back!", { className: "info", globalPosition: 'top center' });
                });
            }).catch(function (err) {
                console.log("User not logged in!");
            });
        }).catch((error) => {
            console.log("User not fetched.")
        })
    };

    //Event for login button click
    $('#login_button').click(function () {

        $scope.login($scope.login_email, $scope.login_password)
        .then(function () {    
            $scope.getCart();
            $('.modal').modal('hide');
            $.notify("Successfully Logged in", { globalPosition: "top center", className: "success" });
        
        }).catch(function (err) {
            
            console.log("Login error :" + err.message)
            $.notify("Failed:\n" +err.message, { globalPosition: "top center", className: "error" });
        
        })
    });

    //Event for Signup button click
    $('#signup_button').click(function () {

        congnitoSignUp($scope.signup_email, $scope.signup_password, $scope.signup_fname, $scope.signup_lname)
        .then(function () {
            
            $('.modal').modal('hide');
            $.notify("Successfully Signed up.\n Please confirm your account!", { className: "success", globalPosition: 'top center' });
        
        }).catch((function (err) {
           
            console.log("Signup error :" + err.message);
            $.notify("Failed:\n" + err.message, { globalPosition: "top center", className: "error"});
        
        }))
    });

    //Event for upload button click
    $('#upload_button').click(function () {
        var inpObj = document.getElementById("uploadForm");
        if (inpObj.checkValidity()) {
            console.log("Upload unicorn with Name:" + $scope.upload_uname + " Des:" + $scope.upload_udes + " Price:" + $scope.upload_uprice + " ImgName:" + $scope.upload_uimg.split('\\').pop().split('/').pop().split('.')[0]);
            addUnicorn($scope.img_base64).then((result) => {
                getUnicorn().catch((error) => {
                    console.log("Unicorn not fetched.")
                })
            }).catch((error) => {
                console.log("Unicorn not added.")
            })         
        }
    });

    //Event for file chosen    
    $("#upload_uimg").change(function(){
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function(event) {
                $('#uimg_preview').attr('src', event.target.result);
                $scope.img_base64 = event.target.result.split(",")[1];
            }
            reader.readAsDataURL(this.files[0]);
        }
    });

    //Logout uses and end session
    $scope.logout = function () {
        logout()
        location.reload()
    };

    //Login user based on email and password
    $scope.login = function (email, password) {
        return new Promise(function(resolve, reject) {
            congnitoLogin(email,password).then((result) => {

                $scope.$apply(function () {
                    $scope.idToken = result['idToken'];
                    $scope.user = result['sub'];
                    $scope.user_email = result['email'];
                    $scope.first_name = (result['given_name'] == null ? "" : result['given_name']);
                    $scope.last_name = (result['family_name'] == null ? "" : result['family_name']);
                    console.log("User scope set");
                    resolve("success")
                });
            }).catch((error)=> { 
                console.log(error);
                reject(error);
            })
        });
    };

    //Return color from list
    $scope.getcolor = function (index) {
        return $scope.colors[index];
    };

    //Add item to cart
    $scope.addToCart = function (puuid) {
        addToBasket(puuid)
    };

    //Event to show user cart
    $('#cartModal').on('show.bs.modal', function (e) {
        $scope.getCart();
    });

    //Fetch the cart contents
    $scope.getCart = function () {
        getBasket()
    };
    
    //Delete an item from the user cart
    $scope.deleteFromCart = function (basket_id) {
        deleteBasket(basket_id)
    };
});