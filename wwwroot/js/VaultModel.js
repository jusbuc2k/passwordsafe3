/// <reference path="../lib/knockout/dist/knockout.debug.js" />
/// <reference path="../lib/knockroute/dist/knockroute.js" />

define(['knockout', 'knockroute', 'crypto'], function(ko, kr, crypto) {
    'use strict';

    function passwordMap(password){
        return {
            name: password.name,
            description: password.description,
            data: password.data,
            plainData: ko.observable()
        }
    }
    
    return function VaultModel() {
        var encryptedMasterKey;

        function decrypt(data, unlockPassword) {
            var salt = crypto.extractIV(password.data);
            var userKey = crypto.keyFromPassword(password, salt);
            var masterKey = crypto.decrypt(userKey, encryptedMasterKey);
            var plainBytes = crypto.decrypt(masterKey, data);
            var plainText = new TextDecoder("utf-8").decode(plainBytes);

            return plainText;
            //TODO: JSON.parse
        }

        this.decrypt = function(password){
            password.plainData(decrypt(password.data, prompt("Unlock Password")));
        };

        this.load = function(routeValues) {
            return $.ajax({
                method: "get",
                url: "/Password/ListPasswords/" + routeValues.id,
                dataType: "json"
            }).then(function(response){
                this.title(response.vault.name);
                this.passwords(response.passwords);
                encryptedMasterKey = response.masterKey;
            }.bind(this), function(err){
                console.error(err);                
            });
        }

        this.title = ko.observable();
        this.passwords = ko.observableArray();
        this.selectedPassword = ko.observable();
    }

});