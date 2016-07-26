define(['knockout','knockroute', 'crypto'], function(ko, kr, crypto) {
    'use strict';
    
    return function PasswordEditModel(router) {

        this.load = function(routeValues){
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