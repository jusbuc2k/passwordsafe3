define(['knockout', 'http', 'crypto'], function(ko, http, crypto) {
    'use strict';
    
    return function PasswordEditModel(router) {
        var cryptoData;

        this.name = ko.observable();
        this.description = ko.observable();
        this.data = ko.observable();

        this.unlockPassword = ko.observable();

        this.unlockClicked = function() {
            var userKey = crypto.keyFromPassword(this.unlockPassword());
            var masterKey = crypto.decrypt(userKey, cryptoData);

            
            
        }.bind(this);

        this.load = function(routeValues) {
            if (routeValues.id) {
                return $.ajax({
                    method: "get",
                    url: "/Password/GetPassword/" + routeValues.id,
                    dataType: "json"
                }).then(function(response){
                    this.vaultID = response.vaultID;
                    this.name(response.name);
                }.bind(this));
            } else {
                return true;
            }
        }

        this.save = function() {
            if (this.passwordID === 0) {
                $.ajax({
                    method: "post",
                    url: "/Password/SetPassword/" + this.passwordID,
                    data: {
                        name: this.name(),
                        description: this.description()
                    }
                }).then(function(response){
                    router.navigate({ view: "Home" });
                });
            }
        };

        this.passwordID = 0;
        this.name = ko.observable();
        this.description = ko.observable();
        this.plainData = ko.observable();
    }

});