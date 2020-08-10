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
* Authentication controller for interacting with AWS Cognito.
*/

/**
 * Function used to Login a user with Cognito and return seeion information.
 * @param {string} email - User email used during signup. 
 * @param {string} password - User password used during signup.
 */
let congnitoLogin = (email, password) => {
    
    let $scope = angular.element($('#page-top')).scope()
    return new Promise((resolve,reject) => {
        var authenticationData = {
            Username: email,
            Password: password,
        }
        var authenticationDetails = new AmazonCognitoIdentity.AuthenticationDetails(authenticationData);
        var userData = {
            Username: email,
            Pool: $scope.userPool,
        }
        var cognitoUser = new AmazonCognitoIdentity.CognitoUser(userData);
        cognitoUser.authenticateUser(authenticationDetails, {
            onSuccess: function (result) {

                getAuthenticatedUser().then((result)=> {
                    resolve(result)
                }).catch((error) => {
                    reject(error)
                })
            },
            onFailure: function (err) {
                console.log("Error Authenticate user :" + err);
                reject(err);
            },
        });
    })
}

/**
 * Function used to register a new user with AWS cognito.
 * @param {string} email - Used email, everification email will be sent to this email id.
 * @param {string} password - User password, must follow Cognito constraints.
 * @param {string} fname - First name of the user.
 * @param {string} lname - Last name of the user.
 */
let congnitoSignUp = (email, password, fname, lname) => {
    let $scope = angular.element($('#page-top')).scope()
    return new Promise((resolve,reject)=> {
        const attributeList = [];

        const familyNameData = {
            Name: 'family_name',
            Value: lname,
        };

        const givenNameData = {
            Name: 'given_name',
            Value: fname,
        };

        var attributeFamilyName = new AmazonCognitoIdentity.CognitoUserAttribute(familyNameData);
        var attributeGivenName = new AmazonCognitoIdentity.CognitoUserAttribute(givenNameData);

        attributeList.push(attributeFamilyName);
        attributeList.push(attributeGivenName);

        $scope.userPool.signUp(email, password, attributeList, null, function (err, result) {
            if (err) {
                reject(err);
                console.log(err);
                return;
            }
            console.log('User ' + result.user.getUsername() + ' successfully signed up.');
            resolve("success");
            
        });
    })
}

/**
 * Function is used to fetch the current user and verify session activity.
 * Returns the user attributes formatted in a map.
 */
let getAuthenticatedUser = () => {
    let $scope = angular.element($('#page-top')).scope()
    return new Promise((resolve,reject) => {
        var cognitoUser =  $scope.userPool.getCurrentUser();
        if (cognitoUser == null) {
            console.log("User cannot be authenticated.");
            reject(new Error("Internal Error: User cannot be authenticated"));
            return;
        }

        cognitoUser.getSession(function (err, session) {
            if (err) {
                console.log("Session invalid.");
                reject(new Error("Internal Error: Session is invalid"));
                return;
            }

            var idToken = session.getIdToken().getJwtToken();

            cognitoUser.getUserAttributes(function (err, result) {
                
                if (err) {
                    console.log("Cannot get user attributes");
                    reject(new Error("Internal Error: Cannot get user attributes"));
                    return;
                }

                var resultMap = { 'idToken': idToken };

                result.map(item => {
                    resultMap[item.Name] = item.Value;
                });

                resolve(resultMap)
            });
        });
    })
}

/**
 *  Function used to logout the user and end the user session.
 */
let logout = () => {
    let $scope = angular.element($('#page-top')).scope()
    var cognitoUser =  $scope.userPool.getCurrentUser();
    cognitoUser.signOut()
}